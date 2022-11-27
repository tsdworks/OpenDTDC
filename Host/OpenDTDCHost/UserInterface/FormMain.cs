using System;
using System.Windows.Forms;

namespace OpenDTDCHost
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            ActionRefreshControllerCOMList();
            ActionRefreshHMICOMList();

            ActionRegisterEvents();
        }

        #region Controller
        private void buttonControllerCOMRefresh_Click(object sender, EventArgs e)
        {
            ActionRefreshControllerCOMList();
        }

        private void buttonControllerConnect_Click(object sender, EventArgs e)
        {
            toolStripStatusLabelMain.Text = "Connecting...";

            if (!ActionControllerConnect())
            {
                _ = MessageBox.Show("Fail to connect to the device.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            toolStripStatusLabelMain.Text = "Ready.";
        }

        private void buttonControllerDisconnect_Click(object sender, EventArgs e)
        {
            toolStripStatusLabelMain.Text = "Disconnecting...";

            if (!ActionControllerDisconnect())
            {
                _ = MessageBox.Show("Fail to disconnect the device.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            toolStripStatusLabelMain.Text = "Ready.";
        }

        private void buttonControllerWrite_Click(object sender, EventArgs e)
        {
            toolStripStatusLabelMain.Text = "Writting...";

            if (!ActionControllerWrite(comboBoxControllerDeviceIO.Text, int.Parse(textBoxControllerValueToWrite.Text)))
            {
                _ = MessageBox.Show("Fail to write to the device.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            toolStripStatusLabelMain.Text = "Ready.";
        }

        private void textBoxControllerValueToWrite_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b')
            {
                if ((e.KeyChar < '0') || (e.KeyChar > '9'))
                {
                    e.Handled = true;
                }
            }
        }
        #endregion

        #region HMI
        private void buttonHMICOMRefresh_Click(object sender, EventArgs e)
        {
            ActionRefreshHMICOMList();
        }

        private void buttonHMIConnect_Click(object sender, EventArgs e)
        {
            toolStripStatusLabelMain.Text = "Connecting...";

            if (!ActionHMIConnect())
            {
                _ = MessageBox.Show("Fail to connect to the device.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            toolStripStatusLabelMain.Text = "Ready.";
        }

        private void buttonHMIDisconnect_Click(object sender, EventArgs e)
        {
            toolStripStatusLabelMain.Text = "Disconnecting...";

            if (!ActionHMIDisconnect())
            {
                _ = MessageBox.Show("Fail to disconnect the device.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            toolStripStatusLabelMain.Text = "Ready.";
        }

        private void buttonHMIWrite_Click(object sender, EventArgs e)
        {
            toolStripStatusLabelMain.Text = "Writting...";

            if (!ActionHMIWrite(comboBoxHMIDeviceIO.Text, textBoxHMIValueToWrite.Text))
            {
                _ = MessageBox.Show("Fail to write to the device.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            toolStripStatusLabelMain.Text = "Ready.";
        }
        #endregion

        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            buttonControllerWrite.Enabled = Controller.IsConnected() && comboBoxControllerDeviceIO.Text != string.Empty && textBoxControllerValueToWrite.Text != string.Empty;
            buttonHMIWrite.Enabled = HMI.IsConnected() && comboBoxHMIDeviceIO.Text != string.Empty && textBoxHMIValueToWrite.Text != string.Empty;
        }
    }
}
