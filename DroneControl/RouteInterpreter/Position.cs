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
        const int HASH_STARTNUMBER = 13; //Should be a prime number to avoid collisions
        const int HASH_MULTIPLIER = 7;

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
            int hash = HASH_STARTNUMBER;
            hash = (hash * HASH_MULTIPLIER) + x.GetHashCode();
            hash = (hash * HASH_MULTIPLIER) + y.GetHashCode();
            hash = (hash * HASH_MULTIPLIER) + z.GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            return string.Format("({0},{1},{2})", x, y, z);
        }
    }
}
