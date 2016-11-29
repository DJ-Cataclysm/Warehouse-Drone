using AR.Drone.Client;
using AR.Drone.Client.Configuration;
using AR.Drone.Data;
using AR.Drone.Data.Navigation;
using AR.Drone.Video;
using AR.Drone.WinApp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DroneControl
{
    public partial class MainForm : Form
    {
        private readonly DroneClient _droneClient;
        private readonly VideoPacketDecoderWorker _videoPacketDecoderWorker;
        private VideoFrame _frame;
        private Bitmap _frameBitmap;
        private uint _frameNumber;
        private NavigationData _navigationData;
        private NavigationPacket _navigationPacket;

        public MainForm()
        {
            InitializeComponent();

            _videoPacketDecoderWorker = new VideoPacketDecoderWorker(PixelFormat.BGR24, true, OnVideoPacketDecoded);
            _videoPacketDecoderWorker.Start();

            _droneClient = new DroneClient("192.168.1.1");
            _droneClient.NavigationPacketAcquired += OnNavigationPacketAcquired;
            _droneClient.VideoPacketAcquired += OnVideoPacketAcquired;
            _droneClient.NavigationDataAcquired += data => _navigationData = data;

            tmrStateUpdate.Enabled = true;
            tmrVideoUpdate.Enabled = true;

            _videoPacketDecoderWorker.UnhandledException += UnhandledException;
        }

        private void UnhandledException(object sender, Exception exception)
        {
            MessageBox.Show(exception.ToString(), "Unhandled Exception (Ctrl+C)", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Text += Environment.Is64BitProcess ? " [64-bit]" : " [32-bit]";
        }

        protected override void OnClosed(EventArgs e)
        {
            _droneClient.Dispose();
            _videoPacketDecoderWorker.Dispose();
            base.OnClosed(e);
        }

        private void OnNavigationPacketAcquired(NavigationPacket packet)
        {
            _navigationPacket = packet;
        }

        private void OnVideoPacketAcquired(VideoPacket packet)
        {
            if (_videoPacketDecoderWorker.IsAlive)
                _videoPacketDecoderWorker.EnqueuePacket(packet);
        }

        private void OnVideoPacketDecoded(VideoFrame frame)
        {
            _frame = frame;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            _droneClient.Start();
            lblConnectionStatus.Text = "Connected";
            btnConnect.Enabled = false;
            btnDisconnect.Enabled = true;
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            _droneClient.Stop();
            lblConnectionStatus.Text = "Disconnected";
            btnConnect.Enabled = true;
            btnDisconnect.Enabled = false;
        }

        private void tmrVideoUpdate_Tick(object sender, EventArgs e)
        {
            if (_frame == null || _frameNumber == _frame.Number)
                return;
            _frameNumber = _frame.Number;

            if (_frameBitmap == null)
                _frameBitmap = VideoHelper.CreateBitmap(ref _frame);
            else
                VideoHelper.UpdateBitmap(ref _frameBitmap, ref _frame);

            pbVideo.Image = _frameBitmap;
        }

        private void tmrStateUpdate_Tick(object sender, EventArgs e)
        {
            if (_navigationData != null)
            {
                //Updating the form with new navigation data.
                lblNavigationState.Text = _navigationData.State.ToString();
                lblYaw.Text = _navigationData.Yaw.ToString();
                lblPitch.Text = _navigationData.Pitch.ToString();
                lblRoll.Text = _navigationData.Roll.ToString();
                lblAltitude.Text = _navigationData.Altitude.ToString();
                lblVelocityZ.Text = "Z: " + _navigationData.Velocity.Z.ToString();
                lblVelocityY.Text = "Y: " + _navigationData.Velocity.Y.ToString();
                lblVelocityX.Text = "X: " + _navigationData.Velocity.X.ToString();
                lblBattery.Text = _navigationData.Battery.Percentage.ToString() + "%";
                lblTime.Text = _navigationData.Time.ToString();
                lblWifi.Text = _navigationData.Wifi.LinkQuality.ToString();
            }
        }
    }
}
