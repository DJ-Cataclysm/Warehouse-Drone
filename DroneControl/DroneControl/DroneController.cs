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
        Route route;
        TaskCompletionSource<bool> flyTaskCompleted;
        TaskCompletionSource<bool> vormTaskCompleted;
        TaskCompletionSource<bool> scanTaskComleted;
        MainForm mainForm;
        bool isBarcodeCalibration;
        bool isLineCalibration;
        bool isAngleCalibration;
        bool isLeft;
        public int droneCalibrationDirection { set; get; }
        int turnDegrees;


        public DroneController(MainForm form)
         
        {
            //The IP-address is always the default gateway when connected to the drone WiFi.
            droneClient = new DroneClient("192.168.1.1"); 
            autopilotController = new AutopilotController(droneClient, this);
            routeInterpreter = new RouteInterpreter(ref autopilotController);
            mainForm = form;
        }

        public async Task CycleCount()
        {
            flyTaskCompleted = new TaskCompletionSource<bool>();
            vormTaskCompleted = new TaskCompletionSource<bool>();

            route = MakeCycleCountRoute();
            List<Position> routeList = route.getPositions();
            autopilotController.Start();
            routeInterpreter.takeOffCommand.execute();
            routeInterpreter.shortHover.execute();
            await flyTaskCompleted.Task;

            // lijn vinden

            isLineCalibration = true;
            mainForm.scanningForLine = true;
            await findLine();
            isLineCalibration = false;
            mainForm.scanningForLine = false;


            // hoek calibratie
            routeInterpreter.shortHover.execute();

            isAngleCalibration = true;
            mainForm.scanningForAngle = true;
            await turnCalibration();
            isAngleCalibration = false;
            mainForm.scanningForAngle  = false;

            //flyTaskComleted = new TaskCompletionSource<bool>();
            //mainForm.isDroneReady = true;
            //await Task.Delay(200);

            //await findLine();
            //mainForm.isDroneReady = false;
            //autopilotController.Start();
            //routeInterpreter.shortHover.execute();
            //await flyTaskComleted.Task;

            for (int i = 0; i < route.getCount()-1; i++ )
            {
              
                Console.Write(" ga door met loop");

                flyTaskCompleted = new TaskCompletionSource<bool>();

                Position current = routeList[i];
                Position target = routeList[i+1];

                routeInterpreter.flyToCoordinate(current, target);
           //     autopilotController.Start();

                Console.Write("flying to target");
                await flyTaskCompleted.Task;

                //voor en achter calibratie
                isLineCalibration = true;
                mainForm.scanningForLine = true;
                await calibration();
                isLineCalibration = false;
                mainForm.scanningForLine = false;

                
                //barcode calibratie
                switchCamera(1);
              isBarcodeCalibration = true;
          mainForm.scanningForBarcode = true;
            await searchForBarcode(current);
            isBarcodeCalibration = false;
            mainForm.scanningForBarcode = false;
                switchCamera(2);



            }
            routeInterpreter.landCommand.execute();
            //mainForm.wmsForm.showMutations();
            // start cycle count
           
        }
        private async Task searchForBarcode(Position i)
        {


            flyTaskCompleted = new TaskCompletionSource<bool>();

            routeInterpreter.barcodeSmallLeft.execute(1000);
            routeInterpreter.shortHover.execute();
            //rechts2 
            routeInterpreter.barcodeSmallRight.execute(2000);
            routeInterpreter.shortHover.execute();
            //links 1
            routeInterpreter.barcodeSmallLeft.execute(1000);
            routeInterpreter.shortHover.execute();
           await flyTaskCompleted.Task;
   
        }

        public async Task findLine()
        {
            flyTaskCompleted = new TaskCompletionSource<bool>();

            routeInterpreter.goForwardCalibration.execute();
            routeInterpreter.shortHover.execute();
            routeInterpreter.goBackwardsCalibration.execute();
            routeInterpreter.shortHover.execute();
            routeInterpreter.goBackwardsCalibration.execute();
            routeInterpreter.shortHover.execute();
            routeInterpreter.goForwardCalibration.execute();
            routeInterpreter.shortHover.execute();

            await flyTaskCompleted.Task;
        }
        public void setFlyTaskCompleted()
        {
            flyTaskCompleted.TrySetResult(true);
        }


        public async Task calibration(){
            flyTaskCompleted = new TaskCompletionSource<bool>();

            //moet naar voren
            if(droneCalibrationDirection == 1){
               routeInterpreter.goForwardCalibration.execute();
               routeInterpreter.shortHover.execute();
                routeInterpreter.goBackwardsCalibration.execute();
                routeInterpreter.shortHover.execute();

            }
            //moet naar achter
            if (droneCalibrationDirection == -1)
            {
                routeInterpreter.goBackwardsCalibration.execute();
                routeInterpreter.shortHover.execute();
                routeInterpreter.goForwardCalibration.execute();
                routeInterpreter.shortHover.execute();
            }

            await flyTaskCompleted.Task;
    }

        public async Task turnCalibration()
        { 
            flyTaskCompleted = new TaskCompletionSource<bool>();
            

           
            if (turnDegrees < -5 || turnDegrees > 5)
            {
               routeInterpreter.turn.execute(turnDegrees);
               Console.WriteLine("[angle] turning ----> " + turnDegrees +"  <---- degrees");

            }
           

                      await flyTaskCompleted.Task;

        }
       

        public void stopCurrentTasks(){
            Console.WriteLine("*** STOPPING (clearing objectives");
            autopilotController.clearObjectives();
            //autopilotController.Stop();
            //setFlyTaskCompleted();
            routeInterpreter.shortHover.execute();
        }

        public void setVormTaskCompleted()
        {
            vormTaskCompleted.SetResult(true);
        }
        
        public async Task SmartScan()
        {
            Route route = MakeSmartScanRoute();
            List<Position> routeList = route.getPositions();

            flyTaskCompleted = new TaskCompletionSource<bool>();

            autopilotController.Start();
            routeInterpreter.takeOffCommand.execute();

            await flyTaskCompleted.Task; //Begin when drone is in the air and ready to fly the route

            for (int i = 0; i < route.getCount() - 1; i++)
            {
                flyTaskCompleted = new TaskCompletionSource<bool>();

                Position current = routeList[i];
                Position target = routeList[i + 1];

                routeInterpreter.flyToCoordinate(current, target);

                await flyTaskCompleted.Task;

                //voor en achter calibratie
                /*
                mainForm.isDroneReady = true;
                if (droneCalibrationDirection != 0)
                {
                    await calibration();
                }*/

                //barcode calibratie
                if (target.isTargetPosition)
                {
                    isBarcodeCalibration = true;
                    mainForm.scanningForBarcode = true;
                    await searchForBarcode(current);
                    isBarcodeCalibration = false;
                    mainForm.scanningForBarcode = false;
                }
                
            }
            routeInterpreter.landCommand.execute();
            //autopilotController.Stop();
            mainForm.wmsForm.showMutations();
        }

        public void scanForBarcode()
        {
            string barcode = null;
            barcode = scanBarcode();
            if (barcode != null)
            {
                Console.WriteLine("Barcode gevoden " + barcode);
                //scanTaskComleted.SetResult(true);
                int id = int.MaxValue;
                int.TryParse(barcode, out id);
                mainForm.wmsForm.productScanned(id);
                mainForm.scanningForBarcode = false;

                if (isBarcodeCalibration)
                {
                    isBarcodeCalibration = false;
                     stopCurrentTasks();
                  
                    //autopilotController.Start();
                    Console.WriteLine("<< BARCODE CALIBRATION STOPPED >> " + barcode);
                    
                }
            }
        }

        private string scanBarcode()
        {
            // create a barcode reader instance
            IBarcodeReader reader = new BarcodeReader();
            // detect and decode the barcode inside the bitmap
            
            
             var result = reader.Decode(mainForm.getFrame());
            // return result or error
            if (result != null)
            {
                return result.Text;
            }
            else { return null; }

        }

        public void enqueueTest()
        {
            routeInterpreter.testRoute();
        }

        public void emergency()
        {
            droneClient.Emergency();
        }

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

        private Route MakeCycleCountRoute()
        {
            /*
             * Find all products, create a position per product and then plot a route between those positions.
             */
            List<Product> products = new List<Product>();

            using (ProductDBContext db = new ProductDBContext())
            {
                products = db.Products.ToList();
            }

            foreach (Product p in products)
            {
                Positions.addPosition(new Position(p.X, p.Y, p.Z));
            }
            //Make full cycle route using the positions in the static Positions class
            return RoutePlan.makeFullCycleRoute();
        }

        private Route MakeSmartScanRoute()
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

        public void calculateAngle()
        {
            Bitmap myBitmap = mainForm.getFrame();
            int angleDeg = 0;

            // lock image
            BitmapData bitmapData = myBitmap.LockBits(
                new Rectangle(0, 0, myBitmap.Width, myBitmap.Height),
                ImageLockMode.ReadWrite, myBitmap.PixelFormat);

            // step 1 - turn background to black
            ColorFiltering colorFilter = new ColorFiltering();

            colorFilter.Red = new IntRange(225, 255);
            colorFilter.Green = new IntRange(225, 255);
            colorFilter.Blue = new IntRange(225, 255);
            colorFilter.FillOutsideRange = true;

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
                    double longestDistanceTo = 0;
                    int longestCorner1 = 0;
                    int longestCorner2 = 0;
                    for (int a = 0; a < corners.Count; a++)
                    {
                        for (int b = 1; b < corners.Count-1; b++)
                        {
                            double currentDistanceTo = corners[a].DistanceTo(corners[b]);
                            if (currentDistanceTo > longestDistanceTo)
                            {
                                longestDistanceTo = currentDistanceTo;
                                longestCorner1 = a;
                                longestCorner2 = b;
                            }
                        }
                    }

                    IntPoint middleF = new IntPoint((corners[longestCorner1].X + corners[longestCorner2].X) / 2, (corners[longestCorner1].Y + corners[longestCorner2].Y));
                    IntPoint top = new IntPoint(middleF.X, middleF.Y + 20);
                    double aLength = top.DistanceTo(middleF);
                    double bLength = middleF.DistanceTo(corners[longestCorner2]);
                    double cLength = top.DistanceTo(corners[longestCorner2]);

                    //Use the known Vertexes to calculate the angle that the drone deviates from the top Point
                    double aCos = ((aLength * aLength) + (cLength * cLength) - (bLength * bLength)) / ((2 * aLength) * cLength);
                    double angleRad = Math.Acos(aCos);
                    angleDeg = ((int)Math.Ceiling(angleRad * (180 / Math.PI))) - 90;
                   
                } 
            }

       turnDegrees = angleDeg;

           if (turnDegrees >-5 && turnDegrees < 10){

          if (isAngleCalibration)
                    {
                        isAngleCalibration = false;
                      
                        stopCurrentTasks();

                        Console.WriteLine("[angle] Correcte angle --> Doorgaan!");

                    }
           }
        }


        public async Task zoekLijn()
        {
            Bitmap myBitmap = mainForm.getFrame();

            // lock image
            BitmapData bitmapData = myBitmap.LockBits(
                new Rectangle(0, 0, myBitmap.Width, myBitmap.Height),
                ImageLockMode.ReadWrite, myBitmap.PixelFormat);

            // step 1 - turn background to black
            ColorFiltering colorFilter = new ColorFiltering();

            colorFilter.Red = new IntRange(225, 255);
            colorFilter.Green = new IntRange(225, 255);
            colorFilter.Blue = new IntRange(225, 255);
            colorFilter.FillOutsideRange = true;

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
                        Console.WriteLine(corner);
                    }
                    mainForm.lineFound = true;
                    mainForm.scanningForLine = false;
                    Console.WriteLine(">>>>> Lijn gevonden <<<<<<");


                    if (isLineCalibration)
                    {
                        isLineCalibration = false;
                        await Task.Delay(1000);
                        stopCurrentTasks();

                        //autopilotController.Start();
                        Console.WriteLine(">>>>> Lijn gevonden STOP de movement >>>>>>>>>>>>");

                    }

                }
            }
            mainForm.pbVideo.Image = myBitmap;
            g.Dispose();
        }
    }
}
