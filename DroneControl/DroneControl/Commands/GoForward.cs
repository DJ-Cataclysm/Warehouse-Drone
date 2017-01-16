using System;
using AR.Drone.Avionics.Objectives;
using AR.Drone.Avionics.Objectives.IntentObtainers;

namespace DroneControl.Commands
{
    public class GoForward : ICommand
    {
        AutopilotController controllerReference;

        public GoForward(ref AutopilotController controllerReference)
        {
            this.controllerReference = controllerReference;
        }

        public void execute()
        {
            controllerReference.enqueueObjective(
                Objective.Create(1250,
                    new VelocityX(1.0f),
                    new VelocityY(0.0f)
                )
            );
            controllerReference.enqueueObjective(new Hover(2000));
        }
    }
}
