using AR.Drone.Avionics.Objectives;
using AR.Drone.Avionics.Objectives.IntentObtainers;

namespace DroneControl.Commands
{
    class Turn
    {
        AutopilotController controllerReference;

        public Turn(ref AutopilotController controllerReference)
        {
            this.controllerReference = controllerReference;
        }

        public void execute(float heading)
        {
            controllerReference.EnqueueObjective(
                Objective.Create(6000, 
                    new VelocityX(0.0f),
                    new VelocityY(0.0f),
                    new AR.Drone.Avionics.Objectives.IntentObtainers.Heading(heading, 1f, true)
                )
            );
            controllerReference.EnqueueObjective(new Hover(2000));
        }
    }
}
