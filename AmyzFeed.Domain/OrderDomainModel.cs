using AmyzFeed.Domain;
using System;
using System.Collections.Generic;


namespace AmyzFactory.Models
{
    public class OrderDomainModel
    {
        public int OrderID { get; set; }
        public string UserID { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderNumber { get; set; }
        public double ShippingCost { get; set; }
        public string Notes { get; set; }
        public string Addreess { get; set; }
        public string  Phone{ get; set; }
        public double OrderTotalPrice { get; set; }


        public int ItemsCount { get; set; }
        public string UserName { get; set; }
        public List<OrderDetailsDomainModel> OrderDetailsList { get; set; }

        public ResultDomainModel Result { get; set; }


    }
}