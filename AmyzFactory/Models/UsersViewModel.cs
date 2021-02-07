using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AmyzFactory.Models
{
 
    public class AdminViewModel 
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }


        public ResultViewModel result { get; set; }

    }

    public class UserViemModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }


        [Required (ErrorMessage ="ادخل الاسم الأول")]
        [Display(Name = "الاسم الأول")]
        public string FirstName{ get; set; }

        [Required(ErrorMessage = "ادخل الاسم التانى")]
        [Display(Name = "الاسم التانى")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "ادخل البريد الالكترونى")]
        [EmailAddress(ErrorMessage ="ادخل البريد الالكترونى بشكل صحيح")]
        [Display(Name = "البريد الالكترونى")]
        public string Email { get; set; }
        
        public string ReturnUrl { get; set; }


        public ResultViewModel result { get; set; }

        [RegularExpression("^[0-9]+$", ErrorMessage ="ادخل الهاتف بشكل صحيح")]
        [Required(ErrorMessage = "ادخل رقم الهاتف")]
        [Display(Name = "رقم الهاتف")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "ادخل رقم الهاتف 11 رقم")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "ادخل العنوان")]
        [Display(Name = "العنوان")]
        public string Address { get; set; }

        [StringLength(14, MinimumLength = 14, ErrorMessage = "ادخل رقم البطاقة 14 رقم")]
        [Required(ErrorMessage = "ادخل رقم البطافة 14 رقما")]
        [Display(Name = "رقم البطاقة")]
        public string PersonalId { get; set; }

        [StringLength(30, MinimumLength = 6, ErrorMessage = "أقل عدد حروف 6")]
        [Required(ErrorMessage = "ادخل رقم المرور")]
        [Display(Name = "كلمة السر")]
        public string Password { get; set; }

        [Required(ErrorMessage = "ادخل تأكيد رقم المرور")]
        [Compare("Password",ErrorMessage ="كلمة السر وتأكيد كلمة السر غير متطابقين")]
        [Display(Name = "تأكيد كلمة السر")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "ادخل المحافظة")]
        public string Governorate { get; set; }

    }
}