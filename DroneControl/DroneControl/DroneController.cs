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
        Route route;
        TaskCompletionSource<bool> flyTaskCompleted;
        TaskCompletionSource<bool> vormTaskCompleted;
        TaskCompletionSource<bool> scanTaskComleted;
        TaskCompletionSource<bool> searchBarcodeTaskCompleted;
        MainForm mainForm;
        bool isBarcodeCalibration;
        public event EventHandler<bool>eted;
        bool isLeft;
        public int droneCalibrationDirection { set; get; }
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
            // lijn vinden

            mainForm.isDroneReady = true;
            await Task.Delay(200);

            await findLine();
            mainForm.isDroneReady = false;

            routeInterpreter.shortHover.execute();
            await flyTaskCompleted.Task;

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
                mainForm.isDroneReady = true;
                await Task.Delay(200);
                if (droneCalibrationDirection != 0)
                {
                    await calibration();
                }
                mainForm.isDroneReady = false;

                //barcode calibratie
                isBarcodeCalibration = true;
                mainForm.scanningForBarcode = true;
                Console.WriteLine(isBarcodeCalibration);
                await searchForBarcode(current);
                isBarcodeCalibration = false;
                mainForm.scanningForBarcode = false;



            }
            routeInterpreter.landCommand.execute();

            // start cycle count
           
        }
        private async Task searchForBarcode(Position i)
        {

            searchBarcodeTaskCompleted = new TaskCompletionSource<bool>();
          
            //scanTaskComleted = new TaskCompletionSource<bool>();
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
            searchBarcodeTaskCompleted.SetResult(true);
        }

        public async Task findLine()
        {
            searchBarcodeTaskCompleted = new TaskCompletionSource<bool>();

            routeInterpreter.goForwardCalibration.execute();
            routeInterpreter.goBackwardsCalibration.execute();
            routeInterpreter.goBackwardsCalibration.execute();
            routeInterpreter.goForwardCalibration.execute();

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
                routeInterpreter.goBackwardsCalibration.execute();

            }
            //moet naar achter
            if (droneCalibrationDirection == -1)
            {
                routeInterpreter.goBackwardsCalibration.execute();
                routeInterpreter.goForwardCalibration.execute();
            }

            await flyTaskCompleted.Task;
            mainForm.isDroneReady = false;
    }  

        public void stopCurrentTasks(){

            autopilotController.clearObjectives();
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
            System.Windows.Forms.MessageBox.Show("Finished with smart scan!");
        }

        public void scanForBarcode()
        {
            string barcode = null;
            barcode = scanBarcode();
            if (barcode != null)
            {
                Console.WriteLine("Barcode gevoden " + barcode);
                //scanTaskComleted.SetResult(true);
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

        public async Task ScanVormen()
        {
            flyTaskCompleted = new TaskCompletionSource<bool>();
            
            Bitmap myBitmap = mainForm.getFrame();
            int angleDeg = 0;

            // lock image
            BitmapData bitmapData = myBitmap.LockBits(
                new Rectangle(0, 0, myBitmap.Width, myBitmap.Height),
                ImageLockMode.ReadWrite, myBitmap.PixelFormat);

            // step 1 - turn background to black
            ColorFiltering colorFilter = new ColorFiltering();

            colorFilter.Red = new IntRange(200, 255);
            colorFilter.Green = new IntRange(200, 255);
            colorFilter.Blue = new IntRange(200, 255);
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
            // check each object and calculate the deviation from the top of the triangle
            for (int i = 0, n = blobs.Length; i < n; i++)
            {

                List<IntPoint> edgePoints = blobCounter.GetBlobsEdgePoints(blobs[i]);
                List<IntPoint> corners;

                //Check if the blob is a triangle, if so, make Points out of the corners
                if (shapeChecker.IsTriangle(edgePoints, out corners))
                {
                    IntPoint point1 = new IntPoint(corners[1].X, corners[1].Y);
                    IntPoint point2 = new IntPoint(corners[0].X, corners[0].Y);
                    IntPoint point3 = new IntPoint(corners[2].X, corners[2].Y);
                    int middle = myBitmap.Width / 2;

                    //Find out what the top Point of the triangle is, make a Point for the middle of the bottom Vertex and create a point parallel to the drone's direction
                    //with the same length as the Vertex from the middle Point and the top Point
                    if (point1.DistanceTo(point3) > point1.DistanceTo(point2) && point2.DistanceTo(point3) > point2.DistanceTo(point1))
                    {
                        angleDeg = calculateAngle(point3, point1, point2, middle);
                    }
                    else if (point1.DistanceTo(point2) > point1.DistanceTo(point3) && point3.DistanceTo(point2) > point3.DistanceTo(point1))
                    {
                        angleDeg = calculateAngle(point2, point3, point1, middle);
                    }
                    else
                    {
                        angleDeg = calculateAngle(point1, point2, point3, middle);
                    }
                }
            }

            //If the deviation is too high, reposition
            if (angleDeg > 3)
            {
                if (isLeft)
                {
                    routeInterpreter.turn.execute(angleDeg);
                    await flyTaskCompleted.Task;
                    vormTaskCompleted.SetResult(true);
                }
                else
                {
                    routeInterpreter.turn.execute(360-angleDeg);
                    await flyTaskCompleted.Task;
                    vormTaskCompleted.SetResult(true);
                }
            }
            else
            {
                vormTaskCompleted.SetResult(true);
            }
            g.Dispose();
        }

        private int calculateAngle(IntPoint top, IntPoint left, IntPoint right, int middleView)
        {
            int angleDeg = 0;
            IntPoint triangularF;

            IntPoint middleF = new IntPoint((left.X + right.X) / 2, (left.Y + right.Y));
            double aLength = top.DistanceTo(middleF);
            if (top.Y > right.Y)
            {
                triangularF = new IntPoint(middleF.X, middleF.Y + (int)Math.Ceiling(aLength));
            }
            else
            {
                triangularF = new IntPoint(middleF.X, middleF.Y - (int)Math.Ceiling(aLength));
            }
            double cLength = top.DistanceTo(triangularF);

            //Use the known Vertexes to calculate the angle that the drone deviates from the top Point
            double aCos = ((aLength * aLength) + (aLength * aLength) - (cLength * cLength)) / ((2 * aLength) * aLength);
            double angleRad = Math.Acos(aCos);
            angleDeg = (int)Math.Ceiling(angleRad * (180 / Math.PI));
            Console.WriteLine(angleDeg + " Point 3");
            if (top.X > middleView)
                isLeft = false;
            else
                isLeft = true;

            return angleDeg;
        }
    }
}
