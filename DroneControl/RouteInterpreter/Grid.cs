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
             * We will need to create a grid point for every possible position within the bounds.
             * Unfortunately this will cause a massive performance drop at sufficiently high numbers 
             * because of the nested loops in this method.
             */
            int[] boundary = findBoundary(itemsToCheck);
            initializeGrid(boundary);

            //Mark destinations
            foreach(Position itemToCheck in itemsToCheck)
            {
                gridPointMap[itemToCheck].position.isTargetPosition = true;
            }

            //Connect all adjacent gridpoints with eachother
            addAdjacencies();
        }

        private int[] findBoundary(List<Position> itemsToCheck)
        {
            /*
             * Find boundary coordinates for use with graph creation by using Linq.
             * Aggregate is used here to loop through the items while keep track of the smallest x/y/z.
             * CC is 22, but it does not really make sense to split this method up any further.
             * Also contains magic index numbers, but since they are declared in the int[] boundary
             * We'll allow it.
             */
            int[] boundary = new int[]
            {
                itemsToCheck.Aggregate((curMin, p) => p.x < curMin.x ? p : curMin).x, //index = 0
                itemsToCheck.Aggregate((curMin, p) => p.x > curMin.x ? p : curMin).x, //index = 1
                itemsToCheck.Aggregate((curMin, p) => p.y < curMin.y ? p : curMin).y, //index = 2
                itemsToCheck.Aggregate((curMin, p) => p.y > curMin.y ? p : curMin).y, //index = 3
                itemsToCheck.Aggregate((curMin, p) => p.z < curMin.z ? p : curMin).z, //index = 4
                itemsToCheck.Aggregate((curMin, p) => p.z > curMin.z ? p : curMin).z  //index = 5
            };

            //Make sure the starting position is always within boundary
            if (boundary[0] > 0) { boundary[0] = 0; } 
            if (boundary[2] > 0) { boundary[2] = 0; }
            if (boundary[4] > 0) { boundary[4] = 0; }

            return boundary;
        }

        private void initializeGrid(int[] boundary)
        {
            /*
             * Create all position objects within boundary and add to gridPointMap
             * See findBoundary() for an definition of the magic index numbers.
             */
            for (int x = boundary[0]; x <= boundary[1]; x++) //xMin to xMax
            {
                for (int y = boundary[2]; y <= boundary[3]; y++) //yMin to yMax
                {
                    for (int z = boundary[4]; z <= boundary[5]; z++) //zMin to zMax
                    {
                        Position curPos = new Position(x, y, z);
                        gridPointMap.Add(curPos, new GridPoint(curPos));
                    }
                }
            }
        }

        private void addAdjacencies()
        {
            //Find and add all edges a position should have.
            foreach(Position position in gridPointMap.Keys)
            {
                List<Position> adjacent = getAdjacentPositions(position);

                foreach (Position adjPos in adjacent)
                {
                    if (adjPos != null)
                    {
                        AddEdge(position, adjPos);
                    }
                }
            }
        }
        
        private List<Position> getAdjacentPositions(Position position)
        {
            /*
             * Check for adjacent gridpoints (on all 6 sides) and returns those.
             * Even though the CC is 19, it does not really make sense to split this method up any further.
             */
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
            return adjacent;
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
            //Reset every gridpoint before doing a Breadth-First Search
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
            //Traverse through the path using a while loop, working from back to front.
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
