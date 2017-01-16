using System;
using AR.Drone.Avionics.Objectives;
using AR.Drone.Avionics.Objectives.IntentObtainers;

namespace DroneControl.Commands
{
    public class FlyToOtherRack : ICommand
    {
        AutopilotController controllerReference;

        public FlyToOtherRack(ref AutopilotController controllerReference)
        {
            this.controllerReference = controllerReference;
        }

        public void execute()
        {
            controllerReference.enqueueObjective(
                Objective.Create(7000,
                    new VelocityX(0.2f),
                    new VelocityY(0.0f)
                )
            );
            controllerReference.enqueueObjective(new Hover(2000));
        }
    }
}
