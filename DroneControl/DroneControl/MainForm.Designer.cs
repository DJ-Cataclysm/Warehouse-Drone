namespace DroneControl
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pbVideo = new System.Windows.Forms.PictureBox();
            this.gbVideoFeed = new System.Windows.Forms.GroupBox();
            this.gbTelemetry = new System.Windows.Forms.GroupBox();
            this.lblDesc0 = new System.Windows.Forms.Label();
            this.lblConnectionStatus = new System.Windows.Forms.Label();
            this.lblWifi = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblBattery = new System.Windows.Forms.Label();
            this.lblVelocityX = new System.Windows.Forms.Label();
            this.lblVelocityZ = new System.Windows.Forms.Label();
            this.lblVelocityY = new System.Windows.Forms.Label();
            this.lblAltitude = new System.Windows.Forms.Label();
            this.lblRoll = new System.Windows.Forms.Label();
            this.lblPitch = new System.Windows.Forms.Label();
            this.lblYaw = new System.Windows.Forms.Label();
            this.lblNavigationState = new System.Windows.Forms.Label();
            this.lblDesc9 = new System.Windows.Forms.Label();
            this.lblDesc8 = new System.Windows.Forms.Label();
            this.lblDesc7 = new System.Windows.Forms.Label();
            this.lblDesc6 = new System.Windows.Forms.Label();
            this.lblDesc5 = new System.Windows.Forms.Label();
            this.lblDesc4 = new System.Windows.Forms.Label();
            this.lblDesc3 = new System.Windows.Forms.Label();
            this.lblDesc2 = new System.Windows.Forms.Label();
            this.lblDesc1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.tmrStateUpdate = new System.Windows.Forms.Timer(this.components);
            this.tmrVideoUpdate = new System.Windows.Forms.Timer(this.components);
            this.gbBarcodeScanning = new System.Windows.Forms.GroupBox();
            this.barcode = new System.Windows.Forms.Label();
            this.btnMockDoneScanning = new System.Windows.Forms.Button();
            this.btnMockScan = new System.Windows.Forms.Button();
            this.lblBarcode = new System.Windows.Forms.Label();
            this.nudMockScan = new System.Windows.Forms.NumericUpDown();
            this.btnScanForBarcode = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSmartScan = new System.Windows.Forms.Button();
            this.btnCycleCount = new System.Windows.Forms.Button();
            this.btnAutopilotStop = new System.Windows.Forms.Button();
            this.btnAutopilotGo = new System.Windows.Forms.Button();
            this.btnEmergency = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnBackward = new System.Windows.Forms.Button();
            this.btnRotateLeft = new System.Windows.Forms.Button();
            this.btnRotateRight = new System.Windows.Forms.Button();
            this.btnRight = new System.Windows.Forms.Button();
            this.btnLeft = new System.Windows.Forms.Button();
            this.btnHover = new System.Windows.Forms.Button();
            this.btnForward = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnFlatTrim = new System.Windows.Forms.Button();
            this.btnTakeoff = new System.Windows.Forms.Button();
            this.btnLand = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbVideo)).BeginInit();
            this.gbVideoFeed.SuspendLayout();
            this.gbTelemetry.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gbBarcodeScanning.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMockScan)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbVideo
            // 
            this.pbVideo.BackColor = System.Drawing.SystemColors.WindowText;
            this.pbVideo.Location = new System.Drawing.Point(12, 18);
            this.pbVideo.Name = "pbVideo";
            this.pbVideo.Size = new System.Drawing.Size(640, 360);
            this.pbVideo.TabIndex = 0;
            this.pbVideo.TabStop = false;
            // 
            // gbVideoFeed
            // 
            this.gbVideoFeed.Controls.Add(this.pbVideo);
            this.gbVideoFeed.Location = new System.Drawing.Point(6, 6);
            this.gbVideoFeed.Name = "gbVideoFeed";
            this.gbVideoFeed.Size = new System.Drawing.Size(666, 400);
            this.gbVideoFeed.TabIndex = 1;
            this.gbVideoFeed.TabStop = false;
            this.gbVideoFeed.Text = "Video Feed";
            this.gbVideoFeed.Enter += new System.EventHandler(this.gbVideoFeed_Enter);
            // 
            // gbTelemetry
            // 
            this.gbTelemetry.Controls.Add(this.lblDesc0);
            this.gbTelemetry.Controls.Add(this.lblConnectionStatus);
            this.gbTelemetry.Controls.Add(this.lblWifi);
            this.gbTelemetry.Controls.Add(this.lblTime);
            this.gbTelemetry.Controls.Add(this.lblBattery);
            this.gbTelemetry.Controls.Add(this.lblVelocityX);
            this.gbTelemetry.Controls.Add(this.lblVelocityZ);
            this.gbTelemetry.Controls.Add(this.lblVelocityY);
            this.gbTelemetry.Controls.Add(this.lblAltitude);
            this.gbTelemetry.Controls.Add(this.lblRoll);
            this.gbTelemetry.Controls.Add(this.lblPitch);
            this.gbTelemetry.Controls.Add(this.lblYaw);
            this.gbTelemetry.Controls.Add(this.lblNavigationState);
            this.gbTelemetry.Controls.Add(this.lblDesc9);
            this.gbTelemetry.Controls.Add(this.lblDesc8);
            this.gbTelemetry.Controls.Add(this.lblDesc7);
            this.gbTelemetry.Controls.Add(this.lblDesc6);
            this.gbTelemetry.Controls.Add(this.lblDesc5);
            this.gbTelemetry.Controls.Add(this.lblDesc4);
            this.gbTelemetry.Controls.Add(this.lblDesc3);
            this.gbTelemetry.Controls.Add(this.lblDesc2);
            this.gbTelemetry.Controls.Add(this.lblDesc1);
            this.gbTelemetry.Location = new System.Drawing.Point(678, 6);
            this.gbTelemetry.Name = "gbTelemetry";
            this.gbTelemetry.Size = new System.Drawing.Size(287, 302);
            this.gbTelemetry.TabIndex = 2;
            this.gbTelemetry.TabStop = false;
            this.gbTelemetry.Text = "Telemetry";
            // 
            // lblDesc0
            // 
            this.lblDesc0.AutoSize = true;
            this.lblDesc0.Location = new System.Drawing.Point(6, 18);
            this.lblDesc0.Name = "lblDesc0";
            this.lblDesc0.Size = new System.Drawing.Size(95, 13);
            this.lblDesc0.TabIndex = 48;
            this.lblDesc0.Text = "Connection status:";
            // 
            // lblConnectionStatus
            // 
            this.lblConnectionStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblConnectionStatus.Location = new System.Drawing.Point(107, 13);
            this.lblConnectionStatus.Name = "lblConnectionStatus";
            this.lblConnectionStatus.Size = new System.Drawing.Size(171, 23);
            this.lblConnectionStatus.TabIndex = 47;
            this.lblConnectionStatus.Text = "Disconnected";
            this.lblConnectionStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblWifi
            // 
            this.lblWifi.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblWifi.Location = new System.Drawing.Point(107, 267);
            this.lblWifi.Name = "lblWifi";
            this.lblWifi.Size = new System.Drawing.Size(171, 23);
            this.lblWifi.TabIndex = 46;
            this.lblWifi.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTime
            // 
            this.lblTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblTime.Location = new System.Drawing.Point(107, 244);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(171, 23);
            this.lblTime.TabIndex = 45;
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBattery
            // 
            this.lblBattery.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblBattery.Location = new System.Drawing.Point(107, 221);
            this.lblBattery.Name = "lblBattery";
            this.lblBattery.Size = new System.Drawing.Size(171, 23);
            this.lblBattery.TabIndex = 44;
            this.lblBattery.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblVelocityX
            // 
            this.lblVelocityX.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblVelocityX.Location = new System.Drawing.Point(107, 198);
            this.lblVelocityX.Name = "lblVelocityX";
            this.lblVelocityX.Size = new System.Drawing.Size(171, 23);
            this.lblVelocityX.TabIndex = 43;
            this.lblVelocityX.Text = "X:";
            this.lblVelocityX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblVelocityZ
            // 
            this.lblVelocityZ.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblVelocityZ.Location = new System.Drawing.Point(107, 152);
            this.lblVelocityZ.Name = "lblVelocityZ";
            this.lblVelocityZ.Size = new System.Drawing.Size(171, 23);
            this.lblVelocityZ.TabIndex = 42;
            this.lblVelocityZ.Text = "Z:";
            this.lblVelocityZ.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblVelocityY
            // 
            this.lblVelocityY.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblVelocityY.Location = new System.Drawing.Point(107, 175);
            this.lblVelocityY.Name = "lblVelocityY";
            this.lblVelocityY.Size = new System.Drawing.Size(171, 23);
            this.lblVelocityY.TabIndex = 41;
            this.lblVelocityY.Text = "Y:";
            this.lblVelocityY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAltitude
            // 
            this.lblAltitude.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblAltitude.Location = new System.Drawing.Point(107, 128);
            this.lblAltitude.Name = "lblAltitude";
            this.lblAltitude.Size = new System.Drawing.Size(171, 23);
            this.lblAltitude.TabIndex = 40;
            this.lblAltitude.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRoll
            // 
            this.lblRoll.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblRoll.Location = new System.Drawing.Point(107, 105);
            this.lblRoll.Name = "lblRoll";
            this.lblRoll.Size = new System.Drawing.Size(171, 23);
            this.lblRoll.TabIndex = 39;
            this.lblRoll.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPitch
            // 
            this.lblPitch.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblPitch.Location = new System.Drawing.Point(107, 82);
            this.lblPitch.Name = "lblPitch";
            this.lblPitch.Size = new System.Drawing.Size(171, 23);
            this.lblPitch.TabIndex = 38;
            this.lblPitch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblYaw
            // 
            this.lblYaw.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblYaw.Location = new System.Drawing.Point(107, 59);
            this.lblYaw.Name = "lblYaw";
            this.lblYaw.Size = new System.Drawing.Size(171, 23);
            this.lblYaw.TabIndex = 37;
            this.lblYaw.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblNavigationState
            // 
            this.lblNavigationState.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblNavigationState.Location = new System.Drawing.Point(107, 36);
            this.lblNavigationState.Name = "lblNavigationState";
            this.lblNavigationState.Size = new System.Drawing.Size(171, 23);
            this.lblNavigationState.TabIndex = 36;
            this.lblNavigationState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDesc9
            // 
            this.lblDesc9.AutoSize = true;
            this.lblDesc9.Location = new System.Drawing.Point(21, 272);
            this.lblDesc9.Name = "lblDesc9";
            this.lblDesc9.Size = new System.Drawing.Size(80, 13);
            this.lblDesc9.TabIndex = 35;
            this.lblDesc9.Text = "Wifi link quality:";
            // 
            // lblDesc8
            // 
            this.lblDesc8.AutoSize = true;
            this.lblDesc8.Location = new System.Drawing.Point(42, 249);
            this.lblDesc8.Name = "lblDesc8";
            this.lblDesc8.Size = new System.Drawing.Size(59, 13);
            this.lblDesc8.TabIndex = 34;
            this.lblDesc8.Text = "Time (sec):";
            // 
            // lblDesc7
            // 
            this.lblDesc7.AutoSize = true;
            this.lblDesc7.Location = new System.Drawing.Point(41, 226);
            this.lblDesc7.Name = "lblDesc7";
            this.lblDesc7.Size = new System.Drawing.Size(60, 13);
            this.lblDesc7.TabIndex = 33;
            this.lblDesc7.Text = "Battery (%):";
            // 
            // lblDesc6
            // 
            this.lblDesc6.AutoSize = true;
            this.lblDesc6.Location = new System.Drawing.Point(33, 157);
            this.lblDesc6.Name = "lblDesc6";
            this.lblDesc6.Size = new System.Drawing.Size(68, 13);
            this.lblDesc6.TabIndex = 32;
            this.lblDesc6.Text = "Velocity (v3):";
            // 
            // lblDesc5
            // 
            this.lblDesc5.AutoSize = true;
            this.lblDesc5.Location = new System.Drawing.Point(39, 133);
            this.lblDesc5.Name = "lblDesc5";
            this.lblDesc5.Size = new System.Drawing.Size(62, 13);
            this.lblDesc5.TabIndex = 31;
            this.lblDesc5.Text = "Altitude (m):";
            // 
            // lblDesc4
            // 
            this.lblDesc4.AutoSize = true;
            this.lblDesc4.Location = new System.Drawing.Point(49, 110);
            this.lblDesc4.Name = "lblDesc4";
            this.lblDesc4.Size = new System.Drawing.Size(52, 13);
            this.lblDesc4.TabIndex = 30;
            this.lblDesc4.Text = "Roll (rad):";
            // 
            // lblDesc3
            // 
            this.lblDesc3.AutoSize = true;
            this.lblDesc3.Location = new System.Drawing.Point(43, 87);
            this.lblDesc3.Name = "lblDesc3";
            this.lblDesc3.Size = new System.Drawing.Size(58, 13);
            this.lblDesc3.TabIndex = 29;
            this.lblDesc3.Text = "Pitch (rad):";
            // 
            // lblDesc2
            // 
            this.lblDesc2.AutoSize = true;
            this.lblDesc2.Location = new System.Drawing.Point(46, 64);
            this.lblDesc2.Name = "lblDesc2";
            this.lblDesc2.Size = new System.Drawing.Size(55, 13);
            this.lblDesc2.TabIndex = 28;
            this.lblDesc2.Text = "Yaw (rad):";
            // 
            // lblDesc1
            // 
            this.lblDesc1.AutoSize = true;
            this.lblDesc1.Location = new System.Drawing.Point(6, 41);
            this.lblDesc1.Name = "lblDesc1";
            this.lblDesc1.Size = new System.Drawing.Size(95, 13);
            this.lblDesc1.TabIndex = 27;
            this.lblDesc1.Text = "Navigational state:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnDisconnect);
            this.groupBox1.Controls.Add(this.btnConnect);
            this.groupBox1.Location = new System.Drawing.Point(678, 322);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(287, 84);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Drone control";
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Enabled = false;
            this.btnDisconnect.Location = new System.Drawing.Point(9, 48);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(269, 23);
            this.btnDisconnect.TabIndex = 1;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(9, 19);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(269, 23);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // tmrStateUpdate
            // 
            this.tmrStateUpdate.Interval = 500;
            this.tmrStateUpdate.Tick += new System.EventHandler(this.tmrStateUpdate_Tick);
            // 
            // tmrVideoUpdate
            // 
            this.tmrVideoUpdate.Interval = 20;
            this.tmrVideoUpdate.Tick += new System.EventHandler(this.tmrVideoUpdate_Tick);
            // 
            // gbBarcodeScanning
            // 
            this.gbBarcodeScanning.Controls.Add(this.barcode);
            this.gbBarcodeScanning.Controls.Add(this.btnMockDoneScanning);
            this.gbBarcodeScanning.Controls.Add(this.btnMockScan);
            this.gbBarcodeScanning.Controls.Add(this.lblBarcode);
            this.gbBarcodeScanning.Controls.Add(this.nudMockScan);
            this.gbBarcodeScanning.Controls.Add(this.btnScanForBarcode);
            this.gbBarcodeScanning.Location = new System.Drawing.Point(678, 412);
            this.gbBarcodeScanning.Name = "gbBarcodeScanning";
            this.gbBarcodeScanning.Size = new System.Drawing.Size(287, 171);
            this.gbBarcodeScanning.TabIndex = 4;
            this.gbBarcodeScanning.TabStop = false;
            this.gbBarcodeScanning.Text = "Barcode Scanning";
            // 
            // barcode
            // 
            this.barcode.AutoSize = true;
            this.barcode.Location = new System.Drawing.Point(115, 135);
            this.barcode.Name = "barcode";
            this.barcode.Size = new System.Drawing.Size(47, 13);
            this.barcode.TabIndex = 5;
            this.barcode.Text = "Barcode";
            // 
            // btnMockDoneScanning
            // 
            this.btnMockDoneScanning.Location = new System.Drawing.Point(9, 86);
            this.btnMockDoneScanning.Name = "btnMockDoneScanning";
            this.btnMockDoneScanning.Size = new System.Drawing.Size(269, 23);
            this.btnMockDoneScanning.TabIndex = 3;
            this.btnMockDoneScanning.Text = "Mock done scanning";
            this.btnMockDoneScanning.UseVisualStyleBackColor = true;
            this.btnMockDoneScanning.Click += new System.EventHandler(this.btnMockDroneScanning_Click);
            // 
            // btnMockScan
            // 
            this.btnMockScan.Location = new System.Drawing.Point(135, 57);
            this.btnMockScan.Name = "btnMockScan";
            this.btnMockScan.Size = new System.Drawing.Size(143, 23);
            this.btnMockScan.TabIndex = 2;
            this.btnMockScan.Text = "Mock scan";
            this.btnMockScan.UseVisualStyleBackColor = true;
            this.btnMockScan.Click += new System.EventHandler(this.btnMockScan_Click);
            // 
            // lblBarcode
            // 
            this.lblBarcode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBarcode.AutoSize = true;
            this.lblBarcode.Location = new System.Drawing.Point(6, 133);
            this.lblBarcode.Name = "lblBarcode";
            this.lblBarcode.Size = new System.Drawing.Size(60, 13);
            this.lblBarcode.TabIndex = 1;
            this.lblBarcode.Text = "Scanresult:";
            // 
            // nudMockScan
            // 
            this.nudMockScan.Location = new System.Drawing.Point(9, 60);
            this.nudMockScan.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudMockScan.Name = "nudMockScan";
            this.nudMockScan.Size = new System.Drawing.Size(120, 20);
            this.nudMockScan.TabIndex = 1;
            // 
            // btnScanForBarcode
            // 
            this.btnScanForBarcode.Location = new System.Drawing.Point(9, 19);
            this.btnScanForBarcode.Name = "btnScanForBarcode";
            this.btnScanForBarcode.Size = new System.Drawing.Size(269, 23);
            this.btnScanForBarcode.TabIndex = 0;
            this.btnScanForBarcode.Text = "Scan for barcode";
            this.btnScanForBarcode.UseVisualStyleBackColor = true;
            this.btnScanForBarcode.Click += new System.EventHandler(this.btnScanForBarcode_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSmartScan);
            this.groupBox2.Controls.Add(this.btnCycleCount);
            this.groupBox2.Controls.Add(this.btnAutopilotStop);
            this.groupBox2.Controls.Add(this.btnAutopilotGo);
            this.groupBox2.Location = new System.Drawing.Point(6, 412);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(431, 54);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Autopilot";
            // 
            // btnSmartScan
            // 
            this.btnSmartScan.Location = new System.Drawing.Point(220, 19);
            this.btnSmartScan.Name = "btnSmartScan";
            this.btnSmartScan.Size = new System.Drawing.Size(75, 23);
            this.btnSmartScan.TabIndex = 8;
            this.btnSmartScan.Text = "Smart Scan";
            this.btnSmartScan.UseVisualStyleBackColor = true;
            this.btnSmartScan.Click += new System.EventHandler(this.btnSmartScan_Click);
            // 
            // btnCycleCount
            // 
            this.btnCycleCount.Location = new System.Drawing.Point(139, 19);
            this.btnCycleCount.Name = "btnCycleCount";
            this.btnCycleCount.Size = new System.Drawing.Size(75, 23);
            this.btnCycleCount.TabIndex = 7;
            this.btnCycleCount.Text = "Cycle count";
            this.btnCycleCount.UseVisualStyleBackColor = true;
            this.btnCycleCount.Click += new System.EventHandler(this.btnCycleCount_Click);
            // 
            // btnAutopilotStop
            // 
            this.btnAutopilotStop.Enabled = false;
            this.btnAutopilotStop.Location = new System.Drawing.Point(74, 19);
            this.btnAutopilotStop.Name = "btnAutopilotStop";
            this.btnAutopilotStop.Size = new System.Drawing.Size(59, 23);
            this.btnAutopilotStop.TabIndex = 2;
            this.btnAutopilotStop.Text = "Stop";
            this.btnAutopilotStop.UseVisualStyleBackColor = true;
            this.btnAutopilotStop.Click += new System.EventHandler(this.btnAutopilotStop_Click);
            // 
            // btnAutopilotGo
            // 
            this.btnAutopilotGo.Enabled = false;
            this.btnAutopilotGo.Location = new System.Drawing.Point(9, 19);
            this.btnAutopilotGo.Name = "btnAutopilotGo";
            this.btnAutopilotGo.Size = new System.Drawing.Size(59, 23);
            this.btnAutopilotGo.TabIndex = 0;
            this.btnAutopilotGo.Text = "Go!";
            this.btnAutopilotGo.UseVisualStyleBackColor = true;
            this.btnAutopilotGo.Click += new System.EventHandler(this.btnAutopilotGo_Click);
            // 
            // btnEmergency
            // 
            this.btnEmergency.Location = new System.Drawing.Point(6, 77);
            this.btnEmergency.Name = "btnEmergency";
            this.btnEmergency.Size = new System.Drawing.Size(101, 23);
            this.btnEmergency.TabIndex = 1;
            this.btnEmergency.Text = "Emergency";
            this.btnEmergency.UseVisualStyleBackColor = true;
            this.btnEmergency.Click += new System.EventHandler(this.btnEmergency_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox5);
            this.groupBox3.Controls.Add(this.groupBox4);
            this.groupBox3.Location = new System.Drawing.Point(6, 472);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(431, 161);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Manual pilot";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnBackward);
            this.groupBox5.Controls.Add(this.btnRotateLeft);
            this.groupBox5.Controls.Add(this.btnRotateRight);
            this.groupBox5.Controls.Add(this.btnRight);
            this.groupBox5.Controls.Add(this.btnLeft);
            this.groupBox5.Controls.Add(this.btnHover);
            this.groupBox5.Controls.Add(this.btnForward);
            this.groupBox5.Location = new System.Drawing.Point(14, 22);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(295, 133);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Flight controls";
            // 
            // btnBackward
            // 
            this.btnBackward.Location = new System.Drawing.Point(113, 77);
            this.btnBackward.Name = "btnBackward";
            this.btnBackward.Size = new System.Drawing.Size(75, 23);
            this.btnBackward.TabIndex = 6;
            this.btnBackward.Text = "Backward";
            this.btnBackward.UseVisualStyleBackColor = true;
            this.btnBackward.Click += new System.EventHandler(this.btnBackward_Click);
            // 
            // btnRotateLeft
            // 
            this.btnRotateLeft.Location = new System.Drawing.Point(32, 19);
            this.btnRotateLeft.Name = "btnRotateLeft";
            this.btnRotateLeft.Size = new System.Drawing.Size(75, 23);
            this.btnRotateLeft.TabIndex = 5;
            this.btnRotateLeft.Text = "Rotate left";
            this.btnRotateLeft.UseVisualStyleBackColor = true;
            this.btnRotateLeft.Click += new System.EventHandler(this.btnRotateLeft_Click);
            // 
            // btnRotateRight
            // 
            this.btnRotateRight.Location = new System.Drawing.Point(194, 19);
            this.btnRotateRight.Name = "btnRotateRight";
            this.btnRotateRight.Size = new System.Drawing.Size(75, 23);
            this.btnRotateRight.TabIndex = 4;
            this.btnRotateRight.Text = "Rotate right";
            this.btnRotateRight.UseVisualStyleBackColor = true;
            this.btnRotateRight.Click += new System.EventHandler(this.btnRotateRight_Click);
            // 
            // btnRight
            // 
            this.btnRight.Location = new System.Drawing.Point(194, 48);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(75, 23);
            this.btnRight.TabIndex = 3;
            this.btnRight.Text = "Right";
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            // 
            // btnLeft
            // 
            this.btnLeft.Location = new System.Drawing.Point(32, 48);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(75, 23);
            this.btnLeft.TabIndex = 2;
            this.btnLeft.Text = "Left";
            this.btnLeft.UseVisualStyleBackColor = true;
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            // 
            // btnHover
            // 
            this.btnHover.Location = new System.Drawing.Point(113, 48);
            this.btnHover.Name = "btnHover";
            this.btnHover.Size = new System.Drawing.Size(75, 23);
            this.btnHover.TabIndex = 1;
            this.btnHover.Text = "Hover";
            this.btnHover.UseVisualStyleBackColor = true;
            this.btnHover.Click += new System.EventHandler(this.btnHover_Click);
            // 
            // btnForward
            // 
            this.btnForward.Location = new System.Drawing.Point(113, 19);
            this.btnForward.Name = "btnForward";
            this.btnForward.Size = new System.Drawing.Size(75, 23);
            this.btnForward.TabIndex = 0;
            this.btnForward.Text = "Forward";
            this.btnForward.UseVisualStyleBackColor = true;
            this.btnForward.Click += new System.EventHandler(this.btnForward_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnFlatTrim);
            this.groupBox4.Controls.Add(this.btnTakeoff);
            this.groupBox4.Controls.Add(this.btnLand);
            this.groupBox4.Controls.Add(this.btnEmergency);
            this.groupBox4.Location = new System.Drawing.Point(315, 19);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(110, 136);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Flight state";
            // 
            // btnFlatTrim
            // 
            this.btnFlatTrim.Location = new System.Drawing.Point(6, 106);
            this.btnFlatTrim.Name = "btnFlatTrim";
            this.btnFlatTrim.Size = new System.Drawing.Size(101, 23);
            this.btnFlatTrim.TabIndex = 3;
            this.btnFlatTrim.Text = "Flat trim";
            this.btnFlatTrim.UseVisualStyleBackColor = true;
            this.btnFlatTrim.Click += new System.EventHandler(this.btnFlatTrim_Click);
            // 
            // btnTakeoff
            // 
            this.btnTakeoff.Location = new System.Drawing.Point(6, 19);
            this.btnTakeoff.Name = "btnTakeoff";
            this.btnTakeoff.Size = new System.Drawing.Size(101, 23);
            this.btnTakeoff.TabIndex = 0;
            this.btnTakeoff.Text = "Takeoff";
            this.btnTakeoff.UseVisualStyleBackColor = true;
            this.btnTakeoff.Click += new System.EventHandler(this.btnTakeoff_Click);
            // 
            // btnLand
            // 
            this.btnLand.Location = new System.Drawing.Point(6, 48);
            this.btnLand.Name = "btnLand";
            this.btnLand.Size = new System.Drawing.Size(101, 23);
            this.btnLand.TabIndex = 2;
            this.btnLand.Text = "Land";
            this.btnLand.UseVisualStyleBackColor = true;
            this.btnLand.Click += new System.EventHandler(this.btnLand_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(971, 662);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.gbBarcodeScanning);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbTelemetry);
            this.Controls.Add(this.gbVideoFeed);
            this.Name = "MainForm";
            this.Text = "MainForm";
            ((System.ComponentModel.ISupportInitialize)(this.pbVideo)).EndInit();
            this.gbVideoFeed.ResumeLayout(false);
            this.gbTelemetry.ResumeLayout(false);
            this.gbTelemetry.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.gbBarcodeScanning.ResumeLayout(false);
            this.gbBarcodeScanning.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMockScan)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbVideo;
        private System.Windows.Forms.GroupBox gbVideoFeed;
        private System.Windows.Forms.GroupBox gbTelemetry;
        private System.Windows.Forms.Label lblWifi;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblBattery;
        private System.Windows.Forms.Label lblVelocityX;
        private System.Windows.Forms.Label lblVelocityZ;
        private System.Windows.Forms.Label lblVelocityY;
        private System.Windows.Forms.Label lblAltitude;
        private System.Windows.Forms.Label lblRoll;
        private System.Windows.Forms.Label lblPitch;
        private System.Windows.Forms.Label lblYaw;
        private System.Windows.Forms.Label lblNavigationState;
        private System.Windows.Forms.Label lblDesc9;
        private System.Windows.Forms.Label lblDesc8;
        private System.Windows.Forms.Label lblDesc7;
        private System.Windows.Forms.Label lblDesc6;
        private System.Windows.Forms.Label lblDesc5;
        private System.Windows.Forms.Label lblDesc4;
        private System.Windows.Forms.Label lblDesc3;
        private System.Windows.Forms.Label lblDesc2;
        private System.Windows.Forms.Label lblDesc1;
        private System.Windows.Forms.Label lblDesc0;
        private System.Windows.Forms.Label lblConnectionStatus;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Timer tmrStateUpdate;
        private System.Windows.Forms.Timer tmrVideoUpdate;
        private System.Windows.Forms.GroupBox gbBarcodeScanning;
        private System.Windows.Forms.Button btnScanForBarcode;
        private System.Windows.Forms.Label lblBarcode;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnAutopilotGo;
        private System.Windows.Forms.Button btnEmergency;
        private System.Windows.Forms.Button btnAutopilotStop;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnLand;
        private System.Windows.Forms.Button btnTakeoff;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnHover;
        private System.Windows.Forms.Button btnForward;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.Button btnRotateRight;
        private System.Windows.Forms.Button btnRotateLeft;
        private System.Windows.Forms.Button btnBackward;
        private System.Windows.Forms.Button btnFlatTrim;
        private System.Windows.Forms.Button btnMockScan;
        private System.Windows.Forms.NumericUpDown nudMockScan;
        private System.Windows.Forms.Button btnMockDoneScanning;
        private System.Windows.Forms.Label barcode;
        private System.Windows.Forms.Button btnCycleCount;
        private System.Windows.Forms.Button btnSmartScan;
    }
}