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
        public WMS.MainForm wmsForm { get; set; }
        public int hasToCalibrate { get; set; }
        public bool scanningForBarcode { get; set; }
        public bool scanningForLine { get; set; }
        public bool lineFound { get; set; }
        public bool scanningForAngle { get; set; }


        /*
         * Constructor: creating the form and creating the droneclient.
         */
        public MainForm()
        {
            
            InitializeComponent();

            lineFound = false;

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
            if (scanningForBarcode)
            {
              _droneController.scanForBarcode();
            }

            if (scanningForLine) {
               _droneController.zoekLijn();
            }

            if (scanningForAngle)
            {
                _droneController.detectLine();
            }
            
            
            
            if(hasToCalibrate == 0)
            {
              checkAfwijking(_frameBitmap);
            }

            
            //moet vooruit vliegen om te calibreren
            if (hasToCalibrate == 1 && scanningForLine)
            {
                _droneController.droneCalibrationDirection = 1;
            }

            //moet achteruit vliegen om te calibreren
            if (hasToCalibrate == -1 && scanningForLine)
            {
                _droneController.droneCalibrationDirection = -1;
            }
        
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
            btnEmergency.Enabled = true;
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
        public Bitmap getFrame()
        {
            return _frameBitmap;
        }

        /*
         * These buttons are used in manually controlling the drone. This functionality will not be in the release.
         */
        private void btnEmergency_Click(object sender, EventArgs e)
        {
            //_droneController.emergency();
            _droneController.stopAutopilot();
            _droneClient.Land();
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

        private void btnCycleCount_Click(object sender, EventArgs e)
        {
            _droneController.CycleCount();
        }

        

        private void btnSmartScan_Click(object sender, EventArgs e)
        {
            _droneController.startSmartScan();
        }

        private void checkAfwijking(Bitmap frame)
        {
            Bitmap myBitmap = frame;

            // lock image
            BitmapData bitmapData = myBitmap.LockBits(
                new Rectangle(0, 0, myBitmap.Width, myBitmap.Height),
                ImageLockMode.ReadWrite, myBitmap.PixelFormat);

            // step 1 - turn background to black
            ColorFiltering colorFilter = new ColorFiltering();

            colorFilter.Red = new IntRange(0, 64);
            colorFilter.Green = new IntRange(0, 64);
            colorFilter.Blue = new IntRange(0, 64);
            colorFilter.FillOutsideRange = false;

            colorFilter.ApplyInPlace(bitmapData);


            // step 2 - locating objects
            BlobCounter blobCounter = new BlobCounter();

            blobCounter.FilterBlobs = true;
            blobCounter.MinHeight = 5;
            blobCounter.MinWidth = 5;

            blobCounter.ProcessImage(bitmapData);
            Blob[] blobs = blobCounter.GetObjectsInformation();
            myBitmap.UnlockBits(bitmapData);

            // step 3 - check objects' type and highlight
            SimpleShapeChecker shapeChecker = new SimpleShapeChecker();

            Graphics g = Graphics.FromImage(myBitmap);

            // check each object and draw triangle around objects, which
            // are recognized as triangles
            for (int i = 0, n = blobs.Length; i < n; i++)
            {
                List<IntPoint> edgePoints = blobCounter.GetBlobsEdgePoints(blobs[i]);
                List<IntPoint> corners;

                if (shapeChecker.IsQuadrilateral(edgePoints, out corners))
                {
                    foreach (IntPoint corner in corners)
                    {
                        if (corner.Y >= pbVideo.Height-40)
                        {
                            hasToCalibrate = 1;
                        }
                        else if (corner.Y <= 40)
                        {
                            hasToCalibrate = -1;
                        }
                    }
                }
            }
            g.Dispose();
        }
        
        private void gbVideoFeed_Enter(object sender, EventArgs e)
        {
        
        }

        private void pbVideo_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            _droneController.stopAutopilot();
            _droneClient.Land();
        }
    }
}

