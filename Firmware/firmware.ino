#include "fwinfo.h"
#include "halports.h"
#include "comm.h"
#include "led.h"

#if AVR_BOARD == 0
void watchdogSetup()
{
    watchdogEnable(1000);
}
#endif

void setup()
{
    // 初始化 GPIO
    hal_device_init();

// 使能看门狗
#if AVR_BOARD
    wdt_enable(WDTO_1S);
#endif
}

void loop()
{
    // 看门狗喂狗
#if AVR_BOARD
    wdt_reset();
#else
    watchdogReset();
#endif

    // 获得系统时间
    uint32_t current_time_ms = millis();

    // 接收处理
    comm_receive_handler(current_time_ms);

    // 发送处理
    comm_send_handler(current_time_ms);

    // LED 闪烁处理
    led_handler(current_time_ms);
}
