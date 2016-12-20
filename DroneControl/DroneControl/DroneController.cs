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
        Task barcodeSearching;
        MainForm mainForm;
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

            await flyTaskComleted.Task;
            routeInterpreter.shortHover.execute();
            Position currentpos = new Position(0, 1, 0);
           // barcodeSearching = searchForBarcode(currentpos);
            await searchBarcodeTaskCompleted.Task;
            await Task.Delay(200);

            for (int i = 0; i < route.getCount()-1; i++ )
            {
                flyTaskComleted = new TaskCompletionSource<bool>();
                scanTaskComleted = new TaskCompletionSource<bool>();
             //   barcodeSearching = searchForBarcode(routeList[i]);
                //eerste in de route moet takeoff zijn, pakt nu de 2e

                Position current = routeList[i];
                Position target = routeList[i+1];

                routeInterpreter.flyToCoordinate(current, target);
           //     autopilotController.Start();
             
                await flyTaskComleted.Task;
                await vormTaskCompleted.Task;
               await searchBarcodeTaskCompleted.Task;

            }
            routeInterpreter.landCommand.execute();

            // start cycle count
           
        }
        private async Task searchForBarcode(Position i)
        {
             searchBarcodeTaskCompleted = new TaskCompletionSource<bool>();
                mainForm.scanningForBarcode = true;
            scanTaskComleted = new TaskCompletionSource<bool>();
           flyTaskComleted = new TaskCompletionSource<bool>();
           float currentY = i.y;
           float factor = 1.5f;

          //omhoog 0.5
          currentY += (0.05f * factor);
          routeInterpreter.goToHeight.execute(currentY);
         routeInterpreter.shortHover.execute();
         //omlaag 1
                   currentY -= (0.1f * factor);
                   routeInterpreter.goToHeight.execute(currentY);
                    routeInterpreter.shortHover.execute();
           //omhoog 0.5
                    currentY += (0.05f * factor);
                    routeInterpreter.goToHeight.execute(currentY);
                    routeInterpreter.shortHover.execute();
         //links 1
                routeInterpreter.barcodeSmallLeft.execute(1000);
                routeInterpreter.shortHover.execute();
                //rechts2 
                         routeInterpreter.barcodeSmallRight.execute(2000);
                         routeInterpreter.shortHover.execute();
                         //links 1
                         routeInterpreter.barcodeSmallLeft.execute(1000);
                         routeInterpreter.shortHover.execute();



         
           if (scanTaskComleted.Task.IsCompleted)
           {
               routeInterpreter.shortHover.execute();
               autopilotController.Stop();
               setFlyTaskCompleted();
                searchBarcodeTaskCompleted.SetResult(true);
           }
           await flyTaskComleted.Task;
            searchBarcodeTaskCompleted.SetResult(true);
           

          
        }

        public void setFlyTaskCompleted()
        {
            flyTaskComleted.SetResult(true);
        }

        public void setVormTaskCompleted()
        {
            vormTaskCompleted.SetResult(true);
        }

        public void doSmartScan()
        {
            /*
            Route r = RoutePlan.makeSmartScanRoute();
            routeInterpreter.interpret(r);
            //start autopilot
            //autopilotController.start();
            */
        }
        public void scanForBarcode()
        {
            string barcode = null;
            barcode = scanBarcode();
            if (barcode != null)
            {
                Console.WriteLine("Barcode gevoden " + barcode);
                scanTaskComleted.SetResult(true);
                mainForm.scanningForBarcode = false;
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

        private void searchBarcode()
        {

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
        public async Task ScanVormen(Bitmap bmp)
        {
            flyTaskComleted = new TaskCompletionSource<bool>();
            
            Bitmap myBitmap = bmp;
            int angleDeg = 0;
            bool isLeft = true;

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
            Pen redPen = new Pen(Color.Red, 2);
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
                    IntPoint triangularF;

                    //Find out what the top Point of the triangle is, make a Point for the middle of the bottom Vertex and create a point parallel to the drone's direction
                    //with the same length as the Vertex from the middle Point and the top Point
                    if (point1.DistanceTo(point3) > point1.DistanceTo(point2) && point2.DistanceTo(point3) > point2.DistanceTo(point1))
                    {
                        IntPoint middleF = new IntPoint((point1.X + point2.X) / 2, (point1.Y + point2.Y));
                        double aLength = point3.DistanceTo(middleF);
                        if (point3.Y > point2.Y)
                        {
                            triangularF = new IntPoint(middleF.X, middleF.Y + (int)Math.Ceiling(aLength));
                        }
                        else
                        {
                            triangularF = new IntPoint(middleF.X, middleF.Y - (int)Math.Ceiling(aLength));
                        }
                        double cLength = point3.DistanceTo(triangularF);

                        //Use the known Vertexes to calculate the angle that the drone deviates from the top Point
                        double aCos = ((aLength * aLength) + (aLength * aLength) - (cLength * cLength)) / ((2 * aLength) * aLength);
                        double angleRad = Math.Acos(aCos);
                        angleDeg = (int)Math.Ceiling(angleRad * (180 / Math.PI));
                        Console.WriteLine(angleDeg + " Point 3");
                        if (point3.X > bmp.Width / 2)
                            isLeft = false;
                        else
                            isLeft = true;
                    }
                    else if (point1.DistanceTo(point2) > point1.DistanceTo(point3) && point3.DistanceTo(point2) > point3.DistanceTo(point1))
                    {
                        IntPoint middleF = new IntPoint((point1.X + point3.X) / 2, (point1.Y + point3.Y) / 2);
                        double aLength = point2.DistanceTo(middleF);
                        if (point2.Y > point1.Y)
                        {
                            triangularF = new IntPoint(middleF.X, middleF.Y + (int)Math.Ceiling(aLength));
                        }
                        else
                        {
                            triangularF = new IntPoint(middleF.X, middleF.Y - (int)Math.Ceiling(aLength));
                        }
                        double cLength = point2.DistanceTo(triangularF);

                        //Use the known Vertexes to calculate the angle that the drone deviates from the top Point
                        double aCos = ((aLength * aLength) + (aLength * aLength) - (cLength * cLength)) / ((2 * aLength) * aLength);
                        double angleRad = Math.Acos(aCos);
                        angleDeg = (int)Math.Ceiling(angleRad * (180 / Math.PI));
                        Console.WriteLine(angleDeg + " Point 2");
                        if (point2.X > bmp.Width / 2)
                            isLeft = false;
                        else
                            isLeft = true;
                    }
                    else
                    {
                        IntPoint middleF = new IntPoint((point2.X + point3.X) / 2, (point2.Y + point3.Y) / 2);
                        double aLength = point1.DistanceTo(middleF);
                        Console.WriteLine(aLength);
                        if (point1.Y > point2.Y)
                        {
                            triangularF = new IntPoint(middleF.X, middleF.Y + (int)Math.Ceiling(aLength));
                        }
                        else
                        {
                            triangularF = new IntPoint(middleF.X, middleF.Y - (int)Math.Ceiling(aLength));
                        }
                        double cLength = point1.DistanceTo(triangularF);

                        //Use the known Vertexes to calculate the angle that the drone deviates from the top Point
                        double aCos = ((aLength * aLength) + (aLength * aLength) - (cLength * cLength)) / ((2 * aLength) * aLength);
                        double angleRad = Math.Acos(aCos);
                        angleDeg = (int)Math.Ceiling(angleRad * (180 / Math.PI));
                        Console.WriteLine(angleDeg + " Point 1");
                        if (point1.X > bmp.Width / 2)
                            isLeft = false;
                        else
                            isLeft = true;
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

            redPen.Dispose();
            g.Dispose();
        }
    }
}
