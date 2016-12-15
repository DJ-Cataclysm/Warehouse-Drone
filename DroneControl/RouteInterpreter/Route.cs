using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutePlanner
{
    public class Route
    {
        List<Position> positions = new List<Position>();

        public void addPosition(Position p)
        {
            positions.Add(p);
        }

        public List<Position> getPositions()
        {
            return positions;
        }
    }
}
