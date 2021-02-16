using AmyzFactory.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace FeedApi.Models
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Name { get; set; }
        [JsonIgnore]
        public ResultDomainModel Result { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        [JsonIgnore]
        public Boolean visibility { get; set; }
        [JsonIgnore]
        public DateTime creation_date { get; set; }
        [JsonIgnore]
        public DateTime updated_date { get; set; }

        //   public SelectList mainDepartmentsDropDown { get; set; }
       
        public List<CategoryViewModel> SubCategoriesList { get; set; }
        [JsonIgnore]
        public List<InformationViewModel> TechnicalsTitle_Description { get; set; }

    }
}