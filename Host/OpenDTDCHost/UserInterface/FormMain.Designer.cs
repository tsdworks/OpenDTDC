namespace OpenDTDCHost
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.buttonControllerConnect = new System.Windows.Forms.Button();
            this.buttonControllerDisconnect = new System.Windows.Forms.Button();
            this.comboBoxControllerCOM = new System.Windows.Forms.ComboBox();
            this.buttonControllerCOMRefresh = new System.Windows.Forms.Button();
            this.labelControllerDeviceInfo = new System.Windows.Forms.Label();
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelMain = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBoxControllerGetter = new System.Windows.Forms.GroupBox();
            this.listViewControllerData = new System.Windows.Forms.ListView();
            this.columnHeaderControllerName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderControllerValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBoxControllerSetter = new System.Windows.Forms.GroupBox();
            this.buttonControllerWrite = new System.Windows.Forms.Button();
            this.textBoxControllerValueToWrite = new System.Windows.Forms.TextBox();
            this.labelControllerValueToWrite = new System.Windows.Forms.Label();
            this.labelControllerDeviceIO = new System.Windows.Forms.Label();
            this.comboBoxControllerDeviceIO = new System.Windows.Forms.ComboBox();
            this.timerUpdate = new System.Windows.Forms.Timer(this.components);
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageController = new System.Windows.Forms.TabPage();
            this.tabPageHMI = new System.Windows.Forms.TabPage();
            this.labelHMIDeviceInfo = new System.Windows.Forms.Label();
            this.groupBoxHMISetter = new System.Windows.Forms.GroupBox();
            this.buttonHMIWrite = new System.Windows.Forms.Button();
            this.textBoxHMIValueToWrite = new System.Windows.Forms.TextBox();
            this.labelHMIValueToWrite = new System.Windows.Forms.Label();
            this.labelHMIDeviceIO = new System.Windows.Forms.Label();
            this.comboBoxHMIDeviceIO = new System.Windows.Forms.ComboBox();
            this.buttonHMIConnect = new System.Windows.Forms.Button();
            this.groupBoxHMIGetter = new System.Windows.Forms.GroupBox();
            this.listViewHMIData = new System.Windows.Forms.ListView();
            this.columnHeaderHMIName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderHMIValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonHMIDisconnect = new System.Windows.Forms.Button();
            this.comboBoxHMICOM = new System.Windows.Forms.ComboBox();
            this.buttonHMICOMRefresh = new System.Windows.Forms.Button();
            this.statusStripMain.SuspendLayout();
            this.groupBoxControllerGetter.SuspendLayout();
            this.groupBoxControllerSetter.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPageController.SuspendLayout();
            this.tabPageHMI.SuspendLayout();
            this.groupBoxHMISetter.SuspendLayout();
            this.groupBoxHMIGetter.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonControllerConnect
            // 
            this.buttonControllerConnect.Location = new System.Drawing.Point(230, 10);
            this.buttonControllerConnect.Name = "buttonControllerConnect";
            this.buttonControllerConnect.Size = new System.Drawing.Size(86, 25);
            this.buttonControllerConnect.TabIndex = 0;
            this.buttonControllerConnect.Text = "Connect";
            this.buttonControllerConnect.UseVisualStyleBackColor = true;
            this.buttonControllerConnect.Click += new System.EventHandler(this.buttonControllerConnect_Click);
            // 
            // buttonControllerDisconnect
            // 
            this.buttonControllerDisconnect.Enabled = false;
            this.buttonControllerDisconnect.Location = new System.Drawing.Point(322, 10);
            this.buttonControllerDisconnect.Name = "buttonControllerDisconnect";
            this.buttonControllerDisconnect.Size = new System.Drawing.Size(86, 25);
            this.buttonControllerDisconnect.TabIndex = 1;
            this.buttonControllerDisconnect.Text = "Disconnect";
            this.buttonControllerDisconnect.UseVisualStyleBackColor = true;
            this.buttonControllerDisconnect.Click += new System.EventHandler(this.buttonControllerDisconnect_Click);
            // 
            // comboBoxControllerCOM
            // 
            this.comboBoxControllerCOM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxControllerCOM.FormattingEnabled = true;
            this.comboBoxControllerCOM.Location = new System.Drawing.Point(11, 11);
            this.comboBoxControllerCOM.Name = "comboBoxControllerCOM";
            this.comboBoxControllerCOM.Size = new System.Drawing.Size(121, 25);
            this.comboBoxControllerCOM.TabIndex = 2;
            // 
            // buttonControllerCOMRefresh
            // 
            this.buttonControllerCOMRefresh.Location = new System.Drawing.Point(138, 10);
            this.buttonControllerCOMRefresh.Name = "buttonControllerCOMRefresh";
            this.buttonControllerCOMRefresh.Size = new System.Drawing.Size(86, 25);
            this.buttonControllerCOMRefresh.TabIndex = 3;
            this.buttonControllerCOMRefresh.Text = "Refresh";
            this.buttonControllerCOMRefresh.UseVisualStyleBackColor = true;
            this.buttonControllerCOMRefresh.Click += new System.EventHandler(this.buttonControllerCOMRefresh_Click);
            // 
            // labelControllerDeviceInfo
            // 
            this.labelControllerDeviceInfo.Location = new System.Drawing.Point(417, 12);
            this.labelControllerDeviceInfo.Name = "labelControllerDeviceInfo";
            this.labelControllerDeviceInfo.Size = new System.Drawing.Size(397, 24);
            this.labelControllerDeviceInfo.TabIndex = 4;
            this.labelControllerDeviceInfo.Text = "Not Connected.";
            this.labelControllerDeviceInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // statusStripMain
            // 
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelMain});
            this.statusStripMain.Location = new System.Drawing.Point(0, 542);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Size = new System.Drawing.Size(830, 22);
            this.statusStripMain.TabIndex = 5;
            this.statusStripMain.Text = "statusStrip1";
            // 
            // toolStripStatusLabelMain
            // 
            this.toolStripStatusLabelMain.Name = "toolStripStatusLabelMain";
            this.toolStripStatusLabelMain.Size = new System.Drawing.Size(47, 17);
            this.toolStripStatusLabelMain.Text = "Ready.";
            // 
            // groupBoxControllerGetter
            // 
            this.groupBoxControllerGetter.Controls.Add(this.listViewControllerData);
            this.groupBoxControllerGetter.Location = new System.Drawing.Point(11, 42);
            this.groupBoxControllerGetter.Name = "groupBoxControllerGetter";
            this.groupBoxControllerGetter.Size = new System.Drawing.Size(397, 465);
            this.groupBoxControllerGetter.TabIndex = 6;
            this.groupBoxControllerGetter.TabStop = false;
            this.groupBoxControllerGetter.Text = "Getter";
            // 
            // listViewControllerData
            // 
            this.listViewControllerData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderControllerName,
            this.columnHeaderControllerValue});
            this.listViewControllerData.GridLines = true;
            this.listViewControllerData.HideSelection = false;
            this.listViewControllerData.Location = new System.Drawing.Point(6, 22);
            this.listViewControllerData.Name = "listViewControllerData";
            this.listViewControllerData.Size = new System.Drawing.Size(385, 437);
            this.listViewControllerData.TabIndex = 0;
            this.listViewControllerData.UseCompatibleStateImageBehavior = false;
            this.listViewControllerData.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderControllerName
            // 
            this.columnHeaderControllerName.Text = "Name";
            this.columnHeaderControllerName.Width = 120;
            // 
            // columnHeaderControllerValue
            // 
            this.columnHeaderControllerValue.Text = "Value";
            this.columnHeaderControllerValue.Width = 240;
            // 
            // groupBoxControllerSetter
            // 
            this.groupBoxControllerSetter.Controls.Add(this.buttonControllerWrite);
            this.groupBoxControllerSetter.Controls.Add(this.textBoxControllerValueToWrite);
            this.groupBoxControllerSetter.Controls.Add(this.labelControllerValueToWrite);
            this.groupBoxControllerSetter.Controls.Add(this.labelControllerDeviceIO);
            this.groupBoxControllerSetter.Controls.Add(this.comboBoxControllerDeviceIO);
            this.groupBoxControllerSetter.Location = new System.Drawing.Point(417, 42);
            this.groupBoxControllerSetter.Name = "groupBoxControllerSetter";
            this.groupBoxControllerSetter.Size = new System.Drawing.Size(397, 465);
            this.groupBoxControllerSetter.TabIndex = 7;
            this.groupBoxControllerSetter.TabStop = false;
            this.groupBoxControllerSetter.Text = "Setter";
            // 
            // buttonControllerWrite
            // 
            this.buttonControllerWrite.Location = new System.Drawing.Point(305, 133);
            this.buttonControllerWrite.Name = "buttonControllerWrite";
            this.buttonControllerWrite.Size = new System.Drawing.Size(86, 25);
            this.buttonControllerWrite.TabIndex = 8;
            this.buttonControllerWrite.Text = "Write";
            this.buttonControllerWrite.UseVisualStyleBackColor = true;
            this.buttonControllerWrite.Click += new System.EventHandler(this.buttonControllerWrite_Click);
            // 
            // textBoxControllerValueToWrite
            // 
            this.textBoxControllerValueToWrite.Location = new System.Drawing.Point(6, 104);
            this.textBoxControllerValueToWrite.MaxLength = 3;
            this.textBoxControllerValueToWrite.Name = "textBoxControllerValueToWrite";
            this.textBoxControllerValueToWrite.Size = new System.Drawing.Size(385, 23);
            this.textBoxControllerValueToWrite.TabIndex = 7;
            this.textBoxControllerValueToWrite.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxControllerValueToWrite_KeyPress);
            // 
            // labelControllerValueToWrite
            // 
            this.labelControllerValueToWrite.Location = new System.Drawing.Point(6, 77);
            this.labelControllerValueToWrite.Name = "labelControllerValueToWrite";
            this.labelControllerValueToWrite.Size = new System.Drawing.Size(385, 24);
            this.labelControllerValueToWrite.TabIndex = 6;
            this.labelControllerValueToWrite.Text = "Value To Write:";
            this.labelControllerValueToWrite.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelControllerDeviceIO
            // 
            this.labelControllerDeviceIO.Location = new System.Drawing.Point(6, 22);
            this.labelControllerDeviceIO.Name = "labelControllerDeviceIO";
            this.labelControllerDeviceIO.Size = new System.Drawing.Size(385, 24);
            this.labelControllerDeviceIO.TabIndex = 5;
            this.labelControllerDeviceIO.Text = "Device IO:";
            this.labelControllerDeviceIO.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxControllerDeviceIO
            // 
            this.comboBoxControllerDeviceIO.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxControllerDeviceIO.FormattingEnabled = true;
            this.comboBoxControllerDeviceIO.Location = new System.Drawing.Point(6, 49);
            this.comboBoxControllerDeviceIO.Name = "comboBoxControllerDeviceIO";
            this.comboBoxControllerDeviceIO.Size = new System.Drawing.Size(385, 25);
            this.comboBoxControllerDeviceIO.TabIndex = 3;
            // 
            // timerUpdate
            // 
            this.timerUpdate.Enabled = true;
            this.timerUpdate.Tick += new System.EventHandler(this.timerUpdate_Tick);
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageController);
            this.tabControlMain.Controls.Add(this.tabPageHMI);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(830, 542);
            this.tabControlMain.TabIndex = 8;
            // 
            // tabPageController
            // 
            this.tabPageController.Controls.Add(this.labelControllerDeviceInfo);
            this.tabPageController.Controls.Add(this.groupBoxControllerSetter);
            this.tabPageController.Controls.Add(this.buttonControllerConnect);
            this.tabPageController.Controls.Add(this.groupBoxControllerGetter);
            this.tabPageController.Controls.Add(this.buttonControllerDisconnect);
            this.tabPageController.Controls.Add(this.comboBoxControllerCOM);
            this.tabPageController.Controls.Add(this.buttonControllerCOMRefresh);
            this.tabPageController.Location = new System.Drawing.Point(4, 26);
            this.tabPageController.Name = "tabPageController";
            this.tabPageController.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageController.Size = new System.Drawing.Size(822, 512);
            this.tabPageController.TabIndex = 0;
            this.tabPageController.Text = "Controller";
            this.tabPageController.UseVisualStyleBackColor = true;
            // 
            // tabPageHMI
            // 
            this.tabPageHMI.Controls.Add(this.labelHMIDeviceInfo);
            this.tabPageHMI.Controls.Add(this.groupBoxHMISetter);
            this.tabPageHMI.Controls.Add(this.buttonHMIConnect);
            this.tabPageHMI.Controls.Add(this.groupBoxHMIGetter);
            this.tabPageHMI.Controls.Add(this.buttonHMIDisconnect);
            this.tabPageHMI.Controls.Add(this.comboBoxHMICOM);
            this.tabPageHMI.Controls.Add(this.buttonHMICOMRefresh);
            this.tabPageHMI.Location = new System.Drawing.Point(4, 26);
            this.tabPageHMI.Name = "tabPageHMI";
            this.tabPageHMI.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageHMI.Size = new System.Drawing.Size(822, 512);
            this.tabPageHMI.TabIndex = 1;
            this.tabPageHMI.Text = "HMI";
            this.tabPageHMI.UseVisualStyleBackColor = true;
            // 
            // labelHMIDeviceInfo
            // 
            this.labelHMIDeviceInfo.Location = new System.Drawing.Point(417, 12);
            this.labelHMIDeviceInfo.Name = "labelHMIDeviceInfo";
            this.labelHMIDeviceInfo.Size = new System.Drawing.Size(397, 24);
            this.labelHMIDeviceInfo.TabIndex = 12;
            this.labelHMIDeviceInfo.Text = "Not Connected.";
            this.labelHMIDeviceInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBoxHMISetter
            // 
            this.groupBoxHMISetter.Controls.Add(this.buttonHMIWrite);
            this.groupBoxHMISetter.Controls.Add(this.textBoxHMIValueToWrite);
            this.groupBoxHMISetter.Controls.Add(this.labelHMIValueToWrite);
            this.groupBoxHMISetter.Controls.Add(this.labelHMIDeviceIO);
            this.groupBoxHMISetter.Controls.Add(this.comboBoxHMIDeviceIO);
            this.groupBoxHMISetter.Location = new System.Drawing.Point(417, 42);
            this.groupBoxHMISetter.Name = "groupBoxHMISetter";
            this.groupBoxHMISetter.Size = new System.Drawing.Size(397, 465);
            this.groupBoxHMISetter.TabIndex = 14;
            this.groupBoxHMISetter.TabStop = false;
            this.groupBoxHMISetter.Text = "Setter";
            // 
            // buttonHMIWrite
            // 
            this.buttonHMIWrite.Location = new System.Drawing.Point(305, 133);
            this.buttonHMIWrite.Name = "buttonHMIWrite";
            this.buttonHMIWrite.Size = new System.Drawing.Size(86, 25);
            this.buttonHMIWrite.TabIndex = 8;
            this.buttonHMIWrite.Text = "Write";
            this.buttonHMIWrite.UseVisualStyleBackColor = true;
            this.buttonHMIWrite.Click += new System.EventHandler(this.buttonHMIWrite_Click);
            // 
            // textBoxHMIValueToWrite
            // 
            this.textBoxHMIValueToWrite.Location = new System.Drawing.Point(6, 104);
            this.textBoxHMIValueToWrite.MaxLength = 3;
            this.textBoxHMIValueToWrite.Name = "textBoxHMIValueToWrite";
            this.textBoxHMIValueToWrite.Size = new System.Drawing.Size(385, 23);
            this.textBoxHMIValueToWrite.TabIndex = 20;
            // 
            // labelHMIValueToWrite
            // 
            this.labelHMIValueToWrite.Location = new System.Drawing.Point(6, 77);
            this.labelHMIValueToWrite.Name = "labelHMIValueToWrite";
            this.labelHMIValueToWrite.Size = new System.Drawing.Size(385, 24);
            this.labelHMIValueToWrite.TabIndex = 6;
            this.labelHMIValueToWrite.Text = "Value To Write:";
            this.labelHMIValueToWrite.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelHMIDeviceIO
            // 
            this.labelHMIDeviceIO.Location = new System.Drawing.Point(6, 22);
            this.labelHMIDeviceIO.Name = "labelHMIDeviceIO";
            this.labelHMIDeviceIO.Size = new System.Drawing.Size(385, 24);
            this.labelHMIDeviceIO.TabIndex = 5;
            this.labelHMIDeviceIO.Text = "Device IO:";
            this.labelHMIDeviceIO.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxHMIDeviceIO
            // 
            this.comboBoxHMIDeviceIO.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxHMIDeviceIO.FormattingEnabled = true;
            this.comboBoxHMIDeviceIO.Location = new System.Drawing.Point(6, 49);
            this.comboBoxHMIDeviceIO.Name = "comboBoxHMIDeviceIO";
            this.comboBoxHMIDeviceIO.Size = new System.Drawing.Size(385, 25);
            this.comboBoxHMIDeviceIO.TabIndex = 3;
            // 
            // buttonHMIConnect
            // 
            this.buttonHMIConnect.Location = new System.Drawing.Point(230, 10);
            this.buttonHMIConnect.Name = "buttonHMIConnect";
            this.buttonHMIConnect.Size = new System.Drawing.Size(86, 25);
            this.buttonHMIConnect.TabIndex = 8;
            this.buttonHMIConnect.Text = "Connect";
            this.buttonHMIConnect.UseVisualStyleBackColor = true;
            this.buttonHMIConnect.Click += new System.EventHandler(this.buttonHMIConnect_Click);
            // 
            // groupBoxHMIGetter
            // 
            this.groupBoxHMIGetter.Controls.Add(this.listViewHMIData);
            this.groupBoxHMIGetter.Location = new System.Drawing.Point(11, 42);
            this.groupBoxHMIGetter.Name = "groupBoxHMIGetter";
            this.groupBoxHMIGetter.Size = new System.Drawing.Size(397, 465);
            this.groupBoxHMIGetter.TabIndex = 13;
            this.groupBoxHMIGetter.TabStop = false;
            this.groupBoxHMIGetter.Text = "Getter";
            // 
            // listViewHMIData
            // 
            this.listViewHMIData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderHMIName,
            this.columnHeaderHMIValue});
            this.listViewHMIData.GridLines = true;
            this.listViewHMIData.HideSelection = false;
            this.listViewHMIData.Location = new System.Drawing.Point(6, 22);
            this.listViewHMIData.Name = "listViewHMIData";
            this.listViewHMIData.Size = new System.Drawing.Size(385, 437);
            this.listViewHMIData.TabIndex = 0;
            this.listViewHMIData.UseCompatibleStateImageBehavior = false;
            this.listViewHMIData.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderHMIName
            // 
            this.columnHeaderHMIName.Text = "Name";
            this.columnHeaderHMIName.Width = 120;
            // 
            // columnHeaderHMIValue
            // 
            this.columnHeaderHMIValue.Text = "Value";
            this.columnHeaderHMIValue.Width = 240;
            // 
            // buttonHMIDisconnect
            // 
            this.buttonHMIDisconnect.Enabled = false;
            this.buttonHMIDisconnect.Location = new System.Drawing.Point(322, 10);
            this.buttonHMIDisconnect.Name = "buttonHMIDisconnect";
            this.buttonHMIDisconnect.Size = new System.Drawing.Size(86, 25);
            this.buttonHMIDisconnect.TabIndex = 9;
            this.buttonHMIDisconnect.Text = "Disconnect";
            this.buttonHMIDisconnect.UseVisualStyleBackColor = true;
            this.buttonHMIDisconnect.Click += new System.EventHandler(this.buttonHMIDisconnect_Click);
            // 
            // comboBoxHMICOM
            // 
            this.comboBoxHMICOM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxHMICOM.FormattingEnabled = true;
            this.comboBoxHMICOM.Location = new System.Drawing.Point(11, 11);
            this.comboBoxHMICOM.Name = "comboBoxHMICOM";
            this.comboBoxHMICOM.Size = new System.Drawing.Size(121, 25);
            this.comboBoxHMICOM.TabIndex = 10;
            // 
            // buttonHMICOMRefresh
            // 
            this.buttonHMICOMRefresh.Location = new System.Drawing.Point(138, 10);
            this.buttonHMICOMRefresh.Name = "buttonHMICOMRefresh";
            this.buttonHMICOMRefresh.Size = new System.Drawing.Size(86, 25);
            this.buttonHMICOMRefresh.TabIndex = 11;
            this.buttonHMICOMRefresh.Text = "Refresh";
            this.buttonHMICOMRefresh.UseVisualStyleBackColor = true;
            this.buttonHMICOMRefresh.Click += new System.EventHandler(this.buttonHMICOMRefresh_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 564);
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.statusStripMain);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OpenDTDC Tweak Tool";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            this.groupBoxControllerGetter.ResumeLayout(false);
            this.groupBoxControllerSetter.ResumeLayout(false);
            this.groupBoxControllerSetter.PerformLayout();
            this.tabControlMain.ResumeLayout(false);
            this.tabPageController.ResumeLayout(false);
            this.tabPageHMI.ResumeLayout(false);
            this.groupBoxHMISetter.ResumeLayout(false);
            this.groupBoxHMISetter.PerformLayout();
            this.groupBoxHMIGetter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonControllerConnect;
        private System.Windows.Forms.Button buttonControllerDisconnect;
        private System.Windows.Forms.ComboBox comboBoxControllerCOM;
        private System.Windows.Forms.Button buttonControllerCOMRefresh;
        private System.Windows.Forms.Label labelControllerDeviceInfo;
        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelMain;
        private System.Windows.Forms.GroupBox groupBoxControllerGetter;
        private System.Windows.Forms.GroupBox groupBoxControllerSetter;
        private System.Windows.Forms.ListView listViewControllerData;
        private System.Windows.Forms.ColumnHeader columnHeaderControllerName;
        private System.Windows.Forms.ColumnHeader columnHeaderControllerValue;
        private System.Windows.Forms.ComboBox comboBoxControllerDeviceIO;
        private System.Windows.Forms.Label labelControllerDeviceIO;
        private System.Windows.Forms.Label labelControllerValueToWrite;
        private System.Windows.Forms.TextBox textBoxControllerValueToWrite;
        private System.Windows.Forms.Button buttonControllerWrite;
        private System.Windows.Forms.Timer timerUpdate;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageController;
        private System.Windows.Forms.TabPage tabPageHMI;
        private System.Windows.Forms.Label labelHMIDeviceInfo;
        private System.Windows.Forms.GroupBox groupBoxHMISetter;
        private System.Windows.Forms.Button buttonHMIWrite;
        private System.Windows.Forms.TextBox textBoxHMIValueToWrite;
        private System.Windows.Forms.Label labelHMIValueToWrite;
        private System.Windows.Forms.Label labelHMIDeviceIO;
        private System.Windows.Forms.ComboBox comboBoxHMIDeviceIO;
        private System.Windows.Forms.Button buttonHMIConnect;
        private System.Windows.Forms.GroupBox groupBoxHMIGetter;
        private System.Windows.Forms.ListView listViewHMIData;
        private System.Windows.Forms.ColumnHeader columnHeaderHMIName;
        private System.Windows.Forms.ColumnHeader columnHeaderHMIValue;
        private System.Windows.Forms.Button buttonHMIDisconnect;
        private System.Windows.Forms.ComboBox comboBoxHMICOM;
        private System.Windows.Forms.Button buttonHMICOMRefresh;
    }
}

