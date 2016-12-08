using DroneControl.Commands;
using RoutePlanner;
using System.Collections.Generic;

namespace DroneControl
{
    public class RouteInterpreter
    {
        ICommand goForward, goLeft, goRight, landCommand, takeOffCommand;
        Turn turn;
        Heading headings;
        GoToHeight goToHeight;
        AutopilotController autopilotController;

        public RouteInterpreter(ref AutopilotController autopilotController)
        {
            this.autopilotController = autopilotController;
            goLeft = new GoLeft(ref autopilotController);
            goRight = new GoRight(ref autopilotController);
            goForward = new GoForward(ref autopilotController);
            goToHeight = new GoToHeight(ref autopilotController);
            landCommand = new Land(ref autopilotController);
            takeOffCommand = new TakeOff(ref autopilotController);
            turn = new Turn(ref autopilotController);
        }

        public void interpret(Route route)
        {
            List<Position> positions = route.getPositions();
            /*
             * bla bla for each position in route enqueue appropriate command
             */
        }

        public void testRoute()
        {
            //Remove this method in release
            //Used for enqueing commands in a test environment
            headings = new Heading(autopilotController.getNavigationData().Yaw);
            //float hoogte = 1f;
            takeOffCommand.execute();
            //turn.execute(headings.right);
            //goLeft.execute();
            //goRight.execute();
            //goForward.execute();
            //turn.execute(headings.back);
            turn.execute(headings.back);
            //goForward.execute();
            //turn.execute(headings.back);
            //goForward.execute();
            //turn.execute(headings.right);
            //goForward.execute();
            //turn.execute(headings.front);
            //turn.execute(headings.front);
            landCommand.execute();

        }
    }
}