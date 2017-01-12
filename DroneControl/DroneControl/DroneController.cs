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

        public async Task startSmartScan()
        {
            flyTaskCompleted = new TaskCompletionSource<bool>();
            route = MakeSmartScanRoute();
            List<Position> routeList = route.getPositions();

            autopilotController.Start();
            routeInterpreter.takeOffCommand.execute();
            routeInterpreter.shortHover.execute();

            await flyTaskCompleted.Task;

            await findLine();

            await turnCalibration();


                 //na het opstijgen en calibreren, loop door de gemaakte route
            for (int i = 0; i < route.getCount()-1; i++ )
            {
              


                //vlieg naar de volgende positie
                flyTaskCompleted = new TaskCompletionSource<bool>();
                Position current = routeList[i];
                Position target = routeList[i+1];
                routeInterpreter.flyToCoordinate(current, target);
                await flyTaskCompleted.Task;

                if (target.isTargetPosition)
                {

                    //voor en achter calibratie
                    await findLine();

                    // hoek calibratie
                    await turnCalibration();

                    //barcode calibratie
                    await searchForBarcode(current);
             
                }


            }
            //einde route, landen.
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
            // Maak de route, start de drone
            flyTaskCompleted = new TaskCompletionSource<bool>();
            vormTaskCompleted = new TaskCompletionSource<bool>();

            //this.route = MakeCycleCountRoute();
            //List<Position> route = this.route.getPositions();
            List<Position> route = MakeCycleCountRoute();
            autopilotController.Start(); 
            routeInterpreter.takeOffCommand.execute();
            routeInterpreter.shortHover.execute();
            await flyTaskCompleted.Task;

            await findLine();

            await turnCalibration();

            //na het opstijgen en calibreren, loop door de gemaakte route
            for (int i = 0; i < route.Count - 1; i++ )
            {

                //vlieg naar de volgende positie
                Position current = routeList[i];
                Position target = routeList[i+1];

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

                await findLine();

                await turnCalibration();

                //barcode calibratie
                await searchForBarcode(current);

            }
            //einde route, landen.
            routeInterpreter.landCommand.execute();
            //mainForm.wmsForm.showMutations();
           
        }

        private async Task flyToZ(Position currentPos, Position targetPos){
            //turn 180 degrees
            routeInterpreter.turn.execute(180);

            //calibrate
            await findLine();
            await turnCalibration();

            //andere lijn vinden
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

        //zoek naar een barcode door naar links en naar rechts te vliegen. 
        private async Task searchForBarcode(Position i)
        {
            flyTaskCompleted = new TaskCompletionSource<bool>();
            switchCamera(1);
            routeInterpreter.shortHover.execute();

            isBarcodeCalibration = true;
            mainForm.scanningForBarcode = true;
 
            flyTaskCompleted = new TaskCompletionSource<bool>();
            //1 maal links
            routeInterpreter.barcodeSmallLeft.execute(1000);
            routeInterpreter.shortHover.execute();
            //2 maal rechts
            routeInterpreter.barcodeSmallRight.execute(2000);
            routeInterpreter.shortHover.execute();
            //1 maal links
            routeInterpreter.barcodeSmallLeft.execute(1000);
            routeInterpreter.shortHover.execute();
           await flyTaskCompleted.Task;

           isBarcodeCalibration = false;
           mainForm.scanningForBarcode = false;

           switchCamera(2);
           await Task.Delay(500);
        }

        public async Task findLine()
        {

            isLineCalibration = true;
            mainForm.scanningForLine = true;

            flyTaskCompleted = new TaskCompletionSource<bool>();
            await Task.Delay(3000);
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

        //calibreren van hoek
        public async Task turnCalibration()
        {

            isAngleCalibration = true;
            mainForm.scanningForAngle = true;
            routeInterpreter.shortHover.execute();

            flyTaskCompleted = new TaskCompletionSource<bool>();
           
            if (turnDegrees < -10 || turnDegrees > 10)
            {
               routeInterpreter.turn.execute(turnDegrees);
               Console.WriteLine("[angle] turning ----> " + turnDegrees +"  <---- degrees");
            }
          await flyTaskCompleted.Task;
          isAngleCalibration = false;
          mainForm.scanningForAngle = false;
        }
       

        public void stopCurrentTasks(){
            Console.WriteLine("[clearing objectives] iets heeft de stop getriggered.");
            autopilotController.clearObjectives();
            routeInterpreter.shortHover.execute();
        }

        public void setVormTaskCompleted()
        {
            vormTaskCompleted.SetResult(true);
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
                    Console.WriteLine("[barcode] stop barcode calibratie , gevonden barcode: " + barcode);
                    
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

        public void calculateAngle()
        {
            Bitmap myBitmap = mainForm.getFrame();
            int angleDeg = 0;
            bool goLeft;

            // lock image
            BitmapData bitmapData = myBitmap.LockBits(
                new Rectangle(0, 0, myBitmap.Width, myBitmap.Height),
                ImageLockMode.ReadWrite, myBitmap.PixelFormat);

            // step 1 - turn background to black
            ColorFiltering colorFilter = new ColorFiltering();

            colorFilter.Red = new IntRange(150, 255);
            colorFilter.Green = new IntRange(150, 255);
            colorFilter.Blue = new IntRange(150, 255);
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
                

                if (shapeChecker.IsQuadrilateral(edgePoints, out corners) && corners[0].DistanceTo(corners[1]) * corners[1].DistanceTo(corners[2]) > 200)
                {
                    double shortestDistanceTo = 99999999;
                    int shortestCorner1 = 0;
                    int shortestCorner2 = 0;
                    for (int a = 0; a < corners.Count; a++)
                    {
                        for (int b = 1; b < corners.Count-1; b++)
                        {
                            double currentDistanceTo = corners[a].DistanceTo(corners[b]);
                            if (currentDistanceTo < shortestDistanceTo && a != b)
                            {
                                shortestDistanceTo = currentDistanceTo;
                                    shortestCorner1 = a;
                                    shortestCorner2 = b;
                            }
                        }
                    }
                    IntPoint corner1 = corners[shortestCorner1];
                    IntPoint corner2 = corners[shortestCorner2];
                    IntPoint crossing;
                    Console.WriteLine(corner1.X + corner1.Y);

                    double aLength;
                    double bLength;
                    double cLength;

                    if (corner1.Y < corner2.Y)
                    {
                        crossing = new IntPoint(corner1.X, corner2.Y);
                        aLength = corner1.DistanceTo(corner2);
                        bLength = corner1.DistanceTo(crossing);
                        cLength = corner2.DistanceTo(crossing);
                        if (corner1.X < corner2.X)
                            goLeft = true;
                        else
                            goLeft = false;
                    }
                    else
                    {
                        crossing = new IntPoint(corner2.X, corner1.Y);
                        aLength = corner2.DistanceTo(corner1);
                        bLength = corner2.DistanceTo(crossing);
                        cLength = corner1.DistanceTo(crossing);
                        if (corner2.X < corner1.X)
                            goLeft = true;
                        else
                            goLeft = false;
                    }
                    
                    

                    Pen redPen = new Pen(Color.Red, 2);
                    g.DrawLine(redPen, corner1.X, corner1.Y, corner2.X, corner2.Y);
                    g.DrawLine(redPen, corner1.X, corner1.Y, crossing.X, crossing.Y);
                    g.DrawLine(redPen, corner2.X, corner2.Y, crossing.X, crossing.Y);



                    //Use the known Vertexes to calculate the angle that the drone deviates from the top Point
                    double aCos = Math.Acos(((aLength * aLength) + (bLength * bLength) - (cLength * cLength)) / ((2 * aLength) * bLength));
                    angleDeg = (int)Math.Ceiling(aCos * (180.0 / Math.PI));

                    if (goLeft)
                    {
                        angleDeg = angleDeg * -1;
                    } 
                } 
            }

            if (angleDeg != 0)
            {
                turnDegrees = angleDeg;
            }
       Console.WriteLine(" [angle] ((FOUT)) : " + turnDegrees);
           if (turnDegrees >-5 && turnDegrees < 5 && turnDegrees != 0){
                   Console.WriteLine(" [angle] ((GOED)) : " + turnDegrees);

          if (isAngleCalibration)
                    {
                        //isAngleCalibration = false;
                      
                        //stopCurrentTasks();

                        Console.WriteLine(turnDegrees + " [angle] Correcte angle --> Doorgaan!");

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

            colorFilter.Red = new IntRange(180, 255);
            colorFilter.Green = new IntRange(160, 255);
            colorFilter.Blue = new IntRange(180, 255);
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
                    Console.WriteLine("[line] Lijn gevonden");


                    if (isLineCalibration)
                    {
                        isLineCalibration = false;
                        await Task.Delay(700);
                        stopCurrentTasks();

                        //autopilotController.Start();
                        Console.WriteLine("[line] 700 ms gewacht ; lijnvinden gestopt");

                    }

                }
            }
            mainForm.pbVideo.Image = myBitmap;
            g.Dispose();
        }
    }
}
