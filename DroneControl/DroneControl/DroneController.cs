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
        TaskCompletionSource<bool> flyTaskComleted;
        TaskCompletionSource<bool> vormTaskCompleted;
        TaskCompletionSource<bool> scanTaskComleted;
        TaskCompletionSource<bool> searchBarcodeTaskCompleted;
        MainForm mainForm;
        bool isBarcodeCalibration;
        public event EventHandler<bool>eted;
        bool isLeft;
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
            flyTaskComleted = new TaskCompletionSource<bool>();
            vormTaskCompleted = new TaskCompletionSource<bool>();

            route = MakeRoute();
            List<Position> routeList = route.getPositions();
            autopilotController.Start();
            routeInterpreter.takeOffTrim();
          
            routeInterpreter.shortHover.execute();
            //Position currentpos = new Position(0, 1, 0);
            // searchForBarcode(currentpos);
            //await searchBarcodeTaskCompleted.Task
            await flyTaskComleted.Task;
            for (int i = 0; i < route.getCount()-1; i++ )
            {
                Console.Write(" ga door met loop");
                //if (!autopilotController.isAutopilotActive())
                //{
                //    autopilotController.Start();
                //    Console.Write("Starting Autopilot");
                //}
                flyTaskComleted = new TaskCompletionSource<bool>();
                //scanTaskComleted = new TaskCompletionSource<bool>();
             //   barcodeSearching = searchForBarcode(routeList[i]);
                //eerste in de route moet takeoff zijn, pakt nu de 2e

                Position current = routeList[i];
                Position target = routeList[i+1];

                routeInterpreter.flyToCoordinate(current, target);
           //     autopilotController.Start();

                Console.Write("flying to target");
                await flyTaskComleted.Task;
                isBarcodeCalibration = true;
                mainForm.scanningForBarcode = true;
                Console.WriteLine(isBarcodeCalibration);
                await searchForBarcode(current);
                isBarcodeCalibration = false;
                mainForm.scanningForBarcode = false;
                //await vormTaskCompleted.Task;
                //await searchBarcodeTaskCompleted.Task;

            }
            routeInterpreter.landCommand.execute();

            // start cycle count
           
        }
        private async Task searchForBarcode(Position i)
        {

            searchBarcodeTaskCompleted = new TaskCompletionSource<bool>();
          
            //scanTaskComleted = new TaskCompletionSource<bool>();
            flyTaskComleted = new TaskCompletionSource<bool>();

            routeInterpreter.barcodeSmallLeft.execute(1000);
            routeInterpreter.shortHover.execute();
            //rechts2 
            routeInterpreter.barcodeSmallRight.execute(2000);
            routeInterpreter.shortHover.execute();
            //links 1
            routeInterpreter.barcodeSmallLeft.execute(1000);
            routeInterpreter.shortHover.execute();
           await flyTaskComleted.Task;
            searchBarcodeTaskCompleted.SetResult(true);
           

          
        }

        public void setFlyTaskCompleted()
        {
       
            flyTaskComleted.TrySetResult(true);
        }

            

        public void setVormTaskCompleted()
        {
            vormTaskCompleted.SetResult(true);
        }

        public void doSmartScan()
        {
            //TODO: Put items from WMS here
            List<Position> itemsToCheck = new List<Position>()
            {
                new Position(2,0,0),
                new Position(0,200,1),
                new Position(0,50,10),
                new Position(0,89,2),
                new Position(0,135,10),
                new Position(0,78,10),
                new Position(0,50,10),
                new Position(1,20,5),
                new Position(0,50,10),
                new Position(0,5,2)
            };
            Route r = RoutePlan.makeSmartScanRoute(itemsToCheck);
            routeInterpreter.interpret(r);
            //start autopilot
            //autopilotController.start();
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
                    setFlyTaskCompleted();
                    routeInterpreter.shortHover.execute();
                    autopilotController.Stop();
                    autopilotController.Start();
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

            var configuration = new Settings();
            configuration.Video.Channel = VideoChannelType.Horizontal;
            droneClient.Send(configuration);
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

        private Route MakeRoute()
        {
            //TODO: make either a smart route or a full cycle count route

            //Console.Write(" clicked the make route button - making route");
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

            /*List<Position> a = r.getPositions();
            foreach (Position X in a)
            {
                Console.Write("position --> X: " + X.x.ToString() + " Y: " + X.y.ToString());
                Console.WriteLine();

            }*/
        }
        public async Task ScanVormen()
        {
            flyTaskComleted = new TaskCompletionSource<bool>();
            
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
                    await flyTaskComleted.Task;
                    vormTaskCompleted.SetResult(true);
                }
                else
                {
                    routeInterpreter.turn.execute(360-angleDeg);
                    await flyTaskComleted.Task;
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
