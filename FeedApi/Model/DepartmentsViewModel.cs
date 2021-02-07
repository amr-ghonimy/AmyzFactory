using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeedApi.Model
{
    public class DepartmentsViewModel : DepartmentBase
    {
        public IEnumerable<CategoriesViewModel> Categories { get; set; }
    }
}