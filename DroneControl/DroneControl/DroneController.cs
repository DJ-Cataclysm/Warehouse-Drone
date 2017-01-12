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
        bool isBarcodeCalibration;
        bool isLineCalibration;
        bool isAngleCalibration;
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

        public async Task TESTCycleCount()
        {
            isAngleCalibration = true;
            mainForm.scanningForAngle = true;
        }

        public async Task CycleCount()
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
            isBarcodeCalibration = true;
            mainForm.scanningForBarcode = true;
            
            //calibrate by fling to the left and right, stops when the barcode is found
            routeInterpreter.barcodeSmallLeft.execute(1000);
            routeInterpreter.shortHover.execute();
            routeInterpreter.barcodeSmallRight.execute(2000);
            routeInterpreter.shortHover.execute();
            routeInterpreter.barcodeSmallLeft.execute(1000);
            routeInterpreter.shortHover.execute();
            await flyTaskCompleted.Task;

            isBarcodeCalibration = false;
            mainForm.scanningForBarcode = false;

            //switch back to bottom camera
            switchCamera(2);
            await Task.Delay(500);
        }

        //function for finding the line. used for front and back calibration
        public async Task findLine()
        {
            //enables scanning for the line
            isLineCalibration = true;
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
            isLineCalibration = false;
            mainForm.scanningForLine = false;
           
        }
        //function used to manually set the flytask to completed.
        public void setFlyTaskCompleted()
        {
            flyTaskCompleted.TrySetResult(true);
        }

/*
 * -----------------------
 * 
 *checken of deze functie weg kan
 *
 * ----------------------------
 * 
 */

    //    public async Task calibration(){
    //        flyTaskCompleted = new TaskCompletionSource<bool>();

    //        //moet naar voren
    //        if(droneCalibrationDirection == 1){
    //           routeInterpreter.goForwardCalibration.execute();
    //           routeInterpreter.shortHover.execute();
    //            routeInterpreter.goBackwardsCalibration.execute();
    //            routeInterpreter.shortHover.execute();

    //        }
    //        //moet naar achter
    //        if (droneCalibrationDirection == -1)
    //        {
    //            routeInterpreter.goBackwardsCalibration.execute();
    //            routeInterpreter.shortHover.execute();
    //            routeInterpreter.goForwardCalibration.execute();
    //            routeInterpreter.shortHover.execute();
    //        }

    //        await flyTaskCompleted.Task;
    //}

        //function used for calibrating the angle of the drone using the line on the ground
        public async Task turnCalibration()
        {
            //enables scanning for the angle
            isAngleCalibration = true;
            mainForm.scanningForAngle = true;
            routeInterpreter.shortHover.execute();
            flyTaskCompleted = new TaskCompletionSource<bool>();
           
            //if the angle is out of the range turn the drone
            if (turnDegrees < -10 || turnDegrees > 10)
            {
               routeInterpreter.turn.execute(turnDegrees);
               Console.WriteLine("[angle] turning ----> " + turnDegrees +"  <---- degrees");
            }

            await flyTaskCompleted.Task;
            isAngleCalibration = false;
            mainForm.scanningForAngle = false;
        }
       
        //function that stops all current tasks. Used for clearing the drone's tasks if it's calibrated.
        public void stopCurrentTasks(){
            Console.WriteLine("[clearing objectives] iets heeft de stop getriggered.");
            autopilotController.clearObjectives();
            routeInterpreter.shortHover.execute();
        }

        /*function that scans for the barcode, and if the barcode is found updates it in the WMS
         * Called from Mainform frame update */
        public void scanForBarcode()
        {
            string barcode;

            // create a barcode reader instance
            IBarcodeReader reader = new BarcodeReader();
            // detect and decode the barcode inside the bitmap
            var result = reader.Decode(mainForm.getFrame());
            // return result or error
            if (result != null)
            {
                barcode = result.Text;
            }
            else { barcode = null; }

            if (barcode != null)
            {
                Console.WriteLine("Barcode gevoden " + barcode);
                int id = int.MaxValue;
                int.TryParse(barcode, out id);
                mainForm.wmsForm.productScanned(id);
                mainForm.scanningForBarcode = false;

                if (isBarcodeCalibration)
                {
                    isBarcodeCalibration = false;
                     stopCurrentTasks();
                    Console.WriteLine("[barcode] stop barcode calibratie , gevonden barcode: " + barcode);
                }
            }
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

        //function that calculates the angle from the drone using the line. Called from mainform frame update.
        public void calculateAngle()
        {
            Bitmap videoFrame = mainForm.getFrame();
            BitmapData bitmapData = createFilteredBitMap(videoFrame);
            BlobCounter blobCounter = new BlobCounter();
            Blob[] blobs = findBlobs(blobCounter, bitmapData);
            SimpleShapeChecker shapeChecker = new SimpleShapeChecker();
            Graphics g = Graphics.FromImage(videoFrame);
            videoFrame.UnlockBits(bitmapData);
            int angleDeg = 0;
            bool goLeft;

            // Check if there's a line on the ground with a minimum surface area of 200 pixels and get it in the middle of the screen
            for (int i = 0, n = blobs.Length; i < n; i++)
            {
                List<IntPoint> edgePoints = blobCounter.GetBlobsEdgePoints(blobs[i]);
                List<IntPoint> corners;
                if (shapeChecker.IsQuadrilateral(edgePoints, out corners) && corners[0].DistanceTo(corners[1]) * corners[1].DistanceTo(corners[2]) > 200)
                {
                    // Locate one of the two shortest side of the line
                    double shortestDistanceTo = 99999999;
                    int shortestLineCorner1 = 0;
                    int shortestLineCorner2 = 0;
                    for (int a = 0; a < corners.Count; a++)
                    {
                        for (int b = 1; b < corners.Count-1; b++)
                        {
                            double currentDistanceTo = corners[a].DistanceTo(corners[b]);
                            if (currentDistanceTo < shortestDistanceTo && a != b)
                            {
                                shortestDistanceTo = currentDistanceTo;
                                    shortestLineCorner1 = a;
                                    shortestLineCorner2 = b;
                            }
                        }
                    }
                    IntPoint corner1 = corners[shortestLineCorner1];
                    IntPoint corner2 = corners[shortestLineCorner2];
                    IntPoint crossing;

                    double aSide;
                    double bSide;
                    double cSide;

                    //Use the top point of the small side to create a triangle with the bottom point of the small side
                    if (corner1.Y < corner2.Y)
                    {
                        crossing = new IntPoint(corner1.X, corner2.Y);
                        aSide = corner1.DistanceTo(corner2);
                        bSide = corner1.DistanceTo(crossing);
                        cSide = corner2.DistanceTo(crossing);
                        if (corner1.X < corner2.X)
                        {
                            goLeft = true;
                        }
                        else
                        {
                            goLeft = false;
                        }
                    }
                    else
                    {
                        crossing = new IntPoint(corner2.X, corner1.Y);
                        aSide = corner2.DistanceTo(corner1);
                        bSide = corner2.DistanceTo(crossing);
                        cSide = corner1.DistanceTo(crossing);
                        if (corner2.X < corner1.X)
                        {
                            goLeft = true;
                        }
                        else
                        {
                            goLeft = false;
                        }
                    }

                    //Use the known sides to calculate the angle and direction that the drone has to turn to align itself with the line
                    //using the formula cos(C) = (A^2 + B^2 - C^2) / (2*A*B)
                    double aCos = Math.Acos(((aSide * aSide) + (bSide * bSide) - (cSide * cSide)) / ((2 * aSide) * bSide));
                    angleDeg = (int)Math.Ceiling(aCos * (180.0 / Math.PI));

                    if (goLeft)
                    {
                        angleDeg = angleDeg * -1;
                    } 
                } 
            }

            if (angleDeg > 3 || angleDeg < -3)
            {
                turnDegrees = angleDeg;
            }
           if (turnDegrees >-5 && turnDegrees < 5 && turnDegrees != 0){
           if (isAngleCalibration)
                    {
                        //isAngleCalibration = false;
                      
                        //stopCurrentTasks();

                        Console.WriteLine(turnDegrees + " [angle] Correcte angle --> Doorgaan!");
                    }
           }
        }

        //function that checks if the line is found. Called from Mainform frame update
        public async Task zoekLijn()
        {
            Bitmap videoFrame = mainForm.getFrame();
            BitmapData bitmapData = createFilteredBitMap(videoFrame);
            SimpleShapeChecker shapeChecker = new SimpleShapeChecker();
            Graphics g = Graphics.FromImage(videoFrame);
            BlobCounter blobCounter = new BlobCounter();
            Blob[] blobs = findBlobs(blobCounter, bitmapData);
            videoFrame.UnlockBits(bitmapData);

            // Check if there's a line on the ground and get it in the middle of the screen
            for (int i = 0, n = blobs.Length; i < n; i++)
            {
                List<IntPoint> edgePoints = blobCounter.GetBlobsEdgePoints(blobs[i]);
                List<IntPoint> corners;
                if (shapeChecker.IsQuadrilateral(edgePoints, out corners))
                {
                    mainForm.lineFound = true;
                    mainForm.scanningForLine = false;
                    if (isLineCalibration)
                    {
                        isLineCalibration = false;
                        await Task.Delay(700);
                        stopCurrentTasks();
                    }
                }
            }
            mainForm.pbVideo.Image = videoFrame;
            g.Dispose();
        }

        private BitmapData createFilteredBitMap(Bitmap frame)
        {
            // Lock image to prevent other sources from interfering
            BitmapData bitmapData = frame.LockBits(
                new Rectangle(0, 0, frame.Width, frame.Height),
                ImageLockMode.ReadWrite, frame.PixelFormat);

            // Turn anything that isn't white, into black
            ColorFiltering colorFilter = new ColorFiltering();
            colorFilter.Red = new IntRange(160, 255);
            colorFilter.Green = new IntRange(160, 255);
            colorFilter.Blue = new IntRange(160, 255);
            colorFilter.FillOutsideRange = true;
            colorFilter.ApplyInPlace(bitmapData);

            return bitmapData;
        }

        private Blob[] findBlobs(BlobCounter counter, BitmapData bmpData)
        {
            // Find the corners in the frame and identify them
            counter.FilterBlobs = true;
            counter.MinHeight = 5;
            counter.MinWidth = 5;
            counter.ProcessImage(bmpData);
            Blob[] blobs = counter.GetObjectsInformation();

            return blobs;
        }
    }
}
