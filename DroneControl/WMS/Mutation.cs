using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS
{
    public class Mutation
    {
        //Compound primary key of ID and MutationDate
        [Key, Column(Order = 0)]
        public int ID { get; set; }
        [Key, Column(Order = 1)]
        public DateTime MutationDate { get; set; }
        public int NewCount { get; set; }
        public int OldCount { get; set; }
        public double Deviation { get; set; }

        //Generate new mutation for a product
        public Mutation(Product p)
        {
            ID = p.ID;
            MutationDate = DateTime.Now;
            NewCount = 0;
            OldCount = p.Count;
            Deviation = (OldCount / 100) * NewCount; 
        }
    }
}
