using DroneControl.Commands;
using RoutePlanner;
using System;
using System.Collections.Generic;

namespace DroneControl
{
    public class RouteInterpreter
    {
        public ICommand goForward, goLeft, goRight, landCommand, takeOffCommand, shortHover;
        public GoToHeight goToHeight;
        public BarcodeSmallLeft barcodeSmallLeft;
        public BarcodeSmallRight barcodeSmallRight;
        public Turn turn;
        BarcodeSmallUpDown barcodeSmallUpDown;
        AutopilotController autopilotController;
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
            barcodeSmallUpDown = new BarcodeSmallUpDown(ref autopilotController);
            shortHover = new ShortHover(ref autopilotController);


            turn = new Turn(ref autopilotController);
        }
        
        public void interpret(Route route)
        {
            //Enqueue takeoff
            takeOffCommand.execute();

            List<Position> positions = new List<Position>();
            positions.Add(new Position(0, 0, 0));
            positions.AddRange(route.getPositions());

            for (int i = 0; i < positions.Count-1; i++)
            {
                flyToCoordinate(positions[i], positions[i + 1]);
            }

            //Enqueue landing
            landCommand.execute();
        }

        public void flyToCoordinate(Position current, Position target)
        {
            int deltaX = target.x - current.x;
            int deltaY = target.y - current.y;
            int deltaZ = target.z - current.z;

            //Enqueue horizontal movement (X-axis)
            if (deltaX > 0)
            {
                for (int timesRight = 0; timesRight < deltaX; timesRight++)
                {
                    if (facingDirection)
                    {
                        goRight.execute();
                    }
                    else
                    {
                        goLeft.execute();
                    }

                }
            }
            else if (deltaX < 0)
            {
                for (int timesLeft = 0; timesLeft > deltaX; timesLeft--)
                {
                    if (facingDirection)
                    {
                        goLeft.execute();
                    }
                    else
                    {
                        goRight.execute();
                    }
                }
            }

            //Enqueue vertical movement (Y-axis)
            if ((deltaY > 0 || deltaY < 0) && target.y < 4 && target.y >= 0)
            {
                float distanceFromGround = 0.2f;
                goToHeight.execute(target.y + distanceFromGround);
            }

            //Currently limited to either 0 or 1
            if (deltaZ != 0)
            {
                //Turn around and go forward
                turn.execute(180);
                goForward.execute();

                //Set facingDirection to true or false by even or uneven Z coordinate
                facingDirection = (target.z % 2 == 0); //When false the strafe directions are inverted
            }
        }
        public void testRoute()
        {
            //Remove this method in release
            //Used for enqueing commands in a test environment
            //headings = new Heading(autopilotController.getNavigationData().Yaw);
            //float hoogte = 1f;
            takeOffCommand.execute();
            goToHeight.execute(1.20f);
            //goLeft.execute();
            //goToHeight.execute(2.20f);
            //goToHeight.execute(1.20f);
            //turn.execute(180);
            //goForward.execute();
            landCommand.execute();
        }
    }
}