using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AmyzFactory.Models
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

        [RegularExpression("^[0-9]+$", ErrorMessage = "ادخل الهاتف بشكل صحيح")]
        [Required(ErrorMessage = "ادخل رقم الهاتف")]
        [Display(Name = "رقم الهاتف")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "ادخل رقم الهاتف 11 رقم")]
        public string Phone { get; set; }
       


        public double OrderTotalPrice { get; set; }
        public int ItemsCount { get; set; }
 
        public List<OrderDetailsViewModel> OrderDetailsList { get; set; }

        public ResultViewModel Result { get; set; }

    }
}