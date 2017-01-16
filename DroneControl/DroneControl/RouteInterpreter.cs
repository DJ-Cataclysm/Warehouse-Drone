using DroneControl.Commands;
using RoutePlanner;
using System;

namespace DroneControl
{
    public class RouteInterpreter
    {
        public ICommand goForward, goLeft, goRight, landCommand, takeOffCommand, shortHover, goBackwardsCalibration, goForwardCalibration, flyToOtherRack;
        public GoToHeight goToHeight;
        public BarcodeSmallLeft barcodeSmallLeft;
        public BarcodeSmallRight barcodeSmallRight;
        public Turn turn;
        AutopilotController autopilotController;
        const float DISTANCE_FROM_GROUND = 0.2f;
        const int MINIMUM_HEIGHT = 0;
        const int MAXIMUM_HEIGHT = 4;
        bool facingDirection = true; //If false, strafing left and right are switched

        public RouteInterpreter(ref AutopilotController autopilotController)
        {
            this.autopilotController = autopilotController;
            goLeft = new GoLeft(ref autopilotController);
            goRight = new GoRight(ref autopilotController);
            goForward = new GoForward(ref autopilotController);
            goToHeight = new GoToHeight(ref autopilotController);
            landCommand = new Land(ref autopilotController);
            takeOffCommand = new TakeOff(ref autopilotController);
            barcodeSmallLeft = new BarcodeSmallLeft(ref autopilotController);
            barcodeSmallRight = new BarcodeSmallRight(ref autopilotController);
            shortHover = new ShortHover(ref autopilotController);
            goBackwardsCalibration = new GoBackwardsCalibration(ref autopilotController);
            goForwardCalibration = new GoForwardCalibration(ref autopilotController);
            flyToOtherRack = new FlyToOtherRack(ref autopilotController);
            turn = new Turn(ref autopilotController);
        }

        public void flyToCoordinate(Position current, Position target)
        {
            int deltaX = target.x - current.x;
            int deltaY = target.y - current.y;

            //Enqueue horizontal movement (X-axis)
            flyHorizontal(deltaX);

            //Enqueue vertical movement (Y-axis), must be in range of 0 meters and 4.
            flyVertical(deltaY, target.y);

            //Z-axis movement is handled in DroneControl.
        }

        private void flyHorizontal(int deltaX)
        {
            if (deltaX != 0)
            {
                for (int i = 0; i < Math.Abs(deltaX); i++)
                {
                    if (facingDirection && deltaX > 0)
                    {
                        goRight.execute();
                    }
                    else if (!facingDirection && deltaX > 0)
                    {
                        goLeft.execute();
                    }
                    else if (facingDirection)
                    {
                        goLeft.execute();
                    }
                    else
                    {
                        goRight.execute();
                    }
                }
            }
        }

        private void flyVertical(int deltaY, int targetY)
        {
            if (deltaY != 0 && targetY < MAXIMUM_HEIGHT && targetY >= MINIMUM_HEIGHT)
            {
                goToHeight.execute(targetY + DISTANCE_FROM_GROUND);
            }
        }
    }
}