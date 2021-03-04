using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace AmyzFactory.Models
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Name { get; set; }
        public string ImageUrl { get; set; }

        public ResultViewModel Result { get; set; }

        public Nullable<int> DepartmentID { get; set; }

        public Boolean Visibility { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        [JsonIgnore]
        public HttpPostedFileBase ImageFile { get; set; }

        public SelectList mainDepartmentsDropDown { get; set; }

        public List<CategoryViewModel> Categories { get; set; }
        public List<TextsViewModel> TechnicalsTitle_Description { get; set; }

    }
}