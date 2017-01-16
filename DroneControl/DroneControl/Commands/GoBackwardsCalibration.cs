using System;
using AR.Drone.Avionics.Objectives;
using AR.Drone.Avionics.Objectives.IntentObtainers;

namespace DroneControl.Commands
{
    public class GoBackwardsCalibration : ICommand
    {
        AutopilotController controllerReference;

        public GoBackwardsCalibration(ref AutopilotController controllerReference)
        {
            this.controllerReference = controllerReference;
        }

        public void execute()
        {
            controllerReference.enqueueObjective(
                Objective.Create(4000,
                    new VelocityX(-0.16f),
                    new VelocityY(0.0f)
                )
            );
            controllerReference.enqueueObjective(new Hover(2000));
        }
    }
}
