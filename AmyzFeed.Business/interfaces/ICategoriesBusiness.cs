using AmyzFactory.Models;
using AmyzFeed.Domain;
using AmyzFeed.Repository;
using System;
using System.Collections.Generic;


namespace AmyzFeed.Business.interfaces
{
   public interface ICategoriesBusiness
    {
        List<DepartmentDomainModel> getDepartments(string role);
        List<CategoryDomainModel> getCategories(string role);
        CategoryDomainModel getCategoryByID(int id);

        List<CategoryDomainModel> getCategoriesByDepID(int departmentID);
      
        ResultDomainModel isCategoyExists(string name);
        ResultDomainModel isDepartmentExists(string name);
 

        ResultDomainModel createCategory(CategoryDomainModel category);
        ResultDomainModel createDepartment(  DepartmentDomainModel department);
 
        ResultDomainModel editDepartment(DepartmentDomainModel department);
        ResultDomainModel editSubCategory(CategoryDomainModel category);
 
        bool changeDepartmentVisibility(int id);
        bool changeCategoryVisibility(int id);

        bool deleteCategory(int categoryID);
        bool deleteDepartment(int categoryID);
 
    }
}
