 
using System.Collections.Generic;
 
 

namespace FeedApi.Models
{
    public class CategoryShowModel
    {
        public int Id { get; set; }

         public string Name { get; set; }


        public virtual List<CategoryShowModel> SubCategoriesList { get; set; }
        

    }
}