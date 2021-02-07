using AmyzFeed.Domain;
using System;
using System.Collections.Generic;


namespace AmyzFactory.Models
{
    public class DepartmentDomainModel:BaseDepartment
    {
        public List<CategoryDomainModel> SubCategoriesList { get; set; }
    }
}