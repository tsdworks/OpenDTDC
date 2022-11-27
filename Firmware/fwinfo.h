#ifndef _FWINFO_H
#define _FWINFO_H

#define AVR_BOARD 1

#include "arduino.h"
#include "HardwareSerial.h"
#include "inttypes.h"

#if AVR_BOARD
#include "avr/wdt.h"
#endif

// 设备型号 uint8
#define FIRMWARE_DEVICE_MODEL 0x00
// 固件版本 uint8
#define FIRMWARE_VERSION 0x01

// 串口波特率
#define BAUDRATE 250000l

// 控制器数量
#define CTR_NUM 35

// 执行器序列，仅输出
#define ACT0 0x00 // 0
#define ACT1 0x01 // 1
#define ACT2 0x02 // 2

// 蜂鸣器控制器序列，仅输出
#define BP0 0x03 // 3

// 扩展指示灯控制器序列，仅输出
#define L0 0x04 // 4
#define L1 0x05 // 5
#define L2 0x06 // 6
#define L3 0x07 // 7
#define L4 0x08 // 8

// 模拟量控制器序列，仅输入
#define ANA0 0x09 // 9
#define ANA1 0x0a // 10
#define ANA2 0x0b // 11

// 双值数字量控制器序列，仅输入
#define DDS01 0x0c // 12
#define DDS02 0x0d // 13
#define DDS11 0x0e // 14
#define DDS12 0x0f // 15
#define DDS21 0x10 // 16
#define DDS22 0x11 // 17
#define DDS31 0x12 // 18
#define DDS32 0x13 // 19
#define DDS41 0x14 // 20
#define DDS42 0x15 // 21

// 单值数字量控制器序列，输入输出
#define SDS0 0x16 // 22
#define SDS1 0x17 // 23
#define SDS2 0x18 // 24
#define SDS3 0x19 // 25
#define SDS4 0x1a // 26
#define SDS5 0x1b // 27

// 单值PWM支持数字量控制器序列，输入输出
#define SDS6 0x1c  // 28
#define SDS7 0x1d  // 29
#define SDS8 0x1e  // 30
#define SDS9 0x1f  // 31
#define SDS10 0x20 // 32
#define SDS11 0x21 // 33

// 串口
#define SER0 0x22 // 34

#endif
