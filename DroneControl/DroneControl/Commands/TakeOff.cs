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
            controllerReference.EnqueueObjective(new FlatTrim(500));
            controllerReference.EnqueueObjective(new Takeoff(5000));
            /* controllerReference.EnqueueObjective(
                Objective.Create(5000,
                    new VelocityX(0.0f),
                    new VelocityY(0.0f),
                    new Altitude(0.5f)
                )
            );*/
            controllerReference.EnqueueObjective(new Hover(2000));
        }
    }
}
