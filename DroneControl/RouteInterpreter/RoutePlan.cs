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
        /*
         * Do fancy shortest-path algorithms here
         */

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

        public static Route makeSmartScanRoute()
        {
            //TODO: use WMS input and create shortest paths to each required grid point
            return null;
        }
    }
}
