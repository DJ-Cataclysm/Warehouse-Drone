using AR.Drone.Avionics;
using AR.Drone.Avionics.Objectives;
using AR.Drone.Client;
using AR.Drone.Data.Navigation;
using System.Threading;
using System;
namespace DroneControl
{
    public class AutopilotController
    {
        private Autopilot _autopilot;
        private DroneClient _droneClient;
        private DroneController droneController;

        public AutopilotController(DroneClient droneclient, DroneController dc)
        {

            _droneClient = droneclient;
            _autopilot = new Autopilot(_droneClient);
            _autopilot.BindToClient();
            droneController = dc;

            _autopilot.OnOutOfObjectives += volgende;
        }

        public void EnqueueObjective(Objective objective)
        {
            _autopilot.EnqueueObjective(objective);
        }

        public void Start()
        {
            _autopilot.Start();
            _autopilot.Active = true;
        }

        public void volgende()

        {

            Console.WriteLine("next------");

            droneController.setFlyTaskCompleted();
            //
            
        }
        public bool isAutopilotActive()
        {
            if (_autopilot.Active)
            {
                return true;

            }
            else { return false; }
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
    }
}
