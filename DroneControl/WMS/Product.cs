using System;

namespace WMS
{
    public class Product
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime? LastCheck { get; set; } //Nullable field
        public int Count { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public string Description { get; set; }
    }
}
