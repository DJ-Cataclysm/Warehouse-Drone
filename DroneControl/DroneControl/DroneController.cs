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

namespace DroneControl
{
    public class DroneController
    {
        AutopilotController autopilotController;
        DroneClient droneClient;
        RouteInterpreter routeInterpreter;
        TaskCompletionSource<bool> flyTaskCompleted, cameraSwitched;
        MainForm mainForm;
        public BarcodeScanning barcodeScanning;
        public LineDetection2 angleDetection;
        Settings settings;
        public int droneCalibrationDirection { set; get; }
        public int turnDegrees { set; get; }
        public bool scanningForBarcode, scanningForLine, scanningForAngle;
        const int MAX_TURN_DEVIATION = 10; //if drone is off by more than MAX_TURN_DEVIATION, do corrective turn.
        const double PRODUCT_DEVIATION_THRESHOLD = 0; //Products exceeding the product deviation threshold will be smartscanned.

        public DroneController(MainForm form)
        {
            //The IP-address is always the default gateway when connected to the drone WiFi.
            droneClient = new DroneClient("192.168.1.1"); 
            autopilotController = new AutopilotController(droneClient, this);
            routeInterpreter = new RouteInterpreter(ref autopilotController);
            angleDetection = new LineDetection2(form, this);
            barcodeScanning = new BarcodeScanning(form, this);
            mainForm = form;
        }

        //Method called each time the MainForm.tmrVideoUpdate ticks
        public void videoUpdateTick()
        {
            if (scanningForBarcode)
            {
                barcodeScanning.scanForBarcode();
            }

            if (scanningForAngle)
            {
                angleDetection.detectLine();
            }
        }
       
        public async Task SmartScan()
        {
            //Initial setup, starting the drone, finding the line & calibrating
            flyTaskCompleted = new TaskCompletionSource<bool>();
            List<Position> route = makeSmartScanRoute();

            autopilotController.Start();
            routeInterpreter.takeOffCommand.execute();
            routeInterpreter.shortHover.execute();

            await flyTaskCompleted.Task;
            await findLine();
            await turnCalibration();

            //Loop through the made route
            for (int i = 0; i < route.Count()-1; i++ )
            {
                //Fly to the next position
                flyTaskCompleted = new TaskCompletionSource<bool>();
                Position current = route[i];
                Position target = route[i+1];
                routeInterpreter.flyToCoordinate(current, target);
                await flyTaskCompleted.Task;

                //Calibrate if the target position is a smartscan item & scan the item
                if (target.isTargetPosition)
                {
                    await findLine();
                    await turnCalibration();
                    await searchForBarcode(current);
                }
            }

            //Land after the route is done
            routeInterpreter.landCommand.execute();
            mainForm.wmsForm.showMutations();
        }

        public async Task CycleCount()
        {
            //Initial setup, starting the drone, finding the line & calibrating
            flyTaskCompleted = new TaskCompletionSource<bool>();
            List<Position> route = makeCycleCountRoute();
            autopilotController.Start(); 
            routeInterpreter.takeOffCommand.execute();
            routeInterpreter.shortHover.execute();

            await flyTaskCompleted.Task;
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
                    flyTaskCompleted = new TaskCompletionSource<bool>();
                    routeInterpreter.flyToCoordinate(current, target);
                    await flyTaskCompleted.Task;
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
            routeInterpreter.landCommand.execute();
            mainForm.wmsForm.showMutations();
        }

        //Function for flying to another rack
        private async Task flyToZ(Position currentPos, Position targetPos)
        {
            //Turn 180 degrees
            routeInterpreter.turn.execute(180);

            //Start flying to the other rack and find the line
            routeInterpreter.flyToOtherRack.execute();
            await Task.Delay(2000);
            await findLine();
            await turnCalibration();

            //Fly to coordinate
            Position newCurrentPos = currentPos;

            //Mirroring of x coordinate, limited to x values between 0 and 8.
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
     
            flyTaskCompleted = new TaskCompletionSource<bool>();
            routeInterpreter.flyToCoordinate(newCurrentPos, targetPos);
            await flyTaskCompleted.Task;
        }

        //Function for calibrating the drone by searching the barcode 
        private async Task searchForBarcode(Position i)
        {
            //Enables front camera, enables the scanning for barcodes
            flyTaskCompleted = new TaskCompletionSource<bool>();
            await switchCamera(VideoChannelType.Horizontal);
            routeInterpreter.shortHover.execute();

            //At each new videoframe it will scan for a barcode.
            scanningForBarcode = true;
            
            //It will slowly strafe left and right in order to find it.
            routeInterpreter.barcodeSmallLeft.execute(1000);
            routeInterpreter.shortHover.execute();
            routeInterpreter.barcodeSmallRight.execute(2000);
            routeInterpreter.shortHover.execute();
            routeInterpreter.barcodeSmallLeft.execute(1000);
            routeInterpreter.shortHover.execute();
            await flyTaskCompleted.Task;

            //It's possible the barcode hasn't been found, continue anyway.
            scanningForBarcode = false;
        }

        //function for finding the line. used for front and back calibration
        public async Task findLine()
        {
            //enables scanning for the line, switch to bottom camera
            await switchCamera(VideoChannelType.Vertical);
            scanningForLine = true;

            //fly forwards and backwards to find the line. Stops if the line is found
            flyTaskCompleted = new TaskCompletionSource<bool>();
            await Task.Delay(300);
            routeInterpreter.shortHover.execute();
            routeInterpreter.goForwardCalibration.execute();
            routeInterpreter.shortHover.execute();
            routeInterpreter.goBackwardsCalibration.execute();
            routeInterpreter.shortHover.execute();
            routeInterpreter.goBackwardsCalibration.execute();
            routeInterpreter.shortHover.execute();
            routeInterpreter.goForwardCalibration.execute();
            routeInterpreter.shortHover.execute();
            await flyTaskCompleted.Task;
            scanningForLine = false;
        }

        //function used to manually set the flytask to completed.
        public void setFlyTaskCompleted()
        {
            flyTaskCompleted.TrySetResult(true);
        }

        //function used for calibrating the angle of the drone using the line on the ground
        public async Task turnCalibration()
        {
            scanningForAngle = true; //enables scanning for the angle
            routeInterpreter.shortHover.execute();
            await Task.Delay(500);
            flyTaskCompleted = new TaskCompletionSource<bool>();
            //If turnDegrees is too large it needs to turn
            if (turnDegrees < -MAX_TURN_DEVIATION || turnDegrees > MAX_TURN_DEVIATION)
            {
               routeInterpreter.turn.execute(turnDegrees);
            }
            await flyTaskCompleted.Task;
            scanningForAngle = false;

            //Always look at front camera after turning
            await switchCamera(VideoChannelType.Horizontal);
        }
       
        //function that stops all current tasks. Used for clearing the drone's tasks if it's calibrated.
        public void stopCurrentTasks(){
            autopilotController.clearObjectives();
            routeInterpreter.shortHover.execute();
        }

        //function used to switch to the bottom camera(for line detection) or front camera(barcode detection)
        public async Task switchCamera(VideoChannelType videoChannelType)
        {
            //Send basic configuration, while also changing the video channel
            TaskCompletionSource<bool> cameraSwitched = new TaskCompletionSource<bool>();
            if(settings == null)
            {
                settings = new Settings();
            }
            var sendConfigTask = new Task(() =>
            {
                settings.Video.Channel = videoChannelType;
                droneClient.Send(settings);
            });
            sendConfigTask.Start();
            await Task.Delay(500); //Wait for config to be read by drone
        }
        
        public DroneClient getDroneClient()
        {
            return droneClient;
        }

        public void emergency()
        {
            droneClient.Emergency();
        }

        public void land()
        {
            droneClient.Land();
        }

        public void startClient()
        {
            droneClient.Start();

            //Send starting configuration
            settings = new Settings();
            var sendConfigTask = new Task(() =>
            {

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
                    
                    settings.Video.Channel = VideoChannelType.Vertical;
                    droneClient.Send(settings);

                    droneClient.AckControlAndWaitForConfirmation();
                }

                settings.General.NavdataDemo = false;
                settings.General.NavdataOptions = NavdataOptions.All;

                settings.Video.BitrateCtrlMode = VideoBitrateControlMode.Dynamic;
                settings.Video.Bitrate = 1000;
                settings.Video.MaxBitrate = 2000;

                droneClient.Send(settings);
            });
            sendConfigTask.Start();
        }

        public void stopClient()
        {
            droneClient.Stop();
        }

        public void stopAutopilot()
        {
            autopilotController.Stop();
        }

        public void attachEventHandlers(Action<VideoPacket> videoPacketHandler, Action<NavigationData> navDataHandler)
        {
            droneClient.VideoPacketAcquired += videoPacketHandler;
            droneClient.NavigationDataAcquired += navDataHandler;
        }

        public void dispose()
        {
            droneClient.Dispose();
        }

        //function that makes a full cycle count route using the RoutePlan class
        private List<Position> makeCycleCountRoute()
        {
            /*
             * Find all products, create a position per product and then plot a route between those positions.
             */
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

        //function that makes a smart scan route using the RoutePlan class
        private List<Position> makeSmartScanRoute()
        {
            /*
             * Find all products exceeding the deviation threshhold and plot a route between those products.
             */
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
