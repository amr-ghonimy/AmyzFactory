using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AmyzFactory.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string Definition { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public float Price{ get; set; }
        [Required(ErrorMessage = "This field is required")]
        public int Quantity{ get; set; }
        public float Total { get; set; }

        public string  ImageURL { get; set; }


        [JsonIgnore]
        public HttpPostedFileBase ImageFile { get; set; }

         public ResultViewModel ResponseResult { get; set; }

        public List<ProductViewModel> list { get; set; }


        public bool isVisible{ get; set; }
        public bool isAvailable
        {
            get
            {
                if (this.Quantity > 0)
                {
                    return true;
                }
                return false;
            }

        }
     

    }
}