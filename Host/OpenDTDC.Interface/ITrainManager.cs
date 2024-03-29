﻿namespace OpenDTDC.Interface
{
    public interface ITrainManager
    {
        #region Setters
        // 设置功率手柄
        void SetTrainPowerHandle(int dataValue);

        // 设置列车制动
        void SetTrainBrakeHandle(int dataValue);

        // 设置再生（电阻）制动
        void SetDynamicBrakeHandle(int dataValue);

        // 设置机车制动
        void SetEngineBrakeHandle(int dataValue);

        // 设置手制动
        void SetHandBrakeHandle(int dataValue);

        // 设置紧急制动
        void SetEmergency(int dataValue);

        // 设置换向器
        void SetReverser(int dataValue);

        // 设置汽笛
        void SetHornState(int dataValue);

        // 设置头灯
        void SetHeadlightState(int dataValue);

        // 设置撒沙
        void SetSanderState(int dataValue);

        // 设置受电弓
        void SetPantoState(int dataValue);

        // 设置左车门
        void SetLeftDoorState(int dataValue);

        // 设置右车门
        void SetRightDoorState(int dataValue);
        #endregion

        #region Getters
        // 获取功率手柄状态
        int GetPowerHandle();

        // 获取列车制动状态
        int GetTrainBrakeHandle();

        // 获取再生（电阻）制动状态
        int GetDynamicBrakeHandle();

        // 获取机车制动状态
        int GetEngineBrakeHandle();

        // 获取手制动状态
        int GetHandBrakeState();

        // 获取紧急制动状态
        int GetEmergencyState();

        // 获取换向器状态
        int GetReverserState();

        // 获取汽笛状态
        int GetHornState();

        // 获取头灯状态
        int GetHeadlightState();

        // 获取撒沙状态
        int GetSanderState();

        // 获取受电弓状态
        int GetPantoState();

        // 获取左车门状态
        int GetLeftDoorState();

        // 获取右车门状态
        int GetRightDoorState();

        // 获取速度
        double GetSpeed();

        // 获取当前限速
        double GetSectionSpeedLimit();

        // 获取下一个信号
        int GetNextSectionSignal();

        // 获取下一个信号距离
        double GetNextSectionSignalDistance();

        // 获取当前坡度
        double GetRouteGradient();

        // 获取电压
        double GetVoltage();

        // 获取电流
        double GetCurrent();

        // 获取线路名
        string GetRouteName();

        // 获取列车名
        string GetTrainName();

        // 获取编组名
        string GetConsistName();

        // 获取任务名
        string GetActivityName();

        // 获取当前时间
        string GetTime();
        #endregion
    }
}
