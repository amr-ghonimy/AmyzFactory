using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmyzFactory.Models
{
    public class OrderDetailsViewModel
    {
        public int OrderID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string ItemImg { get; set; }

        public float Price { get; set; }
        public int Quantity { get; set; }
        public float Total { get; set; }
    }
}