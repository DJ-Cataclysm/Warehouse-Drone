using AR.Drone.Avionics.Objectives;
using AR.Drone.Avionics.Objectives.IntentObtainers;

namespace DroneControl.Commands
{
    /*
     * Because the execute method has a parameter, this command does not implement the ICommand interface.
     */
    class GoToHeight
    {
        AutopilotController controllerReference;

        public GoToHeight(ref AutopilotController controllerReference)
        {
            this.controllerReference = controllerReference;
        }

        public void execute(float hoogte)
        {
            controllerReference.EnqueueObjective(
               Objective.Create(4000,
                   new VelocityX(0.0f),
                   new VelocityY(0.0f),
                   new Altitude(hoogte)
               )
            );
            controllerReference.EnqueueObjective(new Hover(2000));
        }
    }
}
