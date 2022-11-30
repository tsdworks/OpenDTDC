using System;
using System.Collections.Generic;

namespace OpenDTDC.Interface
{
    public interface IHardware<TIOMode, TIOEnum, TValue>
    {
        // 连接设备
        bool Connect(string connectionString);

        // 断开连接
        bool Disconnect();

        // 判断是否连接
        bool IsConnected();

        // 获取型号
        int GetModel();

        // 获取版本
        int GetVersion();

        // 获取 IO 名称
        List<string> GetIONames();

        // 使用过滤器获取 IO 名称
        List<string> GetIONames(TIOMode typeFilter);

        // 获取 IO 值键值对列表
        List<Tuple<string, TValue>> GetValueTuples();

        // 获取值
        TValue GetValue(TIOEnum io);

        // 获取值
        TValue GetValue(string ioName);

        // 设置值
        bool SetValue(TIOEnum io, object value, bool stringValue);

        // 设置值
        bool SetValue(string ioName, object value, bool stringValue);
    }
}
