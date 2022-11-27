using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenDTDC.RunCore.UserInterface
{
    public partial class FormMonitor : Form
    {
        public FormMonitor()
        {
            InitializeComponent();
        }

        private void FormMonitor_Load(object sender, EventArgs e)
        {
            toolStripStatusLabelMain.Text = "Ready.";

            buttonConnect.PerformClick();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            toolStripStatusLabelMain.Text = "Connecting...";

            buttonConnect.Enabled = false;
            buttonDisconnect.Enabled = false;

            new Task(() =>
            {
                bool? result = ConnectDevices?.Invoke(null);

                if (result != null)
                {
                    ActionUpdateConnectionState((bool)result);

                    if (!(bool)result)
                    {
                        _ = Invoke(new Action(() =>
                        {
                            _ = MessageBox.Show("Fail to connect devices.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }));
                    }
                }

                _ = Invoke(new Action(() =>
                {
                    toolStripStatusLabelMain.Text = "Ready.";
                }));
            }).Start();
        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            toolStripStatusLabelMain.Text = "Disconnecting...";

            buttonConnect.Enabled = false;
            buttonDisconnect.Enabled = false;

            new Task(() =>
            {
                bool? result = DisconnectDevices?.Invoke();

                _ = Invoke(new Action(() =>
                {
                    toolStripStatusLabelMain.Text = "Ready.";
                }));
            }).Start();
        }
    }
}
