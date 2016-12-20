namespace DroneControl.Commands
{
   public interface ICommand
    {
        void execute(); //Used for enqueueing objectives at the AutoPilotController
    }
}
