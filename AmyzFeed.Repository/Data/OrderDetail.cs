using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmyzFeed.Repository.Data
{
  public  class OrderDetail
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int OrderID { get; set; }
        [Required]
        public int ProductID { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public double Price { get; set; }
       
        [Required]
        public double Total { get; set; }

        public Order Order { get; set; }
        public Product Product { get; set; }

    }
}
