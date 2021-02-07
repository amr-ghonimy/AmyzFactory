using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmyzFeed.Domain
{
 public  class OrderDetailsDomainModel
    {
        public int OrderID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string ItemImg { get; set; }

        public double Price { get; set; }
        public int Quantity { get; set; }
        public double Total { get; set; }
        
    }
}
