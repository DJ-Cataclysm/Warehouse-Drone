using AR.Drone.Client;
using AR.Drone.Client.Configuration;
using AR.Drone.Data;
using AR.Drone.Data.Navigation;
using RoutePlanner;
using System;
using System.Collections.Generic;
using System.Linq;
using WMS;
using System.Threading.Tasks;
using System.Drawing;
using ZXing;

namespace DroneControl
{
    public class DroneController
    {
        AutopilotController _autopilotController;
        DroneClient _droneClient;
        RouteInterpreter _routeInterpreter;
        TaskCompletionSource<bool> _flyTaskCompleted;
        MainForm _mainForm;
        LineDetection _lineDetection;
        IBarcodeReader _barcodeReader;
        Settings _settings;
        int _turnDegrees;
        bool _scanningForBarcode, _scanningForLine;
        const int MAX_TURN_DEVIATION = 10; //if drone is off by more than MAX_TURN_DEVIATION, do corrective turn
        const double PRODUCT_DEVIATION_THRESHOLD = 0; //Products exceeding the product deviation threshold will be smartscanned
        const int VIDEO_BITRATE = 1000;
        const int VIDEO_MAX_BITRATE = 2000;

        public DroneController(MainForm form)
        {
            //The IP-address is always the default gateway when connected to the drone WiFi
            _droneClient = new DroneClient("192.168.1.1"); 
            _autopilotController = new AutopilotController(_droneClient, this);
            _routeInterpreter = new RouteInterpreter(ref _autopilotController);
            _lineDetection = new LineDetection();
            _barcodeReader = new BarcodeReader();
            _mainForm = form;
            
        }

        //Method called each time the MainForm.tmrVideoUpdate ticks
        public void videoUpdateTick()
        {
            Bitmap videoFrame = _mainForm.getFrame();

            if (_scanningForBarcode)
            {
                scanForBarcode(videoFrame);
            }

            if (_scanningForLine)
            {
                _turnDegrees = _lineDetection.detectLine(videoFrame);
                if (_turnDegrees != 0)
                {
                    _scanningForLine = false;
                    stopCurrentTasks();
                }
            }
        }

        /*
         * Function that scans for the barcode, and if the barcode is found, 
         * send it to the WMS. Called from Mainform frame update.
         */
        private void scanForBarcode(Bitmap frame)
        {
            var result = _barcodeReader.Decode(frame);

            if (result != null)
            {
                int id = int.MaxValue;
                int.TryParse(result.Text, out id);
                _mainForm.wmsForm.productScanned(id);
                _scanningForBarcode = false;
                stopCurrentTasks();
            }
        }
       
        public async Task SmartScan()
        {
            //Initial setup, starting the drone, finding the line & calibrating
            _flyTaskCompleted = new TaskCompletionSource<bool>();
            List<Position> route = makeSmartScanRoute();

            _autopilotController.start();
            _routeInterpreter.takeOffCommand.execute();
            _routeInterpreter.shortHover.execute();

            await _flyTaskCompleted.Task;
            await findLine();
            await turnCalibration();

            //Loop through the made route
            for (int i = 0; i < route.Count()-1; i++ )
            {
                //Fly to the next position
                _flyTaskCompleted = new TaskCompletionSource<bool>();
                Position current = route[i];
                Position target = route[i+1];
                _routeInterpreter.flyToCoordinate(current, target);
                await _flyTaskCompleted.Task;

                //Calibrate if the target position is a smartscan item & scan the item
                if (target.isTargetPosition)
                {
                    await findLine();
                    await turnCalibration();
                    await searchForBarcode(current);
                }
            }

            //Land after the route is done
            _routeInterpreter.landCommand.execute();
            _mainForm.wmsForm.showMutations();
        }

        public async Task CycleCount()
        {
            //Initial setup, starting the drone, finding the line & calibrating
            _flyTaskCompleted = new TaskCompletionSource<bool>();
            List<Position> route = makeCycleCountRoute();
            _autopilotController.start(); 
            _routeInterpreter.takeOffCommand.execute();
            _routeInterpreter.shortHover.execute();

            await _flyTaskCompleted.Task;
            await findLine();
            await turnCalibration();

            //Loop through the route
            for (int i = 0; i < route.Count - 1; i++ )
            {
                Position current = route[i];
                Position target = route[i+1];

                //Fly to the next position (uses flyToZ if the position is on another rack)
                if (current.z == target.z)
                {
                    _flyTaskCompleted = new TaskCompletionSource<bool>();
                    _routeInterpreter.flyToCoordinate(current, target);
                    await _flyTaskCompleted.Task;
                }
                else
                {
                    await flyToZ(current, target);
                }

                //Calibration and finding barcode
                await turnCalibration();
                await searchForBarcode(current);

            }
            //Land after the route is done
            _routeInterpreter.landCommand.execute();
            _mainForm.wmsForm.showMutations();
        }

        //Function for flying to another rack
        private async Task flyToZ(Position currentPos, Position targetPos)
        {
            //Turn 180 degrees (half turn)
            _routeInterpreter.turn.execute(180);

            //Start flying to the other rack and find the line
            _routeInterpreter.flyToOtherRack.execute();
            await Task.Delay(2000);
            await findLine();
            await turnCalibration();

            //Fly to coordinate
            Position newCurrentPos = currentPos;

            //Mirroring of x coordinate, limited to x values between 0 and 8
            switch (currentPos.x)
            {
                case 0:
                    newCurrentPos.x = 8;
                    break;
                case 1:
                    newCurrentPos.x = 7;
                    break;
                case 2:
                    newCurrentPos.x = 6;
                    break;
                case 3:
                    newCurrentPos.x = 5;
                    break;
                case 5:
                    newCurrentPos.x = 3;
                    break;
                case 6:
                    newCurrentPos.x = 2;
                    break;
                case 7:
                    newCurrentPos.x = 1;
                    break;
                case 8:
                    newCurrentPos.x = 0;
                    break;
            }
     
            _flyTaskCompleted = new TaskCompletionSource<bool>();
            _routeInterpreter.flyToCoordinate(newCurrentPos, targetPos);
            await _flyTaskCompleted.Task;
        }

        //Function for calibrating the drone by searching the barcode 
        private async Task searchForBarcode(Position i)
        {
            //Enables front camera, enables the scanning for barcodes
            _flyTaskCompleted = new TaskCompletionSource<bool>();
            await switchCamera(VideoChannelType.Horizontal);
            _routeInterpreter.shortHover.execute();

            //At each new videoframe it will scan for a barcode
            _scanningForBarcode = true;
            
            //It will slowly strafe left and right in order to find it
            _routeInterpreter.barcodeSmallLeft.execute(1000);
            _routeInterpreter.shortHover.execute();
            _routeInterpreter.barcodeSmallRight.execute(2000);
            _routeInterpreter.shortHover.execute();
            _routeInterpreter.barcodeSmallLeft.execute(1000);
            _routeInterpreter.shortHover.execute();
            await _flyTaskCompleted.Task;

            //It's possible the barcode hasn't been found, continue anyway
            _scanningForBarcode = false;
        }

        //Function for finding the line. used for front and back calibration
        public async Task findLine()
        {
            //Enables scanning for the line, switch to bottom camera
            await switchCamera(VideoChannelType.Vertical);

            //Fly forwards and backwards to find the line. Stops if the line is found
            _flyTaskCompleted = new TaskCompletionSource<bool>();
            await Task.Delay(300);
            _routeInterpreter.shortHover.execute();
            _routeInterpreter.goForwardCalibration.execute();
            _routeInterpreter.shortHover.execute();
            _routeInterpreter.goBackwardsCalibration.execute();
            _routeInterpreter.shortHover.execute();
            _routeInterpreter.goBackwardsCalibration.execute();
            _routeInterpreter.shortHover.execute();
            _routeInterpreter.goForwardCalibration.execute();
            _routeInterpreter.shortHover.execute();
            await _flyTaskCompleted.Task;
        }

        //Function used to manually set the flytask to completed.
        public void setFlyTaskCompleted()
        {
            _flyTaskCompleted.TrySetResult(true);
        }

        //function used for calibrating the angle of the drone using the line on the ground
        public async Task turnCalibration()
        {
            _scanningForLine = true; //enables scanning for the angle
            _routeInterpreter.shortHover.execute();
            await Task.Delay(500);
            _flyTaskCompleted = new TaskCompletionSource<bool>();
            //If turnDegrees is too large it needs to turn
            if (_turnDegrees < -MAX_TURN_DEVIATION || _turnDegrees > MAX_TURN_DEVIATION)
            {
               _routeInterpreter.turn.execute(_turnDegrees);
            }
            await _flyTaskCompleted.Task;
            _scanningForLine = false;

            //Always look at front camera after turning
            await switchCamera(VideoChannelType.Horizontal);
        }
       
        //function that stops all current tasks. Used for clearing the drone's tasks if it's calibrated.
        public void stopCurrentTasks(){
            _autopilotController.clearObjectives();
            _routeInterpreter.shortHover.execute();
        }

        //function used to switch to the bottom camera(for line detection) or front camera(barcode detection)
        public async Task switchCamera(VideoChannelType videoChannelType)
        {
            //Send basic configuration, while also changing the video channel
            TaskCompletionSource<bool> cameraSwitched = new TaskCompletionSource<bool>();
            if(_settings == null)
            {
                _settings = new Settings();
            }
            var sendConfigTask = new Task(() =>
            {
                _settings.Video.Channel = videoChannelType;
                _droneClient.Send(_settings);
            });
            sendConfigTask.Start();
            await Task.Delay(500); //Wait for config to be read by drone
        }

        public void emergency()
        {
            _droneClient.Emergency();
        }

        public void land()
        {
            _droneClient.Land();
        }

        public bool startClient()
        {
            _droneClient.Start();

            if (!_droneClient.IsConnected)
            {
                return false;
            }

            //Send starting configuration
            _settings = new Settings();
            var sendConfigTask = new Task(() =>
            {

                if (string.IsNullOrEmpty(_settings.Custom.SessionId) ||
                    _settings.Custom.SessionId == "00000000")
                {
                    // set new session, application and profile
                    _droneClient.AckControlAndWaitForConfirmation(); // wait for the control confirmation

                    _settings.Custom.SessionId = Settings.NewId();
                    _droneClient.Send(_settings);

                    _droneClient.AckControlAndWaitForConfirmation();

                    _settings.Custom.ProfileId = Settings.NewId();
                    _droneClient.Send(_settings);

                    _droneClient.AckControlAndWaitForConfirmation();

                    _settings.Custom.ApplicationId = Settings.NewId();
                    _droneClient.Send(_settings);

                    _droneClient.AckControlAndWaitForConfirmation();
                    
                    _settings.Video.Channel = VideoChannelType.Vertical;
                    _droneClient.Send(_settings);

                    _droneClient.AckControlAndWaitForConfirmation();
                }

                _settings.General.NavdataDemo = false;
                _settings.General.NavdataOptions = NavdataOptions.All;

                //Video quality settings
                _settings.Video.BitrateCtrlMode = VideoBitrateControlMode.Dynamic;
                _settings.Video.Bitrate = VIDEO_BITRATE;
                _settings.Video.MaxBitrate = VIDEO_MAX_BITRATE;

                _droneClient.Send(_settings);
            });
            sendConfigTask.Start();

            return true;
        }

        public void stopClient()
        {
            _droneClient.Stop();
        }

        public void stopAutopilot()
        {
            _autopilotController.stop();
        }

        public void attachEventHandlers(Action<VideoPacket> videoPacketHandler, Action<NavigationData> navDataHandler)
        {
            _droneClient.VideoPacketAcquired += videoPacketHandler;
            _droneClient.NavigationDataAcquired += navDataHandler;
        }

        public void dispose()
        {
            _droneClient.Dispose();
        }

        //function that makes a full cycle count route using the RoutePlan class
        private List<Position> makeCycleCountRoute()
        {
            //Find all products, create a position per product and then plot a route between those positions.
            List<Product> products = new List<Product>();

            using (ProductDBContext db = new ProductDBContext())
            {
                products = db.Products.ToList();
            }

            List<Position> itemsToCheck = new List<Position>();
            foreach (Product p in products)
            {
                itemsToCheck.Add(new Position(p.X, p.Y, p.Z));
            }
            //Make full cycle route using the positions in the static Positions class
            return RoutePlan.makeCycleCountRoute(itemsToCheck);
        }

        private List<Position> makeSmartScanRoute()
        {
            //Find all products exceeding the deviation threshhold and plot a route between those products.
            List<Product> products = new List<Product>();

            using (ProductDBContext db = new ProductDBContext())
            {
                products = db.Products.Where(p => p.Deviation > PRODUCT_DEVIATION_THRESHOLD).ToList();
            }

            List<Position> itemsToCheck = new List<Position>();
            foreach(Product p in products)
            {
                itemsToCheck.Add(new Position(p.X, p.Y, p.Z, true));
            }

            return RoutePlan.makeSmartScanRoute(itemsToCheck);
        }
    }
}
