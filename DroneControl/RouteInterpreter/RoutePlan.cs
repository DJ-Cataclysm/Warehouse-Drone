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
        public static List<Position> makeCycleCountRoute(List<Position> itemsToCheck)
        {
            /*
             * Create a cycle count route using a zig-zag algorithm.
             * First find all the unique Z coordinates, then find maximum Y value for coordinates with specified Z.
             * In a loop, order all positions matching Y and Z, either ascending or descending depending on direction.
             * Then add those positions to the final route. Each iteration we reverse the direction.
             * We end up with a List ordered in such a way that the RouteInterpreter can plot a course through 
             * the whole warehouse and scan every position.
             */

            if (itemsToCheck.Count == 0)
            {
                throw new InvalidOperationException("Empty list");
            }

            List<Position> route = new List<Position>();
            route.Add(new Position(0, 0, 0)); //Add beginning position

            List<int> allZCoords = itemsToCheck.Select(p => p.z).Distinct().ToList(); //Find all distinct z values
            allZCoords.Sort(); //Sort this list ascending if not already done

            foreach(int z in allZCoords)
            {
                //get maximum y value within the current z coordinate.
                int yAxisMax = itemsToCheck.Aggregate((curMin, p) => p.y > curMin.y  && p.z == z? p : curMin).y; 

                //Sort the list so that each row gets reversed
                bool reverse = true;
                for (int i = 0; i <= yAxisMax; i++)
                {
                    List<Position> positionsToAdd = new List<Position>();

                    //Add positions whose y value matches i, and whose z value matches current z coordinate.
                    positionsToAdd.AddRange(itemsToCheck.Where(p => p.y == i && p.z == z)); 

                    if (reverse)
                    {
                        positionsToAdd = positionsToAdd.OrderByDescending(Position => Position.x).ToList(); // descending 
                    }
                    else
                    {
                        positionsToAdd = positionsToAdd.OrderBy(Position => Position.x).ToList(); // ascending
                    }

                    route.AddRange(positionsToAdd);

                    reverse = !reverse;
                }
            }

            return route;
        }

        public static List<Position> makeSmartScanRoute(List<Position> itemsToCheck)
        {
           
            Grid grid = new Grid(itemsToCheck);

            //Add drone starting point and endpoint
            Position startAndEndpoint = new Position(0, 0, 0);
            Position startPoint = startAndEndpoint;
            GridPoint nearestNeighbour;

            //Keep creating routes between startPoint and nearestNeighbour until there are no new items to check.
            List<Position> route = new List<Position>();
            route.Add(startPoint);
            while (itemsToCheck.Count >= 1)
            {
                grid.unweighted(startPoint); //Calculate all distances between startPoint and other points
                itemsToCheck.RemoveAll(pos => pos.Equals(startPoint));
                if(itemsToCheck.Count == 0) { break; }
                nearestNeighbour = grid.getNearestNeighbour(itemsToCheck);
                //Add path from startPoint to nearestNeighbour
                route.AddRange(grid.getPath(nearestNeighbour.position)); //TODO: kan getPath niet beter een GridPoint als parameter hebben?
                
                startPoint = nearestNeighbour.position;
            }

            //This last step is required to return to (0,0,0)
            grid.unweighted(startPoint);
            route.AddRange(grid.getPath(startAndEndpoint));

            return route;
        }
    }
}
