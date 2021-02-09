using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeedApi.Model
{
    public class ProductsViewModel
    {
        public int Id { get; set; }
        public int CategoryID { get; set; }
        public string Name{ get; set; }
        public string CategoryName { get; set; }
        public string Definition { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public bool isVisible { get; set; }
        public string ImageUrl { get; set; }
    }
}