using AR.Drone.Avionics.Objectives;
using AR.Drone.Avionics.Objectives.IntentObtainers;

namespace DroneControl.Commands
{
   public class ShortHover : ICommand
    {
        AutopilotController controllerReference;

        public ShortHover(ref AutopilotController controllerReference)
        {
            this.controllerReference = controllerReference;
        }

        public void execute()
        {
            controllerReference.enqueueObjective(new Hover(700));
        }
    }
}
