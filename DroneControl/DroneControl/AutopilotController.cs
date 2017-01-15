﻿using AR.Drone.Avionics;
using AR.Drone.Avionics.Objectives;
using AR.Drone.Client;
using AR.Drone.Data.Navigation;

namespace DroneControl
{
    public class AutopilotController
    {
        private Autopilot _autopilot;
        private DroneClient _droneClient;

        public AutopilotController(DroneClient droneclient, DroneController dc)
        {
            _droneClient = droneclient;
            _autopilot = new Autopilot(_droneClient);
            _autopilot.BindToClient();
            _autopilot.OnOutOfObjectives += dc.setFlyTaskCompleted;
        }

        public void EnqueueObjective(Objective objective)
        {
            _autopilot.EnqueueObjective(objective);
        }

        public void Start()
        {
            _autopilot.Active = true;
            _autopilot.Start();
        }

        public void Stop()
        {
            _autopilot.Active = false;
            _autopilot.ClearObjectives();
        }

        public NavigationData getNavigationData()
        {
            return _droneClient.NavigationData;
        }

        public void clearObjectives()
        {
            _autopilot.ClearObjectives();
        }
    }
}
