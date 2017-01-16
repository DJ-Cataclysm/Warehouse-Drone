using AR.Drone.Avionics.Objectives;
using AR.Drone.Avionics.Objectives.IntentObtainers;

namespace DroneControl.Commands
{
   public  class TakeOff : ICommand
    {
        AutopilotController controllerReference;

        public TakeOff(ref AutopilotController controllerReference)
        {
            this.controllerReference = controllerReference;
        }

        public void execute()
        {
            controllerReference.enqueueObjective(new FlatTrim(500));
            controllerReference.enqueueObjective(new Takeoff(5000));
            controllerReference.enqueueObjective(new Hover(2000));
        }
    }
}
