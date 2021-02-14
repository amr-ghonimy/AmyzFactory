using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeedApi.Model
{
    public class PricesViewModel
    {
        public int Id { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }

        public string Name{ get; set; }
        public double Price { get; set; }

    }
}