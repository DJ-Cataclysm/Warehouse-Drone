using AR.Drone.Avionics.Objectives;
using AR.Drone.Avionics.Objectives.IntentObtainers;

namespace DroneControl.Commands
{
    public class GoRight : ICommand
    {
        AutopilotController controllerReference;

        public GoRight(ref AutopilotController controllerReference)
        {
            this.controllerReference = controllerReference;
        }

        public void execute()
        {
            controllerReference.enqueueObjective(
                Objective.Create(2000,
                    new VelocityX(0.0f),
                    new VelocityY(0.5f)
                )
            );
            controllerReference.enqueueObjective(new Hover(5000));
        }
    }
}
