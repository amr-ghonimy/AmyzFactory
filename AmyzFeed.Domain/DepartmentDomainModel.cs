using AmyzFeed.Domain;
using System;
using System.Collections.Generic;


namespace AmyzFactory.Models
{
    public class DepartmentDomainModel:BaseDepartment
    {
        public string ImageUrl { get; set; }

        public List<CategoryDomainModel> Categories { get; set; }
    }
}