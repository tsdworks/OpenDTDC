#ifndef _COMM_H
#define _COMM_H

#include "fwinfo.h"
#include "halports.h"
#include "led.h"

// 类型定义
typedef uint8_t COMM_STA;
typedef uint8_t COMM_PACKSIZE;
typedef uint8_t COMM_INDEX;

// 数据收发时间相关
#define COMM_SEND_FREQ 40
#define COMM_RECV_TIMEOUT 500
#define COMM_ON_CHANGE_BEEP_DURATION 100

// 状态定义
#define COMM_STATE_NONE 0
#define COMM_STATE_WAIT_FOR_CONNECT 1
#define COMM_STATE_CONNECTED 2

// 数据包长度
#define COMM_PACK_MIN_SIZE 5
#define COMM_CMD_LINE_MAX_SIZE 15

// 数据包特殊值
#define COMM_EMPTY ""
#define COMM_CON_REQ "Connect"
#define COMM_CON_RESP "Controller"
#define COMM_START_TRANS_REQ "Start"
#define COMM_HEAD 0x6a
#define COMM_TAIL 0xa6
#define COMM_NODATA 0xff

// 数据包特殊位定义
#define COMM_HEAD_POS 0
#define COMM_TYPE_POS 1
#define COMM_VER_POS 2
#define COMM_NUM_POS 3

// 缓冲长度
#define COMM_RECV_BUFFER_SIZE 128
#define COMM_SEND_BUFFER_SIZE 128

// 缓冲区
extern uint8_t comm_recv_buffer[COMM_RECV_BUFFER_SIZE];
extern uint8_t comm_send_buffer[COMM_SEND_BUFFER_SIZE];

/*
函数功能：数据接收
*/
void comm_receive_handler(uint32_t time);

/*
函数功能：数据发送
*/
void comm_send_handler(uint32_t time);

#endif