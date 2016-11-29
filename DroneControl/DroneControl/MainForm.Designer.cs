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
            ((System.ComponentModel.ISupportInitialize)(this.pbVideo)).BeginInit();
            this.gbVideoFeed.SuspendLayout();
            this.gbTelemetry.SuspendLayout();
            this.groupBox1.SuspendLayout();
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(971, 415);
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
    }
}