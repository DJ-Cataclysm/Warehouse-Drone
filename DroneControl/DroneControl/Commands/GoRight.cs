using AR.Drone.Avionics.Objectives;
using AR.Drone.Avionics.Objectives.IntentObtainers;

namespace DroneControl.Commands
{
    class GoRight : ICommand
    {
        AutopilotController controllerReference;

        public GoRight(ref AutopilotController controllerReference)
        {
            this.controllerReference = controllerReference;
        }

        public void execute()
        {
            controllerReference.EnqueueObjective(
                Objective.Create(1250,
                    new VelocityX(0.0f),
                    new VelocityY(1.0f)
                )
            );
            controllerReference.EnqueueObjective(new Hover(2000));
        }
    }
}
