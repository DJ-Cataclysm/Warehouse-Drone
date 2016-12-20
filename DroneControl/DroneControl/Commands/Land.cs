namespace DroneControl.Commands
{
    public class Land : ICommand
    {
        AutopilotController controllerReference;

        public Land(ref AutopilotController controllerReference)
        {
            this.controllerReference = controllerReference;
        }

        public void execute()
        {
            controllerReference.EnqueueObjective(new AR.Drone.Avionics.Objectives.Land(2500));
        }
    }
}
