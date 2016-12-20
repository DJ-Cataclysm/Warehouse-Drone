using RoutePlanner;
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
        bool reverse = false;
        for (int i = 0; i <= yAsMax; i++)
        {
            List<Position> templist = new List<Position>();
            foreach (Position p in positions)
            {
                if( p.y == i){
                    templist.Add(p);
                }
                
            }
            if (reverse == false){

                templist = templist.OrderBy(Position => Position.x).ToList(); // van klein naar groot
                foreach (Position p in templist)
                {
                    Console.Write(p.x.ToString() + " ");
                }
            }else{
             templist = templist.OrderByDescending(Position => Position.x).ToList(); // van groot naar klein 
            
        
            foreach (Position p in templist)
            {
                Console.Write(p.x.ToString() + " ");
            }
            }
            foreach(Position p in templist){
                returnRoute.addPosition(p);
            }

            if (reverse == true)
            {
                reverse = false;
            }
            else { reverse = true; }

        }

            return returnRoute;
        }

        public static Route makeSmartScanRoute(List<Position> itemsToCheck)
        {
            //Find boundary coordinates for use with graph creation by using linq
            int xLowerBound = itemsToCheck.Aggregate((curMin, p) => p.x < curMin.x ? p : curMin).x;
            int xUpperBound = itemsToCheck.Aggregate((curMin, p) => p.x > curMin.x ? p : curMin).x;
            int yLowerBound = itemsToCheck.Aggregate((curMin, p) => p.y < curMin.y ? p : curMin).y;
            int yUpperBound = itemsToCheck.Aggregate((curMin, p) => p.y > curMin.y ? p : curMin).y;
            int zLowerBound = itemsToCheck.Aggregate((curMin, p) => p.z < curMin.z ? p : curMin).z;
            int zUpperBound = itemsToCheck.Aggregate((curMin, p) => p.z > curMin.z ? p : curMin).z;

            Grid grid = new Grid(xLowerBound, xUpperBound, yLowerBound, yUpperBound, zLowerBound, zUpperBound);


            //Something something foreach item to check do unweighted then get path
            grid.unweighted(itemsToCheck[0]);


            //TODO: Tie paths together to create smart route

            return null;
        }
    }
}
