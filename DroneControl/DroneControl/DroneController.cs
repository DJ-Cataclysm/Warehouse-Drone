﻿using AR.Drone.Client;
using AR.Drone.Data;
using AR.Drone.Data.Navigation;
using System;

namespace DroneControl
{
    class DroneController
    {
        AutopilotController autopilotController;
        DroneClient droneClient;
        RouteInterpreter routeInterpreter;

        public DroneController()
        {
            droneClient = new DroneClient("192.168.1.1");
            autopilotController = new AutopilotController(droneClient);
            routeInterpreter = new RouteInterpreter(ref autopilotController);
        }

        public void doFullCycle()
        {
            /*
            Route r = RoutePlan.makeFullCycleRoute();
            routeInterpreter.interpret(r); //route interpreter enqueues the 
            //start autopilot
            //autopilotController.start();
            */
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

        public void attachEventHandlers(Action<NavigationPacket> navPacketHandler, Action<VideoPacket> videoPacketHandler, Action<NavigationData> navDataHandler)
        {
            droneClient.NavigationPacketAcquired += navPacketHandler;
            droneClient.VideoPacketAcquired += videoPacketHandler;
            droneClient.NavigationDataAcquired += navDataHandler;
        }

        public void Dispose()
        {
            droneClient.Dispose();
        }
    }
}
