#include "led.h"

// LED 闪烁使能
uint8_t led_flash_enable = 0;

// 标记为闪烁的 LED 编号
DEV_ID led_flash_id = L0;

// 闪烁 LED 当前状态
DEV_VALUE led_flash_value = DEV_LOW;

// LED 闪烁延时
uint16_t led_flash_duration = LED_DEFAULT_FLASH_DURATION;

void led_turn_on_all()
{
    for (DEV_ID i = L0; i <= L4; i++)
    {
        _HAL_SET_VALUE(i, DEV_HIGH);
    }

    led_flash_enable = 0;
}

void led_turn_off_all()
{
    for (DEV_ID i = L0; i <= L4; i++)
    {
        _HAL_SET_VALUE(i, DEV_LOW);
    }

    led_flash_enable = 0;
}

void led_flash(uint8_t id, uint16_t duration)
{
    led_turn_off_all();

    led_flash_id = id;
    led_flash_duration = duration;
    led_flash_value = DEV_LOW;
    led_flash_enable = 1;
}

void led_handler(uint32_t time)
{
    static uint32_t last_flash_time = 0;

    if (led_flash_enable &&
        abs(time - last_flash_time) >= led_flash_duration)
    {
        led_flash_value = (led_flash_value == DEV_HIGH ? DEV_LOW : DEV_HIGH);

        _HAL_SET_VALUE(led_flash_id, led_flash_value);

        last_flash_time = time;
    }
}