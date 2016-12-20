using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math.Geometry;
using AR.Drone.Client;
using AR.Drone.Data;
using AR.Drone.Data.Navigation;
using AR.Drone.Video;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using ZXing;
using WMS;
using RoutePlanner;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneControl
{
    public partial class MainForm : Form
    {
        /*
         * Fields
         */
        private DroneController _droneController;
        private DroneClient _droneClient;
        private readonly VideoPacketDecoderWorker _videoPacketDecoderWorker;
        private VideoFrame _frame;
        private Bitmap _frameBitmap;
        private uint _frameNumber;
        private NavigationData _navigationData;
        private WMS.MainForm wmsForm;
        private bool checkVoorVormen = false;

        /*
         * Constructor: creating the form and creating the droneclient.
         */
        public MainForm()
        {
            InitializeComponent();

            //Start videopacketdecoder worker
            _videoPacketDecoderWorker = new VideoPacketDecoderWorker(AR.Drone.Video.PixelFormat.BGR24, true, OnVideoPacketDecoded);
            _videoPacketDecoderWorker.Start();

            //Create a droneclient and attach event handlers
            _droneController = new DroneController(this);
            _droneController.attachEventHandlers(OnVideoPacketAcquired, OnNavigationDataAcquired);

            //Start timers
            tmrStateUpdate.Enabled = true;
            tmrVideoUpdate.Enabled = true;

            //Attach exceptionhandler to the videopacketdecoder worker.
            _videoPacketDecoderWorker.UnhandledException += UnhandledException;

            //Create instance of WMS.
            wmsForm = new WMS.MainForm();
            wmsForm.Show();
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
            _droneController.stopAutopilot();
            _droneController.stopClient();
            _droneController.Dispose();
            _videoPacketDecoderWorker.Dispose();
            base.OnClosed(e);
        }

        /*
         * Event: Storing received navigation data.
         */
        private void OnNavigationDataAcquired(NavigationData data)
        {
            _navigationData = data;
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
                lblBattery.Text = _navigationData.Battery.Percentage.ToString() + "% " + _navigationData.Battery.Voltage;
                lblTime.Text = _navigationData.Time.ToString();
                lblWifi.Text = _navigationData.Wifi.LinkQuality.ToString();
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            _droneController.startClient();
            lblConnectionStatus.Text = "Connected";
            btnConnect.Enabled = false;
            btnDisconnect.Enabled = true;
            btnAutopilotGo.Enabled = true;
            btnAutopilotStop.Enabled = true;
            _droneClient = _droneController.getDroneClient();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            _droneController.stopClient();
            lblConnectionStatus.Text = "Disconnected";
            btnConnect.Enabled = true;
            btnDisconnect.Enabled = false;
        }

        /*
         * Button Click: Scan current stored frame for barcodes.
         */
        private void btnScanForBarcode_Click(object sender, EventArgs e)
        {
            string result = scanBarcode();
            barcode.Text = result;
        }

        private void btnAutopilotGo_Click(object sender, EventArgs e)
        {
            //Commands invoegen
            //Start autopilot
            //_droneController.enqueueTest();
            _droneController.startAutopilot();
            btnAutopilotGo.Enabled = false;
        }


        /*
         * These buttons are used in manually controlling the drone. This functionality will not be in the release.
         */
        private void btnEmergency_Click(object sender, EventArgs e)
        {
            _droneController.emergency();
        }

        private void btnAutopilotStop_Click(object sender, EventArgs e)
        {
            _droneController.startAutopilot();
            btnAutopilotGo.Enabled = true;
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            _droneClient.Progress(AR.Drone.Client.Command.FlightMode.Progressive, pitch: -0.05f);
        }

        private void btnBackward_Click(object sender, EventArgs e)
        {
            _droneClient.Progress(AR.Drone.Client.Command.FlightMode.Progressive, pitch: 0.05f);
        }

        private void btnHover_Click(object sender, EventArgs e)
        {
            _droneClient.Progress(AR.Drone.Client.Command.FlightMode.Hover, 0, 0, 0, 0);
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            _droneClient.Progress(AR.Drone.Client.Command.FlightMode.Progressive, roll: -0.05f);
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            _droneClient.Progress(AR.Drone.Client.Command.FlightMode.Progressive, roll: 0.05f);
        }

        private void btnRotateLeft_Click(object sender, EventArgs e)
        {
            _droneClient.Progress(AR.Drone.Client.Command.FlightMode.Hover, yaw: -0.2f);
        }

        private void btnRotateRight_Click(object sender, EventArgs e)
        {
            _droneClient.Progress(AR.Drone.Client.Command.FlightMode.Hover, yaw: 0.2f);
        }

        private void btnTakeoff_Click(object sender, EventArgs e)
        {
            _droneClient.Takeoff();
        }

        private void btnLand_Click(object sender, EventArgs e)
        {
            _droneClient.Land();
        }

        private void btnFlatTrim_Click(object sender, EventArgs e)
        {
            _droneClient.FlatTrim();
        }
        private string scanBarcode()
        {
            // create a barcode reader instance
            IBarcodeReader reader = new BarcodeReader();
            // detect and decode the barcode inside the bitmap
            var result = reader.Decode(_frameBitmap);
            // return result or error
            if (result != null)
            {
                return result.Text;
            }
            else
            {
                return "Geen Barcode Gevonden";
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            _droneController.doSmartScan();
        }

        private void btnMockScan_Click(object sender, EventArgs e)
        {
            int scannedId = (int)nudMockScan.Value;
            wmsForm.productScanned(scannedId);
        }

        private void btnMockDroneScanning_Click(object sender, EventArgs e)
        {
            wmsForm.showMutations();
        }


        private void btnCycleCount_Click(object sender, EventArgs e)
        {
            _droneController.CycleCount();
        }

        private void CheckVormen_Click(object sender, EventArgs e)
        {
            checkVoorVormen = false;
            _droneController.ScanVormen(_frameBitmap);
        }
            

        public System.Drawing.Point[] ToPointsArray(List<IntPoint> points)
        {
            System.Drawing.Point[] array = new System.Drawing.Point[points.Count];

            for (int i = 0, n = points.Count; i < n; i++)
            {
                array[i] = new System.Drawing.Point(points[i].X, points[i].Y);
            }

            return array;
        }

        

        private void btnSmartScan_Click(object sender, EventArgs e)
        {
            _droneController.doSmartScan();
        }
    }
}

