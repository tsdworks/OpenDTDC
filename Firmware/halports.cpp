#include "halports.h"

void hal_device_init()
{
    // 初始化设备
    for (DEV_ID i = 0; i < CTR_NUM; i++)
    {
        _HAL_SET_MODE(i, hal_device_mode_list[i]);
    }

    // 蜂鸣器测试
    _HAL_SET_VALUE(BP0, DEV_HIGH);

    delay(50);

    _HAL_SET_VALUE(BP0, DEV_LOW);
}