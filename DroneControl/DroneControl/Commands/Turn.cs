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
            float msPerDegree = 9.7222f;
            float velocity = 1f;
            if(degrees < 0)
            {
                velocity *= -1;
            }
            long totalTimeForManeuver = (long)(msPerDegree * degrees);
            controllerReference.EnqueueObjective(
                Objective.Create(totalTimeForManeuver,
                    new VelocityX(0.0f),
                    new VelocityY(0.0f),
                    new SetYaw(velocity)
                    //new AR.Drone.Avionics.Objectives.IntentObtainers.Heading(heading, 1f, true)
                )
            );
            controllerReference.EnqueueObjective(new Hover(4000));
        }
    }
}
