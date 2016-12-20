using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutePlanner
{
    public class Edge
    {
        /*
         * An edge connects two gridpoints to eachother
         * In our grid system they are connected in a lattice like this:
         * 
         *      *----*
         *      |    |
         *      *----*
         * 
         * This means no diagonal edges.
         */
        public GridPoint destination;
        
        public Edge(GridPoint destination)
        {
            this.destination = destination;
        }
    }
}
