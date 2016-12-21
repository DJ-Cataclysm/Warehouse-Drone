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
        Heading headings;
        BarcodeSmallUpDown barcodeSmallUpDown;
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

            int deltaX = 0, deltaZ = 0, deltaY = 0, y = 0;
            for (int i = 0; i < positions.Count-1; i++)
            {
                deltaX = positions[i + 1].x - positions[i].x;
                deltaY = positions[i + 1].y - positions[i].y;
                deltaZ = positions[i + 1].z - positions[i].z;
                y = positions[i + 1].y;

                //Enqueue horizontal movement (X-axis)
                if (deltaX > 0)
                {
                    for (int timesRight = 0; timesRight < deltaX; timesRight++)
                    {
                        goRight.execute();
                    }
                }
                else if (deltaX < 0)
                {
                    for (int timesLeft = 0; timesLeft > deltaX; timesLeft--)
                    {
                        goLeft.execute();
                    }
                }

                //Enqueue vertical movement (Y-axis)
                if ((deltaY > 0 || deltaY < 0) && y < 4 && y >= 0)
                {
                    float distanceFromGround = 0.5f;
                    goToHeight.execute(y + distanceFromGround);
                }

                //TODO:Enqueue turns and forward movement (Z-axis)
                //Currently limited to either 0 or 1
                if(deltaZ != 0)
                {
                    //Turn around and go forward
                    turn.execute(180);
                    goForward.execute();
                }
            }

            //Enqueue landing
            landCommand.execute();
        }
        public void flyToCoordinate(Position current, Position target)
        {
            int deltaX;
            int y;
            int deltaZ;

            deltaX = target.x- current.x;
            deltaZ = target.z - current.z;
            y = target.y;

            //Enqueue horizontal movement (X-axis)
            if (deltaX > 0)
            {
                for (int timesRight = 0; timesRight < deltaX; timesRight++)
                {
                    goRight.execute();
                }
            }
            else if (deltaX < 0)
            {
                for (int timesLeft = 0; timesLeft > deltaX; timesLeft--)
                {
                    goLeft.execute();
                }
            }

            //Enqueue vertical movement (Y-axis)
            if (y > 0 || y < 0)
            {
                goToHeight.execute(y);
            }
        }
        public void testRoute()
        {
            //Remove this method in release
            //Used for enqueing commands in a test environment
            //headings = new Heading(autopilotController.getNavigationData().Yaw);
            //float hoogte = 1f;
            takeOffCommand.execute();
            goLeft.execute();
            goToHeight.execute(2f);
            goRight.execute();
            goRight.execute();
            turn.execute(180);
            goForward.execute();
            goRight.execute();
            goToHeight.execute(0.5f);
            turn.execute(180);
            goForward.execute();
            landCommand.execute();

        }
   
        public void takeOffTrim()
        {
            takeOffCommand.execute();
        }
    }
}