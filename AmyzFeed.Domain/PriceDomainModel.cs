﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmyzFeed.Domain
{
  public  class PriceDomainModel
    {
        public int Id { get; set; }
        public int CategoryID { get; set; }

        public string Name { get; set; }
        public double Price { get; set; }
    }
}
