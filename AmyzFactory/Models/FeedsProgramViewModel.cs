using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmyzFactory.Models
{
    public class FeedsProgramViewModel
    {
        public int Id { get; set; }
        public string ProductType { get; set; }
        public int DayFrom { get; set; }
        public int DayTo { get; set; }
        public double ProtienPercentage { get; set; }
        public double Quantity { get; set; }
        public DateTime CreationDate { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
    }
}