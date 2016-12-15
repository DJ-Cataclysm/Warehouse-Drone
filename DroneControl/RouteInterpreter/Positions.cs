using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutePlanner
{
   public static class Positions 
    {
      static List<Position> positions = new List<Position>();

      public static void addPosition(Position p)
       {
           positions.Add(p);
       }
       public static List<Position> getPositions()
       {
           return positions;
       }
   
    }
}
