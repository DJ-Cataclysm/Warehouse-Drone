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

namespace DroneControl
{
    public class DroneController
    {
        AutopilotController autopilotController;
        DroneClient droneClient;
        RouteInterpreter routeInterpreter;
        Route route;
        TaskCompletionSource<bool> flyTaskComleted;
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
    }
}
