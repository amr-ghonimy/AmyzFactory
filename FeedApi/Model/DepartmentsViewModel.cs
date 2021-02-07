using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeedApi.Model
{
    public class DepartmentsViewModel : DepartmentBase
    {
        public string ImageUrl { get; set; }
        public IEnumerable<CategoriesViewModel> Categories { get; set; }
    }
}