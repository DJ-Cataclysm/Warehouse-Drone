using AR.Drone.Avionics.Objectives;
using AR.Drone.Avionics.Objectives.IntentObtainers;

namespace DroneControl.Commands
{
    public class BarcodeSmallRight
    {
        AutopilotController controllerReference;

        public BarcodeSmallRight(ref AutopilotController controllerReference)
        {
            this.controllerReference = controllerReference;
        }

        public void execute(long time)
        {
            controllerReference.EnqueueObjective(
                Objective.Create(time,
                    new VelocityX(0.0f),
                    new VelocityY(0.2f)
                )
            );
    
        }
    }
}
