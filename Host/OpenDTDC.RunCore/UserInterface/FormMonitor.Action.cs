using System;

namespace OpenDTDC.RunCore.UserInterface
{
    public partial class FormMonitor
    {
        public delegate bool DisconnectDevicesHandler();
        public event DisconnectDevicesHandler DisconnectDevices;

        public delegate bool ConnectDevicesHandler(object connectParams);
        public event ConnectDevicesHandler ConnectDevices;

        public void ActionUpdateConnectionState(bool state)
        {
            _ = Invoke(new Action(() =>
            {
                if (state)
                {
                    buttonConnect.Enabled = false;
                    buttonDisconnect.Enabled = true;
                    labelDeviceInfo.Text = "Connected.";
                }
                else
                {
                    buttonConnect.Enabled = true;
                    buttonDisconnect.Enabled = false;
                    labelDeviceInfo.Text = "Not Connected.";
                }
            }));
        }
    }
}
