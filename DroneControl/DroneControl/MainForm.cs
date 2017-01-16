using AR.Drone.Client;
using AR.Drone.Data;
using AR.Drone.Data.Navigation;
using AR.Drone.Video;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DroneControl
{
    public partial class MainForm : Form
    {
        private DroneController _droneController;
        private readonly VideoPacketDecoderWorker _videoPacketDecoderWorker;
        private VideoFrame _frame;
        private Bitmap _frameBitmap;
        private uint _frameNumber;
        private NavigationData _navigationData;
        public WMS.MainForm wmsForm { get; set; }

        public MainForm()
        {
            InitializeComponent();

            //Start videopacketdecoder worker
            _videoPacketDecoderWorker = new VideoPacketDecoderWorker(AR.Drone.Video.PixelFormat.BGR24, true, OnVideoPacketDecoded);
            _videoPacketDecoderWorker.Start();

            _droneController = new DroneController(this);
            _droneController.attachEventHandlers(OnVideoPacketAcquired, OnNavigationDataAcquired);

            tmrStateUpdate.Enabled = true;
            tmrVideoUpdate.Enabled = true;

            //Attach exceptionhandler to the videopacketdecoder worker
            _videoPacketDecoderWorker.UnhandledException += UnhandledException;

            wmsForm = new WMS.MainForm();
            wmsForm.Show();
        }

        private void UnhandledException(object sender, Exception exception)
        {
            MessageBox.Show(exception.ToString(), "Unhandled Exception (Ctrl+C)", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /*
         * Show if current process is 64-bits or 32-bits (important for ffmpeg dll's)
         */
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Text += Environment.Is64BitProcess ? " [64-bit]" : " [32-bit]";
        }

        protected override void OnClosed(EventArgs e)
        {
            _droneController.stopAutopilot();
            _droneController.stopClient();
            _droneController.dispose();
            _videoPacketDecoderWorker.Dispose();
            base.OnClosed(e);
        }

        private void OnNavigationDataAcquired(NavigationData data)
        {
            _navigationData = data;
        }

        private void OnVideoPacketAcquired(VideoPacket packet)
        {
            if (_videoPacketDecoderWorker.IsAlive)
            {
                _videoPacketDecoderWorker.EnqueuePacket(packet);
            } 
        }

        private void OnVideoPacketDecoded(VideoFrame frame)
        {
            _frame = frame;
        }

        //If there is a new frame available, create bitmap of said frame and update display
        private void tmrVideoUpdate_Tick(object sender, EventArgs e)
        {
            //Check if frame(number) has changed, if not: do not update
            if (_frame == null || _frameNumber == _frame.Number)
            {
                return;
            }

            _frameNumber = _frame.Number;

            if (_frameBitmap == null)
            {
               _frameBitmap = VideoHelper.CreateBitmap(ref _frame); 
            }
            else
            {
                VideoHelper.UpdateBitmap(ref _frameBitmap, ref _frame);
            }
            pbVideo.Image = _frameBitmap;

            _droneController.videoUpdateTick();
        }

        //Periodic update of displayed navigational data
        private void tmrStateUpdate_Tick(object sender, EventArgs e)
        {
            //Navigational data can be null if there is no established connection to the drone
            if (_navigationData != null)
            {
                //Updating the form with new navigation data
                lblNavigationState.Text = _navigationData.State.ToString();
                lblYaw.Text = _navigationData.Yaw.ToString();
                lblPitch.Text = _navigationData.Pitch.ToString();
                lblRoll.Text = _navigationData.Roll.ToString();
                lblAltitude.Text = _navigationData.Altitude.ToString();
                lblVelocityZ.Text = "Z: " + _navigationData.Velocity.Z.ToString();
                lblVelocityY.Text = "Y: " + _navigationData.Velocity.Y.ToString();
                lblVelocityX.Text = "X: " + _navigationData.Velocity.X.ToString();
                lblBattery.Text = _navigationData.Battery.Percentage.ToString() + "% " + _navigationData.Battery.Voltage;
                lblTime.Text = _navigationData.Time.ToString();
                lblWifi.Text = _navigationData.Wifi.LinkQuality.ToString();
            }
        }

        public Bitmap getFrame()
        {
            return _frameBitmap;
        }

        private void btnEmergency_Click(object sender, EventArgs e)
        {
            _droneController.emergency();
            _droneController.stopAutopilot();
        }

        private async void btnCycleCount_Click(object sender, EventArgs e)
        {
            await _droneController.CycleCount();
        }

        private async void btnSmartScan_Click(object sender, EventArgs e)
        {
            await _droneController.SmartScan();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (_droneController.startClient())
            {
                lblConnectionStatus.Text = "Connected";
                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;
                btnEmergency.Enabled = true;
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            _droneController.stopClient();
            lblConnectionStatus.Text = "Disconnected";
            btnConnect.Enabled = true;
            btnDisconnect.Enabled = false;
        }

        private void btnLand_Click(object sender, EventArgs e)
        {
            _droneController.stopAutopilot();
            _droneController.land();
        }
    }
}

