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

        public Grid(int xLowerbound, int xUpperBound, int yLowerbound, int yUpperbound, int zLowerBound, int zUpperBound)
        {
            //HashSet<GridPoint> gridPointsToBeInMap = new HashSet<GridPoint>();

            //We will need to create a grid point for every possible position within the bounds.
            for(int x = xLowerbound; x < xUpperBound; x++)
            {
                for (int y = xLowerbound; y < xUpperBound; y++)
                {
                    for (int z = xLowerbound; z < xUpperBound; z++)
                    {
                        //gridPointsToBeInMap.Add(new GridPoint(new Position(x, y, z)));
                        Position curPos = new Position(x, y, z);
                        gridPointMap.Add(curPos, new GridPoint(curPos));
                    }
                }
            }

            AddAdjacencies();
        }

        private void AddAdjacencies()
        {
            foreach(Position position in gridPointMap.Keys)
            {
                //Check for adjacent gridpoints (6 sides)
                List<Position> adjacent = new List<Position>()
                        {
                            gridPointMap.Keys.FirstOrDefault(p => (p.x == position.x - 1) && (p.y == position.y) && (p.z == position.z)), // Left
                            gridPointMap.Keys.FirstOrDefault(p => (p.x == position.x + 1) && (p.y == position.y) && (p.z == position.z)), // Right
                            gridPointMap.Keys.FirstOrDefault(p => (p.x == position.x) && (p.y == position.y - 1) && (p.z == position.z)), // Down
                            gridPointMap.Keys.FirstOrDefault(p => (p.x == position.x) && (p.y == position.y + 1) && (p.z == position.z)), // Up
                            gridPointMap.Keys.FirstOrDefault(p => (p.x == position.x) && (p.y == position.y) && (p.z == position.z - 1)), // Front
                            gridPointMap.Keys.FirstOrDefault(p => (p.x == position.x) && (p.y == position.y) && (p.z == position.z + 1)) // Back
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
            GridPoint gp;
            try
            {
                gp = gridPointMap[position];
            }
            catch (KeyNotFoundException)
            {
                gp = new GridPoint(position);
                gridPointMap[position] = gp;
            }
            return gp;
        }

        private void clearAll()
        {
            foreach (GridPoint gp in gridPointMap.Values)
            {
                gp.reset();
            }
        }

        public void printGraph()
        {
            Dictionary<Position, GridPoint>.Enumerator e = gridPointMap.GetEnumerator();

            while (e.MoveNext())
            {
                Console.WriteLine("Node {0}:", e.Current.Key);
                foreach (Edge edge in e.Current.Value.adjacentGridPoints)
                {
                    Console.WriteLine("Edge {0} -> {1}, cost: {2}", e.Current.Key, edge.destination.position);
                }
                Console.WriteLine("--------");
            }

        }


        //Calculates shortest route to start from every other gridPoint
        public void unweighted(Position startPosition)
        {
            clearAll();
            GridPoint start = gridPointMap.First(p => p.Key.Equals(startPosition)).Value;
            if (start == null)
            {
                throw new Exception("Start vertex not found");
            }

            Queue<GridPoint> q = new Queue<GridPoint>();
            q.Enqueue(start);
            start.distance = 0;

            while (q.Count != 0)
            {
                GridPoint v = q.Dequeue();
                foreach (Edge e in v.adjacentGridPoints)
                {
                    GridPoint w = e.destination;
                    if (w.distance == INFINITY)
                    {
                        w.distance = v.distance + 1;
                        w.previous = v;
                        q.Enqueue(w);
                    }
                }
            }
        }
    }
}
