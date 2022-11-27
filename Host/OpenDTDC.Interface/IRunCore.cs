namespace OpenDTDC.Interface
{
    public interface IRunCore
    {
        // 初始化运行器
        bool Initialize();

        // 连接相关硬件设备
        bool ConnectDevices(object connectParams);

        // 断开相关硬件设备连接
        bool DisconnectDevices();
    }
}
