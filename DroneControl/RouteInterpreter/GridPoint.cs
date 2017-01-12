using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutePlanner
{
    public class GridPoint
    {
        public Position position;
        public List<Edge> adjacentGridPoints;
        public GridPoint previous; //Used in path traversal
        public double distance; //Distance between this GridPoint startPoint
        
        public GridPoint(Position position)
        {
            this.position = position;
            adjacentGridPoints = new List<Edge>();
            reset();
        }

        public void reset()
        {
            previous = null;
            distance = Grid.INFINITY;
        }
    }
}
