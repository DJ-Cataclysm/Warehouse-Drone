using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS
{
    public class Mutation
    {
        [Key, Column(Order = 0)]
        public int ID { get; set; }
        [Key, Column(Order = 1)]
        public DateTime MutationDate { get; set; }
        public int NewCount { get; set; }
        public int OldCount { get; set; }

        public Mutation(Product p)
        {
            ID = p.ID;
            MutationDate = DateTime.Now;
            NewCount = 0;
            OldCount = p.Count;
        }
    }
}
