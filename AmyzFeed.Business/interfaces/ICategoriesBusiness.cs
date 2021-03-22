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
        CategoryDomainModel getCategoryByID(int id, string role);

        List<CategoryDomainModel> getCategoriesByDepID(int departmentID);
      
        ResultDomainModel isCategoyExists(string name, string role);
        ResultDomainModel isDepartmentExists(string name, string role);
 

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
