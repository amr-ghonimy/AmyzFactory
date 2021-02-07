using AmyzFactory.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmyzFeed.Repository.Data
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        [Required]
        public string UserID { get; set; }
        [Required]
        public string OrderNumber { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        public string Note { get; set; }

        [Required]
        public string Address { get; set; }

        public int OrderState { get; set; }

        [Required]
        public int ItemsCount { get; set; }
        [Required]
        public double ShippingCost { get; set; }
        [Required]
        public double TotalCost { get; set; }
        ICollection<OrderDetail> orderDetails { get; set; }
    }
}
