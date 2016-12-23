﻿using RoutePlanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutePlanner
{
    public static class RoutePlan
    {
        public static Route makeFullCycleRoute()
        {
  
        // get list of positions
        Route returnRoute = new Route();
        Position beginPos = new Position(0, 0, 0);
        returnRoute.addPosition(beginPos);

        List<Position> positions = Positions.getPositions();
        
       
        //get maximum height
        if (positions.Count == 0)
        {
            throw new InvalidOperationException("Empty list");
        }
        int yAsMax = -1;
        foreach (Position pos in positions)
        {
            if (pos.y >yAsMax)
            {
                yAsMax = pos.y; 
            }
        }

        //sort the list so that each row gets reversed
        bool reverse = true;
        for (int i = 0; i <= yAsMax; i++)
        {
            List<Position> templist = new List<Position>();
            foreach (Position p in positions)
            {
                if( p.y == i){
                    templist.Add(p);
                }
                
            }
            if (!reverse){

                templist = templist.OrderBy(Position => Position.x).ToList(); // van klein naar groot
                foreach (Position p in templist)
                {
                    Console.Write(p.x.ToString() + " ");
                }
            }else{
             templist = templist.OrderByDescending(Position => Position.x).ToList(); // van groot naar klein 
            
        
         
            }
            foreach(Position p in templist){
                returnRoute.addPosition(p);
                Console.Write("pos x: " + p.x.ToString() + "pos y:" + p.y.ToString() + " ---");
            }

            reverse = !reverse;

        }
     
            return returnRoute;
        }

        public static Route makeSmartScanRoute(List<Position> itemsToCheck)
        {
            /*
             * Find boundary coordinates for use with graph creation by using Linq.
             * Aggregate is used here to loop through the items while keep track of the smallest x/y/z.
             */
            int xLowerBound = itemsToCheck.Aggregate((curMin, p) => p.x < curMin.x ? p : curMin).x;
            int xUpperBound = itemsToCheck.Aggregate((curMin, p) => p.x > curMin.x ? p : curMin).x;
            int yLowerBound = itemsToCheck.Aggregate((curMin, p) => p.y < curMin.y ? p : curMin).y;
            int yUpperBound = itemsToCheck.Aggregate((curMin, p) => p.y > curMin.y ? p : curMin).y;
            int zLowerBound = itemsToCheck.Aggregate((curMin, p) => p.z < curMin.z ? p : curMin).z;
            int zUpperBound = itemsToCheck.Aggregate((curMin, p) => p.z > curMin.z ? p : curMin).z;

            if (xLowerBound > 0) { xLowerBound = 0; }
            if (yLowerBound > 0) { yLowerBound = 0; }
            if (zLowerBound > 0) { zLowerBound = 0; }

            //Create grid within bounds
            Grid grid = new Grid(
                xLowerBound, 
                xUpperBound, 
                yLowerBound, 
                yUpperBound, 
                zLowerBound, 
                zUpperBound, 
                itemsToCheck
            );

            //Add drone starting point and endpoint
            Position startAndEndpoint = new Position(0, 0, 0);
            Position startPoint = startAndEndpoint;
            GridPoint nearestNeighbour;

            //Keep creating routes between startPoint and nearestNeighbour until there are no new items to check.
            Route route = new Route();
            route.addPosition(startPoint);
            while (itemsToCheck.Count >= 1)
            {
                grid.unweighted(startPoint); //Calculate all distances between startPoint and other points
                itemsToCheck.RemoveAll(pos => pos.Equals(startPoint));
                if(itemsToCheck.Count == 0) { break; }
                nearestNeighbour = grid.getNearestNeighbour(itemsToCheck);
                route.addPositions(grid.getPath(nearestNeighbour.position)); //Add to route
                
                startPoint = nearestNeighbour.position;
            }

            //This last step is required to return to (0,0,0)
            grid.unweighted(startPoint);
            route.addPositions(grid.getPath(startAndEndpoint));

            return route;
        }
    }
}
