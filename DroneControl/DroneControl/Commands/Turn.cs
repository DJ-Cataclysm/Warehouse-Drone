using AR.Drone.Avionics.Objectives;
using AR.Drone.Avionics.Objectives.IntentObtainers;

namespace DroneControl.Commands
{
    /*
     * Because the execute method has a parameter, this command does not implement the ICommand interface.
     */
    public class Turn
    {
        AutopilotController controllerReference;

        public Turn(ref AutopilotController controllerReference)
        {
            this.controllerReference = controllerReference;
        }

        public void execute(int degrees)
        {
            //1750ms for SetYaw(1f) is about 180 degrees
            float msPerDegree = 9.7222f *5;
            float velocity = 0.2f;
            if(degrees < 0)
            {
                velocity *= -1;
                degrees *= -1;
            }
            long totalTimeForManeuver = (long)(msPerDegree * degrees);
            controllerReference.enqueueObjective(
                Objective.Create(totalTimeForManeuver,
                    new VelocityX(0.0f),
                    new VelocityY(0.0f),
                    new SetYaw(velocity)
                )
            );
            controllerReference.enqueueObjective(new Hover(4000));
        }
    }
}
