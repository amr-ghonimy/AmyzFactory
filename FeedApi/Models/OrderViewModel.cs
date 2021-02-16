﻿using AmyzFactory.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FeedApi.Models
{
    public class OrderViewModel
    {
        public int OrderID { get; set; }

        public string UserID { get; set; }

        public string OrderDate { get; set; }

        public string OrderNumber { get; set; }

        public double ShippingCost { get; set; }

        [Display(Name = "ادخل ملاحظات للطلب")]
        public string Notes { get; set; }

        [Display(Name = "ادخل عنوانك بالتفصيل")]
        [Required(ErrorMessage ="من فضلك ادخل عنوانك بالتفصيل")]
        public string Addreess { get; set; }
        public double OrderTotalPrice { get; set; }
        public int ItemsCount { get; set; }

 
        public List<OrderDetailsViewModel> OrderDetailsList { get; set; }

        public ResultDomainModel Result { get; set; }

    }
}