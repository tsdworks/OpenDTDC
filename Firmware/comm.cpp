#include "comm.h"

// 缓冲区
uint8_t comm_recv_buffer[COMM_RECV_BUFFER_SIZE] = {0x00};
uint8_t comm_send_buffer[COMM_SEND_BUFFER_SIZE] = {0x00};

// 状态
volatile COMM_STA state = COMM_STATE_WAIT_FOR_CONNECT;
volatile COMM_STA last_state = COMM_STATE_NONE;

// 指令字缓存
String command_line_buffer = COMM_EMPTY;

void comm_receive_handler(uint32_t time)
{
    // 上一次接收时间
    static uint32_t last_received_time = 0;

    if (state == COMM_STATE_WAIT_FOR_CONNECT)
    {
        // 未连接时 LED 闪烁
        if (last_state != state)
        {
            led_flash(L4, LED_DEFAULT_FLASH_DURATION);
        }

        last_state = state;

        // 接收字符串指令
        while (_HAL_GET_SER_INS(SER0).available() > 0)
        {
            command_line_buffer += (char)_HAL_GET_SER_INS(SER0).read();

            if (command_line_buffer.length() > COMM_CMD_LINE_MAX_SIZE)
            {
                command_line_buffer = COMM_EMPTY;
            }
        }

        if (command_line_buffer.indexOf(COMM_CON_REQ) != -1)
        {
            _HAL_GET_SER_INS(SER0).print(COMM_CON_RESP);

            command_line_buffer = COMM_EMPTY;
        }
        else if (command_line_buffer.indexOf(COMM_START_TRANS_REQ) != -1)
        {
            _HAL_GET_SER_INS(SER0).flush();

            command_line_buffer = COMM_EMPTY;

            led_turn_off_all();

            _HAL_SET_VALUE(BP0, DEV_HIGH);
            delay(COMM_ON_CHANGE_BEEP_DURATION);
            _HAL_SET_VALUE(BP0, DEV_LOW);

            last_received_time = time;

            state = COMM_STATE_CONNECTED;
        }
    }
    else if (state == COMM_STATE_CONNECTED)
    {
        last_state = state;

        // 已接收的字节数量
        static COMM_PACKSIZE size = 0;

        // 本次可接收的字节数量
        COMM_PACKSIZE current_size = _HAL_GET_SER_INS(SER0).available();

        // 如果溢出，清空状态
        if (size + current_size > COMM_RECV_BUFFER_SIZE)
        {
            size = 0;
        }

        // 如果存在可读字节
        if (current_size > 0)
        {
            last_received_time = time;

            // 包头包尾位置
            COMM_INDEX packet_start_index = 0;
            COMM_INDEX packet_end_index = 0;

            // 是否找到头尾
            uint8_t find_packet_start = FALSE;
            uint8_t find_packet_end = FALSE;

            // 读取字节
            _HAL_GET_SER_INS(SER0).readBytes(comm_recv_buffer + size, current_size);

            // 更新已接收字节长度
            size += current_size;

            // 遍历已经接收到的数据，寻找包头包尾
            for (COMM_INDEX i = size - 1; i >= 0; i--)
            {
                if (comm_recv_buffer[i] == COMM_TAIL)
                {
                    packet_end_index = i;

                    find_packet_end = TRUE;

                    break;
                }
            }

            if (find_packet_end && packet_end_index >= 1)
            {
                for (COMM_INDEX i = packet_end_index - 1; i >= 0; i--)
                {
                    if (comm_recv_buffer[i] == COMM_HEAD)
                    {
                        packet_start_index = i;

                        find_packet_start = TRUE;

                        break;
                    }
                }
            }

            // 遍历数据包，获取数据
            if ((find_packet_start & find_packet_end) &&
                packet_end_index - packet_start_index + 1 >= COMM_PACK_MIN_SIZE &&
                (packet_end_index - packet_start_index + 1 - COMM_PACK_MIN_SIZE) == comm_recv_buffer[packet_start_index + COMM_NUM_POS])
            {
                for (COMM_PACKSIZE i = packet_start_index + COMM_NUM_POS + 1; i < packet_end_index; i++)
                {
                    // 判断是否可用
                    if (comm_recv_buffer[i] != COMM_NODATA)
                    {
                        // 计算 GPIO 编号
                        DEV_ID device_addr = i - (packet_start_index + COMM_NUM_POS + 1);

                        _HAL_SET_VALUE(device_addr, comm_recv_buffer[i]);
                    }
                }

                size = 0;
            }
        }
        else if (abs(time - last_received_time) > COMM_RECV_TIMEOUT)
        {
            size = 0;

            _HAL_GET_SER_INS(SER0).flush();

            _HAL_SET_VALUE(BP0, DEV_HIGH);
            delay(COMM_ON_CHANGE_BEEP_DURATION);
            _HAL_SET_VALUE(BP0, DEV_LOW);

            // 状态转移为未连接
            state = COMM_STATE_WAIT_FOR_CONNECT;
        }
    }
}

void comm_send_handler(uint32_t time)
{
    // 上一次发送时间
    static uint32_t last_send_time = 0;

    // 发送频率
    static uint32_t send_duration = (uint32_t)(1000.0f / COMM_SEND_FREQ);

    if (state == COMM_STATE_CONNECTED &&
        abs(time - last_send_time) > send_duration)
    {
        // 准备发送的字节数量
        COMM_PACKSIZE size = 0;

        // 包头
        comm_send_buffer[COMM_HEAD_POS] = COMM_HEAD;
        // 类型
        comm_send_buffer[COMM_TYPE_POS] = FIRMWARE_DEVICE_MODEL;
        // 版本
        comm_send_buffer[COMM_VER_POS] = FIRMWARE_VERSION;

        // 保留数量位，稍后填充
        size += 4;

        // 获取实时数据
        for (DEV_ID i = 0; i < CTR_NUM; i++)
        {
            // 预先填充无数据
            comm_send_buffer[size] = COMM_NODATA;

            // 读取数据
            _HAL_GET_VALUE(comm_send_buffer[size], i);

            size++;
        }

        // 数量位
        comm_send_buffer[COMM_NUM_POS] = size - 4;

        // 包尾
        comm_send_buffer[size++] = COMM_TAIL;

        if (_HAL_GET_SER_INS(SER0).availableForWrite() >= size)
        {
            // 发送数据
            _HAL_GET_SER_INS(SER0).write(comm_send_buffer, size);

            // 记录发送时间
            last_send_time = time;
        }
    }
    else
    {
        last_send_time = 0;
    }
}