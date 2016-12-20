using AR.Drone.Avionics.Objectives;
using AR.Drone.Avionics.Objectives.IntentObtainers;

namespace DroneControl.Commands
{
    /*
     * Because the execute method has a parameter, this command does not implement the ICommand interface.
     */
   public class BarcodeSmallUpDown
    {
        AutopilotController controllerReference;

        public BarcodeSmallUpDown(ref AutopilotController controllerReference)
        {
            this.controllerReference = controllerReference;
        }

        public void execute(float hoogte)
        {
            controllerReference.EnqueueObjective(
               Objective.Create(200,
                   new VelocityX(0.0f),
                   new VelocityY(0.0f),
                   new Altitude(hoogte)
               )
            );
        }
    }
}
