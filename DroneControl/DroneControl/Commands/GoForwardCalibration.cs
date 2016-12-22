using System;
using AR.Drone.Avionics.Objectives;
using AR.Drone.Avionics.Objectives.IntentObtainers;

namespace DroneControl.Commands
{
    public class GoForwardCalibration : ICommand
    {
        AutopilotController controllerReference;

        public GoForwardCalibration(ref AutopilotController controllerReference)
        {
            this.controllerReference = controllerReference;
        }

        public void execute()
        {
            controllerReference.EnqueueObjective(
                Objective.Create(4000,
                    new VelocityX(0.16f),
                    new VelocityY(0.0f)
                )
            );
            controllerReference.EnqueueObjective(new Hover(2000));
        }
    }
}
