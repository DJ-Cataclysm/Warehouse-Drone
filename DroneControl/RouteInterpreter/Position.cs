using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutePlanner
{
    public class Position
    {
        public int x, y, z;
        //This boolean determines if this position should be scanned for barcodes
        public bool isTargetPosition;

        public Position(int x, int y, int z, bool isTargetPosition = false)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.isTargetPosition = isTargetPosition; 
        }

        public override bool Equals(object obj)
        {
            var position = obj as Position;

            if(position == null)
            {
                return false;
            }

            return (x == position.x && y == position.y && z == position.z);
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + x.GetHashCode();
            hash = (hash * 7) + y.GetHashCode();
            hash = (hash * 7) + z.GetHashCode();
            return hash;
        }


        public override string ToString()
        {
            return string.Format("({0},{1},{2})", x, y, z);
        }
    }
}
