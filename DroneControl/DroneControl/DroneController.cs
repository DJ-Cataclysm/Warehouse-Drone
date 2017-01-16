﻿using AR.Drone.Client;
using AR.Drone.Client.Configuration;
using AR.Drone.Data;
using AR.Drone.Data.Navigation;
using RoutePlanner;
using System;
using System.Collections.Generic;
using System.Linq;
using WMS;
using System.Threading.Tasks;
using ZXing;
using System.Drawing;
using System.Drawing.Imaging;
using AForge.Imaging.Filters;
using AForge;
using AForge.Imaging;
using AForge.Math.Geometry; 

namespace DroneControl
{
    public class DroneController
    {
        AutopilotController autopilotController;
        DroneClient droneClient;
        RouteInterpreter routeInterpreter;
        TaskCompletionSource<bool> flyTaskCompleted;
        MainForm mainForm;
        public LineDetection lineDetection;
        public BarcodeScanning barcodeScanning;
        public AngleDetection angleDetection;
        bool isBarcodeCalibration;
        bool isLineCalibration;
        public int droneCalibrationDirection { set; get; }
        public int turnDegrees { set; get; }
 

        public DroneController(MainForm form)
         
        {
            //The IP-address is always the default gateway when connected to the drone WiFi.
            droneClient = new DroneClient("192.168.1.1"); 
            autopilotController = new AutopilotController(droneClient, this);
            routeInterpreter = new RouteInterpreter(ref autopilotController);
            lineDetection = new LineDetection(form, this);
            angledetection = new AngleDetection(form, this);
            barcodeScanning = new BarcodeScanning(form, this);
            mainForm = form;
        }
       
        public async Task startSmartScan()
        {
            //initial setup, starting the drone, finding the line & calibrating
            flyTaskCompleted = new TaskCompletionSource<bool>();
            List<Position> route = MakeSmartScanRoute();

            autopilotController.Start();
            routeInterpreter.takeOffCommand.execute();
            routeInterpreter.shortHover.execute();

            await flyTaskCompleted.Task;
            await findLine();
            await turnCalibration();

            //loop through the made route
            for (int i = 0; i < route.Count()-1; i++ )
            {
              


                //fly to the next position
                flyTaskCompleted = new TaskCompletionSource<bool>();
                Position current = route[i];
                Position target = route[i+1];
                routeInterpreter.flyToCoordinate(current, target);
                await flyTaskCompleted.Task;

                //calibrate if the target position is a smartscan item & scan the item
                if (target.isTargetPosition)
                {
                    await findLine();
                    await turnCalibration();
                    await searchForBarcode(current);
                }


            }

            //land after the route is done
            routeInterpreter.landCommand.execute();
            //mainForm.wmsForm.showMutations();
           
        }


        public async Task CycleCount()
        {
            isAngleCalibration = true;
            mainForm.scanningForAngle = true;

            await Task.Delay(5000);
            await turnCalibration();
        }

        public async Task TESTCycleCount()
        {
            //initial setup, starting the drone, finding the line & calibrating
            flyTaskCompleted = new TaskCompletionSource<bool>();
            List<Position> route = MakeCycleCountRoute();
            autopilotController.Start(); 
            routeInterpreter.takeOffCommand.execute();
            routeInterpreter.shortHover.execute();

            await flyTaskCompleted.Task;
            await findLine();
            await turnCalibration();

            //loop through the route
            for (int i = 0; i < route.Count - 1; i++ )
            {
                Position current = route[i];
                Position target = route[i+1];

                //fly to the next position (uses flyToZ if the position is on another rack)
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

                //calibration and finding barcode
                await findLine();
                await turnCalibration();
                await searchForBarcode(current);

            }
            //land after the route is done
            routeInterpreter.landCommand.execute();
            //mainForm.wmsForm.showMutations();
           
        }

        //function for flying to another rack
        private async Task flyToZ(Position currentPos, Position targetPos){

            //turn 180 degrees
            routeInterpreter.turn.execute(180);

            //calibrate
            await findLine();
            await turnCalibration();

            //start flying to the other rack and find the line
            routeInterpreter.flyToOtherRack.execute();
            await Task.Delay(2000);
            await findLine();
            await turnCalibration();

            //fly to coordinate
            Position newCurrentPos = currentPos;
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

        //function for calibrating the drone by searching the barcode 
        private async Task searchForBarcode(Position i)
        {
            //enables front camera, enables the scanning for barcodes
            flyTaskCompleted = new TaskCompletionSource<bool>();
            switchCamera(1);
            routeInterpreter.shortHover.execute();
            mainForm.scanningForBarcode = true;
            
            //calibrate by fling to the left and right, stops when the barcode is found
            routeInterpreter.barcodeSmallLeft.execute(1000);
            routeInterpreter.shortHover.execute();
            routeInterpreter.barcodeSmallRight.execute(2000);
            routeInterpreter.shortHover.execute();
            routeInterpreter.barcodeSmallLeft.execute(1000);
            routeInterpreter.shortHover.execute();
            await flyTaskCompleted.Task;

            mainForm.scanningForBarcode = false;

            //switch back to bottom camera
            switchCamera(2);
            await Task.Delay(500);
        }

        //function for finding the line. used for front and back calibration
        public async Task findLine()
        {
            //enables scanning for the line
            mainForm.scanningForLine = true;

            //fly forwards and backwards to find the line. Stops if the line is found
            flyTaskCompleted = new TaskCompletionSource<bool>();
            await Task.Delay(1000);
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
            mainForm.scanningForLine = false;
           
        }

        //function used to manually set the flytask to completed.
        public void setFlyTaskCompleted()
        {
            flyTaskCompleted.TrySetResult(true);
        }


        //function used for calibrating the angle of the drone using the line on the ground
        public async Task turnCalibration()
        {
            //enables scanning for the angle
            mainForm.scanningForAngle = true;
            routeInterpreter.shortHover.execute();

            await Task.Delay(500);

            flyTaskCompleted = new TaskCompletionSource<bool>();
            if (turnDegrees < -10 || turnDegrees > 10)
            {
               routeInterpreter.turn.execute(turnDegrees);
               Console.WriteLine("[angle] turning :  " + turnDegrees +" degrees");
            }

            await flyTaskCompleted.Task;
            mainForm.scanningForAngle = false;
        }
       
        //function that stops all current tasks. Used for clearing the drone's tasks if it's calibrated.
        public void stopCurrentTasks(){
            autopilotController.clearObjectives();
            routeInterpreter.shortHover.execute();
        }

        //emergency button, used to immediately stop the motor of the drone
        public void emergency()
        {
            droneClient.Emergency();
        }

        //function used to switch to the bottom camera(for line detection) or front camera(barcode detection)
        public void switchCamera(int oneForFrontTwoForBottom)
        {
            //Send basic configuration
            Settings settings = new Settings();
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
                 if (oneForFrontTwoForBottom == 1){
                        settings.Video.Channel = VideoChannelType.Horizontal;
                        droneClient.Send(settings);
                 }
                        if (oneForFrontTwoForBottom == 2){
                        settings.Video.Channel = VideoChannelType.Vertical;
                        droneClient.Send(settings);
                 }
                    

                    droneClient.AckControlAndWaitForConfirmation();
                }

                droneClient.Send(settings);
            });
            sendConfigTask.Start();

       

        }
        
        public DroneClient getDroneClient()
        {
            return droneClient;
        }

        public void startClient()
        {
            droneClient.Start();

            //Send basic configuration
            Settings settings = new Settings();
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

        public void startAutopilot()
        {
            autopilotController.Start();
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

        public void Dispose()
        {
            droneClient.Dispose();
        }

        //function that makes a full cycle count route using the RoutePlan class
        private List<Position> MakeCycleCountRoute()
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
        private List<Position> MakeSmartScanRoute()
        {
            /*
             * Find all products exceeding the deviation threshhold and plot a route between those products.
             */
            double threshold = 0; //Products exceeding the deviation threshold will be smartscanned.
            List<Product> products = new List<Product>();

            using (ProductDBContext db = new ProductDBContext())
            {
                products = db.Products.Where(p => p.Deviation > threshold).ToList();
            }

            List<Position> itemsToCheck = new List<Position>();
            foreach(Product p in products)
            {
                itemsToCheck.Add(new Position(p.X, p.Y, p.Z, true));
            }

            return RoutePlan.makeSmartScanRoute(itemsToCheck);
        }

      
}
