using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutePlanner
{
    public class Grid
    {
        public static double INFINITY = double.MaxValue;
        private Dictionary<Position, GridPoint> gridPointMap = new Dictionary<Position, GridPoint>(); 

        public Grid(List<Position> itemsToCheck)
        {
            /*
             * Find boundary coordinates for use with graph creation by using Linq.
             * Aggregate is used here to loop through the items while keep track of the smallest x/y/z.
             */
            int xMin = itemsToCheck.Aggregate((curMin, p) => p.x < curMin.x ? p : curMin).x;
            int xMax = itemsToCheck.Aggregate((curMin, p) => p.x > curMin.x ? p : curMin).x;
            int yMin = itemsToCheck.Aggregate((curMin, p) => p.y < curMin.y ? p : curMin).y;
            int yMax = itemsToCheck.Aggregate((curMin, p) => p.y > curMin.y ? p : curMin).y;
            int zMin = itemsToCheck.Aggregate((curMin, p) => p.z < curMin.z ? p : curMin).z;
            int zMax = itemsToCheck.Aggregate((curMin, p) => p.z > curMin.z ? p : curMin).z;

            if (xMin > 0) { xMin = 0; }
            if (yMin > 0) { yMin = 0; }
            if (zMin > 0) { zMin = 0; }


            /*
             * We will need to create a grid point for every possible position within the bounds.
             * Unfortunately this will cause a massive performance drop at sufficiently high numbers because of the nested loops.
             */
            for (int x = xMin; x <= xMax; x++)
            {
                for (int y = yMin; y <= yMax; y++)
                {
                    for (int z = zMin; z <= zMax; z++)
                    {
                        Position curPos = new Position(x, y, z);
                        gridPointMap.Add(curPos, new GridPoint(curPos));
                    }
                }
            }

            //Mark destinations
            foreach(Position itemToCheck in itemsToCheck)
            {
                gridPointMap[itemToCheck].position.isTargetPosition = true;
            }

            //Connect all adjacent gridpoints with eachother
            AddAdjacencies();
        }

        private void AddAdjacencies()
        {
            foreach(Position position in gridPointMap.Keys)
            {
                //Check for adjacent gridpoints (6 sides)
                List<Position> adjacent = new List<Position>()
                        {
                            gridPointMap.Keys.FirstOrDefault(
                                p => (p.x == position.x - 1) && (p.y == position.y) && (p.z == position.z)), // Left

                            gridPointMap.Keys.FirstOrDefault(
                                p => (p.x == position.x + 1) && (p.y == position.y) && (p.z == position.z)), // Right

                            gridPointMap.Keys.FirstOrDefault(
                                p => (p.x == position.x) && (p.y == position.y - 1) && (p.z == position.z)), // Down

                            gridPointMap.Keys.FirstOrDefault(
                                p => (p.x == position.x) && (p.y == position.y + 1) && (p.z == position.z)), // Up

                            gridPointMap.Keys.FirstOrDefault(
                                p => (p.x == position.x) && (p.y == position.y) && (p.z == position.z - 1)), // Front

                            gridPointMap.Keys.FirstOrDefault(
                                p => (p.x == position.x) && (p.y == position.y) && (p.z == position.z + 1)) // Back
                        };

                foreach (Position adjPos in adjacent)
                {
                    if (adjPos != null)
                    {
                        AddEdge(position, adjPos);
                    }
                }
            }
        }

        private void AddEdge(Position source, Position destination)
        {
            GridPoint a = getGridPoint(source);
            GridPoint b = getGridPoint(destination);
            a.adjacentGridPoints.Add(new Edge(b));
        }

        //Looks for existing GridPoint in Grid
        private GridPoint getGridPoint(Position position)
        {
            return gridPointMap.FirstOrDefault(p => p.Key.Equals(position)).Value;
        }

        private void clearAll()
        {
            foreach (GridPoint gp in gridPointMap.Values)
            {
                gp.reset();
            }
        }

        public void unweighted(Position startPosition)
        {
            /*
             * Calculates shortest route to start from every other gridPoint using
             * non recursive Breadth-First Search (BFS) with a queue.
             */
            clearAll();
            GridPoint start = getGridPoint(startPosition);
            if (start == null)
            {
                throw new Exception("Start vertex not found");
            }

            Queue<GridPoint> q = new Queue<GridPoint>();
            q.Enqueue(start); //use start as root, and thus distance = 0
            start.distance = 0;
            
            //Continue as long as the queue is not empty
            while (q.Count != 0)
            {
                GridPoint v = q.Dequeue();
                foreach (Edge e in v.adjacentGridPoints)
                {
                    GridPoint w = e.destination;
                    if (w.distance == INFINITY)
                    {
                        //Set distance and add reference for use with getPath()
                        w.distance = v.distance + 1;
                        w.previous = v;
                        q.Enqueue(w); //Enqueue each edge endpoint
                    }
                }
            }
        }

        public GridPoint getNearestNeighbour(List<Position> possibleDestinations)
        {
            /*
             * Finds the nearest neighbouring destination and returns that route.
             * We put each possible destination in a list and in that list we find
             * the destination with the minimum distance.
             */
            List<GridPoint> gridPoints = new List<GridPoint>();
            foreach(Position possibleDestination in possibleDestinations)
            {
                gridPoints.Add(getGridPoint(possibleDestination));
            }
            return gridPoints.Aggregate((c, d) => c.distance < d.distance ? c : d);
        }

        public List<Position> getPath(Position destination)
        {
            List<Position> shortestPath = new List<Position>();
            GridPoint traversal = getGridPoint(destination); //Find GridPoint object tied to the Position
            while(traversal.previous != null)
            {
                //Put in the front of the list because we work from the back to the front
                shortestPath.Insert(0, traversal.position);
                traversal = traversal.previous;
            }
            return shortestPath;
        }
    }
}
