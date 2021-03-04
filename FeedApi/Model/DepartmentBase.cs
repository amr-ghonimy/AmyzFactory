using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeedApi.Model
{
    public class DepartmentBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string  ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool Visibility { get; set; }


    }
}