using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AR.Drone.Client;
using AR.Drone.Data.Navigation;
using AR.Drone.Client.Command;
using AR.Drone.Video;
using AR.Drone.Client.Configuration;
using AR.Drone.Data;
using AR.Drone.WinApp;

namespace DroneControl
{
    public partial class MainForm : Form
    {
        DroneClient droneClient;
        private readonly VideoPacketDecoderWorker videoPacketDecoderWorker;
        private Settings settings;
        private VideoFrame frame;
        private Bitmap frameBitmap;
        private uint frameNumber;
        private NavigationData data;


        public MainForm()
        {
            InitializeComponent();
            droneClient = new DroneClient();
            droneClient.NavigationDataAcquired += DroneClient_NavigationDataAcquired;
            droneClient.VideoPacketAcquired += OnVideoPacketAcquired;
            videoPacketDecoderWorker = new VideoPacketDecoderWorker(PixelFormat.BGR24, true, OnVideoPacketDecoded);
            videoPacketDecoderWorker.Start();
            tmrVideoUpdate.Enabled = true;
            tmrStateUpdate.Enabled = true;
        }

        private void OnVideoPacketAcquired(VideoPacket packet)
        {
            if (videoPacketDecoderWorker.IsAlive)
                videoPacketDecoderWorker.EnqueuePacket(packet);
        }

        private void OnVideoPacketDecoded(VideoFrame frame)
        {
            this.frame = frame;
        }

        private void tmrVideoUpdate_Tick(object sender, EventArgs e)
        {
            if (frame == null || frameNumber == frame.Number)
                return;
            frameNumber = frame.Number;

            if (frameBitmap == null)
                frameBitmap = VideoHelper.CreateBitmap(ref frame);
            else
                VideoHelper.UpdateBitmap(ref frameBitmap, ref frame);

            pbVideo.Image = frameBitmap;
        }

        private void btnSendConfig_Click(object sender, EventArgs e)
        {
            var sendConfigTask = new Task(() =>
            {
                if (settings == null)
                {
                    settings = new Settings();
                }

                if (string.IsNullOrEmpty(settings.Custom.SessionId) ||
                    settings.Custom.SessionId == "00000000")
                {
                    // set new session, application and profile
                    droneClient.AckControlAndWaitForConfirmation(); // wait for the control confirmation

                    settings.Custom.SessionId = Settings.NewId();
                    droneClient.Send(settings);

                    droneClient.AckControlAndWaitForConfirmation();

                    settings.Custom.ProfileId = Settings.NewId();
                    droneClient.Send(settings);

                    droneClient.AckControlAndWaitForConfirmation();

                    settings.Custom.ApplicationId = Settings.NewId();
                    droneClient.Send(settings);

                    droneClient.AckControlAndWaitForConfirmation();
                }

                settings.General.NavdataDemo = false;
                settings.General.NavdataOptions = NavdataOptions.All;

                settings.Video.BitrateCtrlMode = VideoBitrateControlMode.Dynamic;
                settings.Video.Bitrate = 1000;
                settings.Video.MaxBitrate = 2000;

                //send all changes in one pice
                droneClient.Send(settings);
            });
            sendConfigTask.Start();
        }

        private void DroneClient_NavigationDataAcquired(NavigationData data)
        {
            this.data = data;
        }

        private void btnTakeOff_Click(object sender, EventArgs e)
        {
            droneClient.Takeoff();
            btnTakeOff.Enabled = false;
            btnLand.Enabled = true;
        }

        private void btnLand_Click(object sender, EventArgs e)
        {
            droneClient.Land();
            btnLand.Enabled = false;
            btnTakeOff.Enabled = true;
        }

        private void btnEmergency_Click(object sender, EventArgs e)
        {
            droneClient.Emergency();
            btnEmergency.Enabled = false;
            btnResetEmergency.Enabled = true;
        }

        private void btnResetEmergency_Click(object sender, EventArgs e)
        {
            droneClient.ResetEmergency();
            btnResetEmergency.Enabled = false;
            btnEmergency.Enabled = true;
        }

        private void btnFlatTrim_Click(object sender, EventArgs e)
        {
            droneClient.FlatTrim();
        }

        private void btnHover_Click(object sender, EventArgs e)
        {
            droneClient.Hover();
        }

        private void btnUpdateHeading_Click(object sender, EventArgs e)
        {
            float roll = (float)nudRoll.Value;
            float yaw = (float)nudYaw.Value;
            float pitch = (float)nudPitch.Value;
            float gaz = (float)nudGaz.Value;
            droneClient.Progress(FlightMode.Progressive, roll, yaw, pitch, gaz);
        }

        private void btnActivate_Click(object sender, EventArgs e)
        {
            droneClient.Start();
        }

        private void btnDeactivate_Click(object sender, EventArgs e)
        {
            droneClient.Stop();
        }

        private void tmrStateUpdate_Tick(object sender, EventArgs e)
        {
            if(data != null)
            {
                //Updating the form with new navigation data.
                lblNavigationState.Text = data.State.ToString();
                lblYaw.Text = data.Yaw.ToString();
                lblPitch.Text = data.Pitch.ToString();
                lblRoll.Text = data.Roll.ToString();
                lblAltitude.Text = data.Altitude.ToString();
                lblVelocityZ.Text = "Z: " + data.Velocity.Z.ToString();
                lblVelocityY.Text = "Y: " + data.Velocity.Y.ToString();
                lblVelocityX.Text = "X: " + data.Velocity.X.ToString();
                lblBattery.Text = data.Battery.Percentage.ToString() + "%";
                lblTime.Text = data.Time.ToString();
                lblWifi.Text = data.Wifi.LinkQuality.ToString();
            }
            
        }
    }
}
