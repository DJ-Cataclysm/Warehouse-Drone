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
            this.btnTakeOff = new System.Windows.Forms.Button();
            this.btnLand = new System.Windows.Forms.Button();
            this.btnEmergency = new System.Windows.Forms.Button();
            this.btnResetEmergency = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnUpdateHeading = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.nudGaz = new System.Windows.Forms.NumericUpDown();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.nudRoll = new System.Windows.Forms.NumericUpDown();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.nudYaw = new System.Windows.Forms.NumericUpDown();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.nudPitch = new System.Windows.Forms.NumericUpDown();
            this.btnHover = new System.Windows.Forms.Button();
            this.btnFlatTrim = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSendConfig = new System.Windows.Forms.Button();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
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
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.pbVideo = new System.Windows.Forms.PictureBox();
            this.tmrVideoUpdate = new System.Windows.Forms.Timer(this.components);
            this.btnActivate = new System.Windows.Forms.Button();
            this.btnDeactivate = new System.Windows.Forms.Button();
            this.tmrStateUpdate = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudGaz)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRoll)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudYaw)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPitch)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbVideo)).BeginInit();
            this.SuspendLayout();
            // 
            // btnTakeOff
            // 
            this.btnTakeOff.Location = new System.Drawing.Point(6, 19);
            this.btnTakeOff.Name = "btnTakeOff";
            this.btnTakeOff.Size = new System.Drawing.Size(76, 23);
            this.btnTakeOff.TabIndex = 0;
            this.btnTakeOff.Text = "Take off";
            this.btnTakeOff.UseVisualStyleBackColor = true;
            this.btnTakeOff.Click += new System.EventHandler(this.btnTakeOff_Click);
            // 
            // btnLand
            // 
            this.btnLand.Enabled = false;
            this.btnLand.Location = new System.Drawing.Point(88, 19);
            this.btnLand.Name = "btnLand";
            this.btnLand.Size = new System.Drawing.Size(76, 23);
            this.btnLand.TabIndex = 1;
            this.btnLand.Text = "Land";
            this.btnLand.UseVisualStyleBackColor = true;
            this.btnLand.Click += new System.EventHandler(this.btnLand_Click);
            // 
            // btnEmergency
            // 
            this.btnEmergency.Location = new System.Drawing.Point(570, 19);
            this.btnEmergency.Name = "btnEmergency";
            this.btnEmergency.Size = new System.Drawing.Size(75, 23);
            this.btnEmergency.TabIndex = 2;
            this.btnEmergency.Text = "Emergency";
            this.btnEmergency.UseVisualStyleBackColor = true;
            this.btnEmergency.Click += new System.EventHandler(this.btnEmergency_Click);
            // 
            // btnResetEmergency
            // 
            this.btnResetEmergency.Enabled = false;
            this.btnResetEmergency.Location = new System.Drawing.Point(651, 19);
            this.btnResetEmergency.Name = "btnResetEmergency";
            this.btnResetEmergency.Size = new System.Drawing.Size(75, 23);
            this.btnResetEmergency.TabIndex = 3;
            this.btnResetEmergency.Text = "Reset emergency";
            this.btnResetEmergency.UseVisualStyleBackColor = true;
            this.btnResetEmergency.Click += new System.EventHandler(this.btnResetEmergency_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnUpdateHeading);
            this.groupBox1.Controls.Add(this.groupBox6);
            this.groupBox1.Controls.Add(this.groupBox5);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.btnHover);
            this.groupBox1.Controls.Add(this.btnFlatTrim);
            this.groupBox1.Location = new System.Drawing.Point(6, 305);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(274, 196);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Flight Controls";
            // 
            // btnUpdateHeading
            // 
            this.btnUpdateHeading.Location = new System.Drawing.Point(7, 152);
            this.btnUpdateHeading.Name = "btnUpdateHeading";
            this.btnUpdateHeading.Size = new System.Drawing.Size(258, 34);
            this.btnUpdateHeading.TabIndex = 8;
            this.btnUpdateHeading.Text = "Update heading";
            this.btnUpdateHeading.UseVisualStyleBackColor = true;
            this.btnUpdateHeading.Click += new System.EventHandler(this.btnUpdateHeading_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.nudGaz);
            this.groupBox6.Location = new System.Drawing.Point(139, 100);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(126, 46);
            this.groupBox6.TabIndex = 7;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Gaz [-1..1]";
            // 
            // nudGaz
            // 
            this.nudGaz.DecimalPlaces = 2;
            this.nudGaz.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudGaz.Location = new System.Drawing.Point(6, 19);
            this.nudGaz.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudGaz.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.nudGaz.Name = "nudGaz";
            this.nudGaz.Size = new System.Drawing.Size(114, 20);
            this.nudGaz.TabIndex = 3;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.nudRoll);
            this.groupBox5.Location = new System.Drawing.Point(7, 100);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(126, 46);
            this.groupBox5.TabIndex = 6;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Roll [-1..1]";
            // 
            // nudRoll
            // 
            this.nudRoll.DecimalPlaces = 2;
            this.nudRoll.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudRoll.Location = new System.Drawing.Point(6, 19);
            this.nudRoll.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRoll.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.nudRoll.Name = "nudRoll";
            this.nudRoll.Size = new System.Drawing.Size(114, 20);
            this.nudRoll.TabIndex = 3;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.nudYaw);
            this.groupBox4.Location = new System.Drawing.Point(139, 48);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(126, 46);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Yaw [-1..1]";
            // 
            // nudYaw
            // 
            this.nudYaw.DecimalPlaces = 2;
            this.nudYaw.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudYaw.Location = new System.Drawing.Point(6, 19);
            this.nudYaw.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudYaw.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.nudYaw.Name = "nudYaw";
            this.nudYaw.Size = new System.Drawing.Size(114, 20);
            this.nudYaw.TabIndex = 3;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.nudPitch);
            this.groupBox3.Location = new System.Drawing.Point(7, 48);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(126, 46);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Pitch [-1..1]";
            // 
            // nudPitch
            // 
            this.nudPitch.DecimalPlaces = 2;
            this.nudPitch.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudPitch.Location = new System.Drawing.Point(6, 19);
            this.nudPitch.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudPitch.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.nudPitch.Name = "nudPitch";
            this.nudPitch.Size = new System.Drawing.Size(114, 20);
            this.nudPitch.TabIndex = 3;
            // 
            // btnHover
            // 
            this.btnHover.Location = new System.Drawing.Point(139, 19);
            this.btnHover.Name = "btnHover";
            this.btnHover.Size = new System.Drawing.Size(126, 23);
            this.btnHover.TabIndex = 1;
            this.btnHover.Text = "Hover";
            this.btnHover.UseVisualStyleBackColor = true;
            this.btnHover.Click += new System.EventHandler(this.btnHover_Click);
            // 
            // btnFlatTrim
            // 
            this.btnFlatTrim.Location = new System.Drawing.Point(7, 19);
            this.btnFlatTrim.Name = "btnFlatTrim";
            this.btnFlatTrim.Size = new System.Drawing.Size(126, 23);
            this.btnFlatTrim.TabIndex = 0;
            this.btnFlatTrim.Text = "Flat Trim";
            this.btnFlatTrim.UseVisualStyleBackColor = true;
            this.btnFlatTrim.Click += new System.EventHandler(this.btnFlatTrim_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnDeactivate);
            this.groupBox2.Controls.Add(this.btnActivate);
            this.groupBox2.Controls.Add(this.btnSendConfig);
            this.groupBox2.Controls.Add(this.btnResetEmergency);
            this.groupBox2.Controls.Add(this.btnEmergency);
            this.groupBox2.Controls.Add(this.btnLand);
            this.groupBox2.Controls.Add(this.btnTakeOff);
            this.groupBox2.Location = new System.Drawing.Point(286, 451);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(734, 50);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Drone State";
            // 
            // btnSendConfig
            // 
            this.btnSendConfig.Location = new System.Drawing.Point(221, 19);
            this.btnSendConfig.Name = "btnSendConfig";
            this.btnSendConfig.Size = new System.Drawing.Size(76, 23);
            this.btnSendConfig.TabIndex = 4;
            this.btnSendConfig.Text = "Send Config";
            this.btnSendConfig.UseVisualStyleBackColor = true;
            this.btnSendConfig.Click += new System.EventHandler(this.btnSendConfig_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.lblWifi);
            this.groupBox7.Controls.Add(this.lblTime);
            this.groupBox7.Controls.Add(this.lblBattery);
            this.groupBox7.Controls.Add(this.lblVelocityX);
            this.groupBox7.Controls.Add(this.lblVelocityZ);
            this.groupBox7.Controls.Add(this.lblVelocityY);
            this.groupBox7.Controls.Add(this.lblAltitude);
            this.groupBox7.Controls.Add(this.lblRoll);
            this.groupBox7.Controls.Add(this.lblPitch);
            this.groupBox7.Controls.Add(this.lblYaw);
            this.groupBox7.Controls.Add(this.lblNavigationState);
            this.groupBox7.Controls.Add(this.lblDesc9);
            this.groupBox7.Controls.Add(this.lblDesc8);
            this.groupBox7.Controls.Add(this.lblDesc7);
            this.groupBox7.Controls.Add(this.lblDesc6);
            this.groupBox7.Controls.Add(this.lblDesc5);
            this.groupBox7.Controls.Add(this.lblDesc4);
            this.groupBox7.Controls.Add(this.lblDesc3);
            this.groupBox7.Controls.Add(this.lblDesc2);
            this.groupBox7.Controls.Add(this.lblDesc1);
            this.groupBox7.Location = new System.Drawing.Point(6, 12);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(274, 283);
            this.groupBox7.TabIndex = 6;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Navigational Data";
            // 
            // lblWifi
            // 
            this.lblWifi.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblWifi.Location = new System.Drawing.Point(107, 250);
            this.lblWifi.Name = "lblWifi";
            this.lblWifi.Size = new System.Drawing.Size(158, 23);
            this.lblWifi.TabIndex = 26;
            this.lblWifi.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTime
            // 
            this.lblTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblTime.Location = new System.Drawing.Point(107, 227);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(158, 23);
            this.lblTime.TabIndex = 25;
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBattery
            // 
            this.lblBattery.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblBattery.Location = new System.Drawing.Point(107, 204);
            this.lblBattery.Name = "lblBattery";
            this.lblBattery.Size = new System.Drawing.Size(158, 23);
            this.lblBattery.TabIndex = 24;
            this.lblBattery.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblVelocityX
            // 
            this.lblVelocityX.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblVelocityX.Location = new System.Drawing.Point(107, 181);
            this.lblVelocityX.Name = "lblVelocityX";
            this.lblVelocityX.Size = new System.Drawing.Size(158, 23);
            this.lblVelocityX.TabIndex = 23;
            this.lblVelocityX.Text = "X:";
            this.lblVelocityX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblVelocityZ
            // 
            this.lblVelocityZ.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblVelocityZ.Location = new System.Drawing.Point(107, 135);
            this.lblVelocityZ.Name = "lblVelocityZ";
            this.lblVelocityZ.Size = new System.Drawing.Size(158, 23);
            this.lblVelocityZ.TabIndex = 22;
            this.lblVelocityZ.Text = "Z:";
            this.lblVelocityZ.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblVelocityY
            // 
            this.lblVelocityY.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblVelocityY.Location = new System.Drawing.Point(107, 158);
            this.lblVelocityY.Name = "lblVelocityY";
            this.lblVelocityY.Size = new System.Drawing.Size(158, 23);
            this.lblVelocityY.TabIndex = 21;
            this.lblVelocityY.Text = "Y:";
            this.lblVelocityY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAltitude
            // 
            this.lblAltitude.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblAltitude.Location = new System.Drawing.Point(107, 111);
            this.lblAltitude.Name = "lblAltitude";
            this.lblAltitude.Size = new System.Drawing.Size(158, 23);
            this.lblAltitude.TabIndex = 20;
            this.lblAltitude.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRoll
            // 
            this.lblRoll.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblRoll.Location = new System.Drawing.Point(107, 88);
            this.lblRoll.Name = "lblRoll";
            this.lblRoll.Size = new System.Drawing.Size(158, 23);
            this.lblRoll.TabIndex = 19;
            this.lblRoll.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPitch
            // 
            this.lblPitch.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblPitch.Location = new System.Drawing.Point(107, 65);
            this.lblPitch.Name = "lblPitch";
            this.lblPitch.Size = new System.Drawing.Size(158, 23);
            this.lblPitch.TabIndex = 18;
            this.lblPitch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblYaw
            // 
            this.lblYaw.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblYaw.Location = new System.Drawing.Point(107, 42);
            this.lblYaw.Name = "lblYaw";
            this.lblYaw.Size = new System.Drawing.Size(158, 23);
            this.lblYaw.TabIndex = 17;
            this.lblYaw.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblNavigationState
            // 
            this.lblNavigationState.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblNavigationState.Location = new System.Drawing.Point(107, 19);
            this.lblNavigationState.Name = "lblNavigationState";
            this.lblNavigationState.Size = new System.Drawing.Size(158, 23);
            this.lblNavigationState.TabIndex = 16;
            this.lblNavigationState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDesc9
            // 
            this.lblDesc9.AutoSize = true;
            this.lblDesc9.Location = new System.Drawing.Point(73, 255);
            this.lblDesc9.Name = "lblDesc9";
            this.lblDesc9.Size = new System.Drawing.Size(28, 13);
            this.lblDesc9.TabIndex = 15;
            this.lblDesc9.Text = "Wifi:";
            // 
            // lblDesc8
            // 
            this.lblDesc8.AutoSize = true;
            this.lblDesc8.Location = new System.Drawing.Point(42, 232);
            this.lblDesc8.Name = "lblDesc8";
            this.lblDesc8.Size = new System.Drawing.Size(59, 13);
            this.lblDesc8.TabIndex = 14;
            this.lblDesc8.Text = "Time (sec):";
            // 
            // lblDesc7
            // 
            this.lblDesc7.AutoSize = true;
            this.lblDesc7.Location = new System.Drawing.Point(41, 209);
            this.lblDesc7.Name = "lblDesc7";
            this.lblDesc7.Size = new System.Drawing.Size(60, 13);
            this.lblDesc7.TabIndex = 13;
            this.lblDesc7.Text = "Battery (%):";
            // 
            // lblDesc6
            // 
            this.lblDesc6.AutoSize = true;
            this.lblDesc6.Location = new System.Drawing.Point(33, 140);
            this.lblDesc6.Name = "lblDesc6";
            this.lblDesc6.Size = new System.Drawing.Size(68, 13);
            this.lblDesc6.TabIndex = 12;
            this.lblDesc6.Text = "Velocity (v3):";
            // 
            // lblDesc5
            // 
            this.lblDesc5.AutoSize = true;
            this.lblDesc5.Location = new System.Drawing.Point(39, 116);
            this.lblDesc5.Name = "lblDesc5";
            this.lblDesc5.Size = new System.Drawing.Size(62, 13);
            this.lblDesc5.TabIndex = 11;
            this.lblDesc5.Text = "Altitude (m):";
            // 
            // lblDesc4
            // 
            this.lblDesc4.AutoSize = true;
            this.lblDesc4.Location = new System.Drawing.Point(49, 93);
            this.lblDesc4.Name = "lblDesc4";
            this.lblDesc4.Size = new System.Drawing.Size(52, 13);
            this.lblDesc4.TabIndex = 10;
            this.lblDesc4.Text = "Roll (rad):";
            // 
            // lblDesc3
            // 
            this.lblDesc3.AutoSize = true;
            this.lblDesc3.Location = new System.Drawing.Point(43, 70);
            this.lblDesc3.Name = "lblDesc3";
            this.lblDesc3.Size = new System.Drawing.Size(58, 13);
            this.lblDesc3.TabIndex = 9;
            this.lblDesc3.Text = "Pitch (rad):";
            // 
            // lblDesc2
            // 
            this.lblDesc2.AutoSize = true;
            this.lblDesc2.Location = new System.Drawing.Point(46, 47);
            this.lblDesc2.Name = "lblDesc2";
            this.lblDesc2.Size = new System.Drawing.Size(55, 13);
            this.lblDesc2.TabIndex = 8;
            this.lblDesc2.Text = "Yaw (rad):";
            // 
            // lblDesc1
            // 
            this.lblDesc1.AutoSize = true;
            this.lblDesc1.Location = new System.Drawing.Point(6, 24);
            this.lblDesc1.Name = "lblDesc1";
            this.lblDesc1.Size = new System.Drawing.Size(95, 13);
            this.lblDesc1.TabIndex = 7;
            this.lblDesc1.Text = "Navigational state:";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.pbVideo);
            this.groupBox8.Location = new System.Drawing.Point(286, 12);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(734, 433);
            this.groupBox8.TabIndex = 7;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Video feed";
            // 
            // pbVideo
            // 
            this.pbVideo.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.pbVideo.Location = new System.Drawing.Point(6, 19);
            this.pbVideo.Name = "pbVideo";
            this.pbVideo.Size = new System.Drawing.Size(720, 405);
            this.pbVideo.TabIndex = 0;
            this.pbVideo.TabStop = false;
            // 
            // tmrVideoUpdate
            // 
            this.tmrVideoUpdate.Interval = 20;
            this.tmrVideoUpdate.Tick += new System.EventHandler(this.tmrVideoUpdate_Tick);
            // 
            // btnActivate
            // 
            this.btnActivate.Location = new System.Drawing.Point(345, 19);
            this.btnActivate.Name = "btnActivate";
            this.btnActivate.Size = new System.Drawing.Size(76, 23);
            this.btnActivate.TabIndex = 5;
            this.btnActivate.Text = "Activate";
            this.btnActivate.UseVisualStyleBackColor = true;
            this.btnActivate.Click += new System.EventHandler(this.btnActivate_Click);
            // 
            // btnDeactivate
            // 
            this.btnDeactivate.Location = new System.Drawing.Point(427, 19);
            this.btnDeactivate.Name = "btnDeactivate";
            this.btnDeactivate.Size = new System.Drawing.Size(75, 23);
            this.btnDeactivate.TabIndex = 6;
            this.btnDeactivate.Text = "Deactivate";
            this.btnDeactivate.UseVisualStyleBackColor = true;
            this.btnDeactivate.Click += new System.EventHandler(this.btnDeactivate_Click);
            // 
            // tmrStateUpdate
            // 
            this.tmrStateUpdate.Interval = 500;
            this.tmrStateUpdate.Tick += new System.EventHandler(this.tmrStateUpdate_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1033, 506);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "MainForm";
            this.Text = "Drone Control";
            this.groupBox1.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudGaz)).EndInit();
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudRoll)).EndInit();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudYaw)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudPitch)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbVideo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnTakeOff;
        private System.Windows.Forms.Button btnLand;
        private System.Windows.Forms.Button btnEmergency;
        private System.Windows.Forms.Button btnResetEmergency;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnHover;
        private System.Windows.Forms.Button btnFlatTrim;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown nudPitch;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.NumericUpDown nudRoll;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.NumericUpDown nudYaw;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.NumericUpDown nudGaz;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label lblDesc5;
        private System.Windows.Forms.Label lblDesc4;
        private System.Windows.Forms.Label lblDesc3;
        private System.Windows.Forms.Label lblDesc2;
        private System.Windows.Forms.Label lblDesc1;
        private System.Windows.Forms.Label lblDesc9;
        private System.Windows.Forms.Label lblDesc8;
        private System.Windows.Forms.Label lblDesc7;
        private System.Windows.Forms.Label lblDesc6;
        private System.Windows.Forms.Label lblNavigationState;
        private System.Windows.Forms.Label lblRoll;
        private System.Windows.Forms.Label lblPitch;
        private System.Windows.Forms.Label lblYaw;
        private System.Windows.Forms.Label lblAltitude;
        private System.Windows.Forms.Label lblVelocityX;
        private System.Windows.Forms.Label lblVelocityZ;
        private System.Windows.Forms.Label lblVelocityY;
        private System.Windows.Forms.Label lblBattery;
        private System.Windows.Forms.Label lblWifi;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Button btnUpdateHeading;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.PictureBox pbVideo;
        private System.Windows.Forms.Timer tmrVideoUpdate;
        private System.Windows.Forms.Button btnSendConfig;
        private System.Windows.Forms.Button btnDeactivate;
        private System.Windows.Forms.Button btnActivate;
        private System.Windows.Forms.Timer tmrStateUpdate;
    }
}

