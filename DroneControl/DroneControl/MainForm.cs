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
        /*
         * Fields
         */
        private readonly DroneClient _droneClient;
        private readonly VideoPacketDecoderWorker _videoPacketDecoderWorker;
        private VideoFrame _frame;
        private Bitmap _frameBitmap;
        private uint _frameNumber;
        private NavigationData _navigationData;
        private NavigationPacket _navigationPacket;

        /*
         * Constructor: creating the form and creating the droneclient.
         */
        public MainForm()
        {
            InitializeComponent();

            //Start videopacketdecoder worker
            _videoPacketDecoderWorker = new VideoPacketDecoderWorker(PixelFormat.BGR24, true, OnVideoPacketDecoded);
            _videoPacketDecoderWorker.Start();

            //Create a droneclient and attach event handlers
            _droneClient = new DroneClient("192.168.1.1");
            _droneClient.NavigationPacketAcquired += OnNavigationPacketAcquired;
            _droneClient.VideoPacketAcquired += OnVideoPacketAcquired;
            _droneClient.NavigationDataAcquired += data => _navigationData = data;

            //Start timers
            tmrStateUpdate.Enabled = true;
            tmrVideoUpdate.Enabled = true;

            //Attach exceptionhandler to the videopacketdecoder worker.
            _videoPacketDecoderWorker.UnhandledException += UnhandledException;
        }

        /*
         * Basic exception handler
         */
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

        /*
         * Exiting the program, cleaning up.
         */
        protected override void OnClosed(EventArgs e)
        {
            _droneClient.Dispose();
            _videoPacketDecoderWorker.Dispose();
            base.OnClosed(e);
        }

        /*
         * Event: Storing received navigation packet.
         */
        private void OnNavigationPacketAcquired(NavigationPacket packet)
        {
            _navigationPacket = packet;
        }

        /*
         * Event: Enqueue a video packet for the decoding process.
         */
        private void OnVideoPacketAcquired(VideoPacket packet)
        {
            if (_videoPacketDecoderWorker.IsAlive)
            {
                _videoPacketDecoderWorker.EnqueuePacket(packet);
            } 
        }

        /*
         * Event: A video packet has been decoded and will be stored.
         */
        private void OnVideoPacketDecoded(VideoFrame frame)
        {
            _frame = frame;
        }

        /*
         * Timer Tick Event: If there is a new frame available, create bitmap of said frame and update display.
         */
        private void tmrVideoUpdate_Tick(object sender, EventArgs e)
        {
            //Check if frame(number) has changed, if not: do not update.
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
        }

        /*
         * Timer Tick Event: Periodic update of displayed navigational data.
         */
        private void tmrStateUpdate_Tick(object sender, EventArgs e)
        {
            //Navigational data can be null if there is no established connection to the drone
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

        /*
         * Button Click: Start the droneclient and connect to the drone.
         */
        private void btnConnect_Click(object sender, EventArgs e)
        {
            _droneClient.Start();
            lblConnectionStatus.Text = "Connected";
            btnConnect.Enabled = false;
            btnDisconnect.Enabled = true;
        }

        /*
         * Button Click: Stop the droneclient and disconnect the drone.
         */
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            _droneClient.Stop();
            lblConnectionStatus.Text = "Disconnected";
            btnConnect.Enabled = true;
            btnDisconnect.Enabled = false;
        }

        /*
         * Button Click: Scan current stored frame for barcodes.
         */
        private void btnScanForBarcode_Click(object sender, EventArgs e)
        {
            //TODO: Use barcode scanning software here!
        }
    }
}
