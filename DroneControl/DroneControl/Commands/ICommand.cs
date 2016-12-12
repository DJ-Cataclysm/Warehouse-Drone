namespace DroneControl.Commands
{
    interface ICommand
    {
        void execute(); //Used for enqueueing objectives at the AutoPilotController
    }
}
