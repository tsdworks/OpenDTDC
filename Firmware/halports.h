#ifndef _HALPORTS_H
#define _HALPORTS_H

#include "fwinfo.h"

// 类型定义
typedef uint8_t DEV_ID;
typedef uint8_t DEV_MODE;
typedef uint8_t DEV_PIN;
typedef uint8_t DEV_RESULT;
typedef uint8_t DEV_VALUE;

// 通用常量定义
#define FALSE 0x00
#define TRUE 0x01
#define DEV_OP_FAIL 0x00
#define DEV_OP_OK 0x01
#define DEV_LOW 0x00
#define DEV_HIGH 0x01
#define DEV_NONE 0xff

// IO类型定义
#define DEV_MODE_UNDEFINED 0x00
#define DEV_MODE_INPUT 0x01
#define DEV_MODE_INPUT_PULLUP 0x02
#define DEV_MODE_INPUT_ANALOG 0x03
#define DEV_MODE_OUTPUT 0x04
#define DEV_MODE_OUTPUT_ANALOG 0x05
#define DEV_MODE_SERIAL 0x06

// 读取参数
#define DEV_SAMPLING_NUM 2

// 串口
#define SERIAL0 0x00
#define SERIAL1 0x01
#define SERIAL2 0x02
#define SERIAL3 0x03

// 控制器序列 IO 序号
// 执行器序列
#define ACT0_S 22
#define ACT1_S 37
#define ACT2_S 36

// 蜂鸣器控制器序列
#define BP0_S 24

// 扩展指示灯控制器序列
#define L0_S 7
#define L1_S 6
#define L2_S 5
#define L3_S 4
#define L4_S 3

// 模拟量控制器序列
#define ANA0_S A0
#define ANA1_S A1
#define ANA2_S A2

// 双值数字量控制器序列
#define DDS0_S1 53
#define DDS0_S2 52
#define DDS1_S1 51
#define DDS1_S2 50
#define DDS2_S1 49
#define DDS2_S2 48
#define DDS3_S1 47
#define DDS3_S2 46
#define DDS4_S1 45
#define DDS4_S2 44

// 单值数字量控制器序列
#define SDS0_S 42
#define SDS1_S 43
#define SDS2_S 40
#define SDS3_S 41
#define SDS4_S 38
#define SDS5_S 39
#define SDS6_S 13
#define SDS7_S 12
#define SDS8_S 11
#define SDS9_S 10
#define SDS10_S 9
#define SDS11_S 8

// 控制器模式存储
const DEV_MODE hal_device_mode_list[CTR_NUM] =
    {
        // ACT0 - ACT2
        DEV_MODE_OUTPUT, DEV_MODE_OUTPUT, DEV_MODE_OUTPUT,
        // BP0
        DEV_MODE_OUTPUT,
        // L0 - L4
        DEV_MODE_OUTPUT, DEV_MODE_OUTPUT, DEV_MODE_OUTPUT, DEV_MODE_OUTPUT, DEV_MODE_OUTPUT,
        // ANA0 - ANA2
        DEV_MODE_INPUT_ANALOG, DEV_MODE_INPUT_ANALOG, DEV_MODE_INPUT_ANALOG,
        // DDS01 - DDS42
        DEV_MODE_INPUT_PULLUP, DEV_MODE_INPUT_PULLUP,
        DEV_MODE_INPUT_PULLUP, DEV_MODE_INPUT_PULLUP,
        DEV_MODE_INPUT_PULLUP, DEV_MODE_INPUT_PULLUP,
        DEV_MODE_INPUT_PULLUP, DEV_MODE_INPUT_PULLUP,
        DEV_MODE_INPUT_PULLUP, DEV_MODE_INPUT_PULLUP,
        // SDS0 - SDS5
        DEV_MODE_INPUT_PULLUP, DEV_MODE_INPUT_PULLUP, DEV_MODE_INPUT_PULLUP,
        DEV_MODE_INPUT_PULLUP, DEV_MODE_INPUT_PULLUP, DEV_MODE_INPUT_PULLUP,
        // SDS6 - SDS11
        DEV_MODE_INPUT_PULLUP, DEV_MODE_INPUT_PULLUP, DEV_MODE_INPUT_PULLUP,
        DEV_MODE_INPUT_PULLUP, DEV_MODE_INPUT_PULLUP, DEV_MODE_INPUT_PULLUP,
        // SER0
        DEV_MODE_SERIAL};

// 控制器引脚映射存储
const DEV_PIN hal_device_pin_list[CTR_NUM] =
    {
        // ACT0 - ACT2
        ACT0_S, ACT1_S, ACT2_S,
        // BP0
        BP0_S,
        // L0 - L4
        L0_S, L1_S, L2_S, L3_S, L4_S,
        // ANA0 - ANA2
        ANA0_S, ANA1_S, ANA2_S,
        // DDS01 - DDS42
        DDS0_S1, DDS0_S2,
        DDS1_S1, DDS1_S2,
        DDS2_S1, DDS2_S2,
        DDS3_S1, DDS3_S2,
        DDS4_S1, DDS4_S2,
        // SDS0 - SDS5
        SDS0_S, SDS1_S, SDS2_S, SDS3_S, SDS4_S, SDS5_S,
        // SDS6 - SDS11
        SDS6_S, SDS7_S, SDS8_S, SDS9_S, SDS10_S, SDS11_S,
        // SER0
        SERIAL0};

// 通用宏
#define _HAL_GET_SER_INS(device_addr) \
    (                                 \
        hal_device_pin_list[device_addr] == SERIAL0 ? Serial : (hal_device_pin_list[device_addr] == SERIAL1 ? Serial1 : (hal_device_pin_list[device_addr] == SERIAL2 ? Serial2 : (hal_device_pin_list[device_addr] == SERIAL3 ? Serial3 : Serial))))
#define _HAL_SET_MODE(device_addr, device_mode)                          \
    if (hal_device_mode_list[device_addr] == DEV_MODE_OUTPUT ||          \
        hal_device_mode_list[device_addr] == DEV_MODE_OUTPUT_ANALOG)     \
    {                                                                    \
        pinMode(hal_device_pin_list[device_addr], OUTPUT);               \
    }                                                                    \
    else if (hal_device_mode_list[device_addr] == DEV_MODE_INPUT)        \
    {                                                                    \
        pinMode(hal_device_pin_list[device_addr], INPUT);                \
    }                                                                    \
    else if (hal_device_mode_list[device_addr] == DEV_MODE_INPUT_PULLUP) \
    {                                                                    \
        pinMode(hal_device_pin_list[device_addr], INPUT_PULLUP);         \
    }                                                                    \
    else if (hal_device_mode_list[device_addr] == DEV_MODE_SERIAL)       \
    {                                                                    \
        _HAL_GET_SER_INS(device_addr).begin(BAUDRATE);                   \
    }
#define _HAL_GET_DIGITAL_SET_VALUE(device_value) (constrain(device_value, DEV_LOW, DEV_HIGH))
#define _HAL_GET_ANALOG_SET_VALUE(device_value) (map(constrain(device_value, DEV_LOW, 100), DEV_LOW, 100, DEV_LOW, 255))
#define _HAL_SET_VALUE(device_addr, device_value)                                                 \
    if (hal_device_mode_list[device_addr] == DEV_MODE_OUTPUT)                                     \
    {                                                                                             \
        digitalWrite(hal_device_pin_list[device_addr], _HAL_GET_DIGITAL_SET_VALUE(device_value)); \
    }                                                                                             \
    else if (hal_device_mode_list[device_addr] == DEV_MODE_OUTPUT_ANALOG)                         \
    {                                                                                             \
        analogWrite(hal_device_pin_list[device_addr], _HAL_GET_ANALOG_SET_VALUE(device_value));   \
    }
#define _HAL_GET_VALUE(result, device_addr)                                            \
    if (hal_device_mode_list[device_addr] == DEV_MODE_INPUT_ANALOG)                    \
    {                                                                                  \
        int value_sum = 0;                                                             \
        for (int I = 0; I < DEV_SAMPLING_NUM; I++)                                     \
        {                                                                              \
            value_sum += analogRead(hal_device_pin_list[device_addr]);                 \
        }                                                                              \
        result = (byte)map(value_sum / DEV_SAMPLING_NUM, DEV_LOW, 1023, DEV_LOW, 100); \
    }                                                                                  \
    else if (hal_device_mode_list[device_addr] == DEV_MODE_INPUT)                      \
    {                                                                                  \
        result = digitalRead(hal_device_pin_list[device_addr]);                        \
    }                                                                                  \
    else if (hal_device_mode_list[device_addr] == DEV_MODE_INPUT_PULLUP)               \
    {                                                                                  \
        result = !digitalRead(hal_device_pin_list[device_addr]);                       \
    }

/*
函数功能：初始化设备
*/
void hal_device_init();

#endif