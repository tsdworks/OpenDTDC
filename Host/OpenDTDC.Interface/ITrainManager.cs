namespace OpenDTDC.Interface
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
        #endregion

        #region Getters
        // 获取功率手柄
        int GetPowerHandle();

        // 获取列车制动
        int GetTrainBrakeHandle();

        // 获取再生（电阻）制动
        int GetDynamicBrakeHandle();

        // 获取机车制动
        int GetEngineBrakeHandle();

        // 获取手制动
        int GetHandBrakeState();

        // 获取紧急制动
        int GetEmergencyState();

        // 获取换向器
        int GetReverserState();

        // 获取汽笛
        int GetHornState();

        // 获取头灯
        int GetHeadlightState();

        // 获取撒沙
        int GetSanderState();

        // 获取受电弓
        int GetPantoState();

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
