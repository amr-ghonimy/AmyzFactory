using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FeedApi.Models
{
    public class QuestionairViewModel
    {

        [Required(ErrorMessage = "ادخل الاسم")]
        [Display(Name = "الاسم")]
        public string  FullName { get; set; }

        [Display(Name = "البريد الالكترونى")]
        [EmailAddress(ErrorMessage ="من فضلك ادخل بريد الكترونى صحيح")]
        [Required(ErrorMessage ="من فضلك ادخل البريد الالكترونى")]
        public string Email { get; set; }

        [Display(Name = "الاستفسار")]
        [Required(ErrorMessage = "من فضلك ادخل استفسارك")]
        public string Question { get; set; }
        
    }
}