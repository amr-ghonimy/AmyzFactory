using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AmyzFactory.Models
{
    public class CategoryShowModel
    {
        public int Id { get; set; }

         public string Name { get; set; }


        public virtual List<CategoryShowModel> SubCategoriesList { get; set; }
        

    }
}