using AR.Drone.Avionics;
using AR.Drone.Avionics.Objectives;
using AR.Drone.Client;

namespace DroneControl
{
    public class AutopilotController
    {
        private Autopilot _autopilot;

        public AutopilotController(DroneClient droneClient, DroneController droneController)
        {
            _autopilot = new Autopilot(droneClient);
            _autopilot.BindToClient();
            _autopilot.OnOutOfObjectives += droneController.setFlyTaskCompleted;
        }

        public void enqueueObjective(Objective objective)
        {
            _autopilot.EnqueueObjective(objective);
        }

        public void start()
        {
            _autopilot.Active = true;
            _autopilot.Start();
        }

        public void stop()
        {
            _autopilot.Active = false;
            _autopilot.ClearObjectives();
            _autopilot.Stop();
        }

        public void clearObjectives()
        {
            _autopilot.ClearObjectives();
        }
    }
}
