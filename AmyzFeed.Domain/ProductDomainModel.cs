using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmyzFactory.Models
{
    public class ProductDomainModel
    {
        public int Id { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public string CategoryName { get; set; }

        public string Name { get; set; }
        public string Definition { get; set; }
        public string Description { get; set; }
        public double Price{ get; set; }
        public int Quantity{ get; set; }
        public string ImageUrl { get; set; }
        public HttpRequest ImageFile { get; set; }
        
        public bool isVisible{ get; set; }
        public bool isAvailable
        {
            get
            {
                if (this.Quantity > 0)
                {
                    return true;
                }
                return false;
            }

        }
       

    }
}