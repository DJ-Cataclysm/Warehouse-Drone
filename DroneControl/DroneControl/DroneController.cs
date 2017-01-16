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
using System.Diagnostics;

namespace DroneControl
{
    public class DroneController
    {
        AutopilotController autopilotController;
        DroneClient droneClient;
        RouteInterpreter routeInterpreter;
        TaskCompletionSource<bool> flyTaskCompleted, cameraSwitched;
        MainForm mainForm;
        Settings settings;
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

            //await Task.Delay(5000);
            //await turnCalibration();

            //await switchCamera(VideoChannelType.Horizontal);

            //await switchCamera(VideoChannelType.Vertical);
            //await switchCamera(VideoChannelType.Horizontal);
            //await switchCamera(VideoChannelType.Vertical);
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
            await switchCamera(VideoChannelType.Vertical);
            //await findLine();
            await turnCalibration();
            await switchCamera(VideoChannelType.Horizontal);

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
                await switchCamera(VideoChannelType.Vertical);
                //await findLine();
                await turnCalibration();
                await switchCamera(VideoChannelType.Horizontal);
                await searchForBarcode(current);

            }
            //land after the route is done
            routeInterpreter.landCommand.execute();
            mainForm.wmsForm.showMutations();
           
        }

        //function for flying to another rack
        private async Task flyToZ(Position currentPos, Position targetPos){

            //turn 180 degrees
            routeInterpreter.turn.execute(180);

            //calibrate
            //await findLine();
            //await turnCalibration();

            //start flying to the other rack and find the line
            routeInterpreter.flyToOtherRack.execute();
            await Task.Delay(2000);
            await switchCamera(VideoChannelType.Vertical);
            await findLine();
            await turnCalibration();
            await switchCamera(VideoChannelType.Horizontal);

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
            await switchCamera(VideoChannelType.Horizontal);
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
            await switchCamera(VideoChannelType.Vertical);
        }

        //function for finding the line. used for front and back calibration
        public async Task findLine()
        {
            //enables scanning for the line
            isLineCalibration = true;
            mainForm.scanningForLine = true;

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

            await Task.Delay(500);

            flyTaskCompleted = new TaskCompletionSource<bool>();
            Console.WriteLine("I should turn: " + turnDegrees);
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
        public async Task switchCamera(VideoChannelType videoChannelType)
        {
            //Send basic configuration
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
            await Task.Delay(500);
        }

        public DroneClient getDroneClient()
        {
            return droneClient;
        }

        public void startClient()
        {
            droneClient.Start();

            //Send basic configuration
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
        public void detectLine()
        {
            Bitmap myBitmap = mainForm.getFrame();

            // lock image
            BitmapData bitmapData = myBitmap.LockBits(
                new Rectangle(0, 0, myBitmap.Width, myBitmap.Height),
                ImageLockMode.ReadWrite, 
                myBitmap.PixelFormat);

            // step 1 - turn background to black
            IntRange colorRange = new IntRange(150, 255); 
            ColorFiltering colorFilter = new ColorFiltering(colorRange, colorRange, colorRange);
            colorFilter.FillOutsideRange = true; //All colors outside colorRange range are turned black
            colorFilter.ApplyInPlace(bitmapData); //Apply to current bitmap

            // step 2 - locating objects
            BlobCounter blobCounter = new BlobCounter()
            {
                FilterBlobs = true,
                MinHeight = 5,
                MinWidth = 5,
            };
            blobCounter.ProcessImage(bitmapData);

            Blob[] blobs = blobCounter.GetObjectsInformation();

            myBitmap.UnlockBits(bitmapData);

            // step 3 - check objects' type and highlight
            // Check if there's a line on the ground with a minimum surface area of 200 square pixels and get it in the middle of the screen
            for (int i = 0; i < blobs.Length; i++)
            {
                List<IntPoint> edgePoints = blobCounter.GetBlobsEdgePoints(blobs[i]);
                List<IntPoint> corners;
                
                if (isQuadrilateralAndBigEnough(edgePoints, out corners))
                {
                    //Find upperleft corner
                    IntPoint upperLeftCorner = corners.Aggregate((curMin, c) => (c.X + c.Y) < (curMin.X + curMin.Y) ? c : curMin);
                    corners.Remove(upperLeftCorner);

                    IntPoint lowerRightCorner = corners.Aggregate((curMax, c) => (c.X + c.Y) > (curMax.X + curMax.Y) ? c : curMax);

                    //Get IntPoint making shortest line
                    IntPoint lowerLeftCorner = findPointMakingLongestOrShortestLine(upperLeftCorner, corners, false);
                    corners.Remove(lowerLeftCorner);

                    IntPoint remainingCorner;
                    if (upperLeftCorner.Y < lowerRightCorner.Y)
                    {
                        //Get IntPoint making shortest line
                        remainingCorner = findPointMakingLongestOrShortestLine(upperLeftCorner, corners, false);
                    }
                    else
                    {
                        //Get IntPoint making longest line
                        remainingCorner = findPointMakingLongestOrShortestLine(upperLeftCorner, corners, true);
                    }

                    /*
                     * We now have two points. 
                     * Enough to calculate the angle of the line compared to the relative horizon of the drone camera feed.
                     */
                    turnDegrees = calculateAngle(upperLeftCorner, remainingCorner);

                    /*
                    using (Graphics g = Graphics.FromImage(myBitmap))
                    {
                        Pen redPen = new Pen(Color.Red, 8);
                        g.DrawLine(redPen, upperLeftCorner.X, upperLeftCorner.Y, remainingCorner.X, remainingCorner.Y);
                    }*/
                }
            }
        }

        private bool isQuadrilateralAndBigEnough(List<IntPoint> edgePoints, out List<IntPoint> corners)
        {
            double threshold = 200d;
            SimpleShapeChecker shapeChecker = new SimpleShapeChecker();
            bool isQuadrilateral = shapeChecker.IsQuadrilateral(edgePoints, out corners);
            bool isBigEnough = corners[0].DistanceTo(corners[1]) * corners[1].DistanceTo(corners[2]) > threshold;
            return isQuadrilateral && isBigEnough;
        }

        private int calculateAngle(IntPoint pointA, IntPoint pointB)
        {
            if (pointB.X - pointA.X == 0)
            {
                return 0;
            }
            double angleRadians = Math.Atan(((double)pointB.Y - pointA.Y) / (pointB.X - pointA.X));
            int degrees = (int)Math.Ceiling(angleRadians * (180.0 / Math.PI));
            return degrees;
        }

        private IntPoint findPointMakingLongestOrShortestLine(IntPoint startPoint, List<IntPoint> points, bool getLongest)
        {
            /*
             * Returns the point making the longest or shortest line between itself and startPoint.
             * This depends on the getLongest bool. If true then get the longest, else get shortest line.
             */
            double knownDistance;
            if (getLongest)
            {
                knownDistance = 0d;
            }
            else
            {
                knownDistance = double.MaxValue;
            }

            IntPoint longestOrShortestPoint = startPoint; //Must be initialized
            foreach (IntPoint point in points)
            {
                double currentDistance = startPoint.DistanceTo(point);
                if (getLongest && currentDistance > knownDistance)
                {
                    longestOrShortestPoint = point;
                    knownDistance = currentDistance;
                }
                else if(!getLongest && currentDistance < knownDistance)
                {
                    longestOrShortestPoint = point;
                    knownDistance = currentDistance;
                }
            }
            return longestOrShortestPoint;
        }

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
            colorFilter.Red = new IntRange(150, 255);
            colorFilter.Green = new IntRange(150, 255);
            colorFilter.Blue = new IntRange(150, 255);
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
