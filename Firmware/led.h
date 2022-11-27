#ifndef _LED_H
#define _LED_H

#include "fwinfo.h"
#include "halports.h"

#define LED_DEFAULT_FLASH_DURATION 800

// 打开所有 LED 灯
void led_turn_on_all();

// 关闭所有 LED 灯
void led_turn_off_all();

// 设置 LED 闪烁
void led_flash(uint8_t id, uint16_t duration);

// LED 处理程序
void led_handler(uint32_t time);

#endif