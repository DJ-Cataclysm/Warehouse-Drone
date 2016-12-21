using AR.Drone.Avionics.Objectives;
using AR.Drone.Avionics.Objectives.IntentObtainers;

namespace DroneControl.Commands
{
   public class GoLeft : ICommand
    {
        AutopilotController controllerReference;

        public GoLeft(ref AutopilotController controllerReference)
        {
            this.controllerReference = controllerReference;
        }

        public void execute()
        {
            controllerReference.EnqueueObjective(
               Objective.Create(2250,
                   new VelocityX(0.0f),
                   new VelocityY(-0.5f)
               )
            );
            controllerReference.EnqueueObjective(new Hover(5000));
        }
    }
}
