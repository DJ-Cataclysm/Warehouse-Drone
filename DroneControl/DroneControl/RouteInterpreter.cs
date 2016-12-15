﻿using DroneControl.Commands;
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


            int deltaX = 0, deltaZ = 0, y = 0;
            for (int i = 0; i < positions.Count-1; i++)
            {
                deltaX = positions[i + 1].x - positions[i].x;
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
                    for (int timesLeft = 0; timesLeft < deltaX; timesLeft++)
                    {
                        goLeft.execute();
                    }
                }

                //Enqueue vertical movement (Y-axis)
                if (y > 0 || y < 0)
                {
                    goToHeight.execute(y);
                }

                //TODO:Enqueue turns and forward movement (Z-axis)
            }
        }

        public void testRoute()
        {
            //Remove this method in release
            //Used for enqueing commands in a test environment
            headings = new Heading(autopilotController.getNavigationData().Yaw);
            //float hoogte = 1f;
            takeOffCommand.execute();
            //turn.execute(headings.right);
            goLeft.execute();
            goToHeight.execute(2f);
            goRight.execute();
            goRight.execute();
            goToHeight.execute(1f);
            //goForward.execute();
            //turn.execute(headings.back);
            //turn.execute(headings.back);
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