using AR.Drone.Client;
using AR.Drone.Client.Configuration;
using AR.Drone.Data;
using AR.Drone.Data.Navigation;
using RoutePlanner;
using System;
using System.Collections.Generic;
using System.Linq;
using WMS;

namespace DroneControl
{
    class DroneController
    {
        AutopilotController autopilotController;
        DroneClient droneClient;
        RouteInterpreter routeInterpreter;

        public DroneController()
        {
            //The IP-address is always the default gateway when connected to the drone WiFi.
            droneClient = new DroneClient("192.168.1.1"); 
            autopilotController = new AutopilotController(droneClient);
            routeInterpreter = new RouteInterpreter(ref autopilotController);
        }

        public void CycleCount()
        {
            //Route route = MakeRoute();
            //routeInterpreter.interpret(route); //Autopilot is fully setup after this
            routeInterpreter.testRoute();
            // start cycle count
            autopilotController.Start();
        }

        public void doSmartScan()
        {
            List<Position> itemsToCheck = new List<Position>()
            {
                new Position(0,0,0),
                new Position(2,0,0),
                new Position(0,2,2)
                //new Position(-19,380,-38),
                //new Position(238,380, 84)
            };


            Route r = RoutePlan.makeSmartScanRoute(itemsToCheck);
            //routeInterpreter.interpret(r);
            //start autopilot
            //autopilotController.start();
            
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
            configuration.Video.Channel = VideoChannelType.Vertical;
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
