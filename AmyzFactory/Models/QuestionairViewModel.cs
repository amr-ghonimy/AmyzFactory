using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AmyzFactory.Models
{
    public class QuestionairViewModel
    {

        [Required(ErrorMessage = "ادخل اسمك الأول")]
        [Display(Name = "الاسم الأول")]
        public string  FirstNAme { get; set; }

        [Display(Name = "اسم العائلة")]
        [Required(ErrorMessage = "ادخل اسم العائلة")]
        public string LastNAme { get; set; }

        [Display(Name = "البريد الالكترونى")]
        [EmailAddress(ErrorMessage ="من فضلك ادخل بريد الكترونى صحيح")]
        [Required(ErrorMessage ="من فضلك ادخل البريد الالكترونى")]
        public string Email { get; set; }


        [Display(Name = "الاستفسار")]
        [Required(ErrorMessage = "من فضلك ادخل استفسارك")]
        public string Question { get; set; }


    }
}