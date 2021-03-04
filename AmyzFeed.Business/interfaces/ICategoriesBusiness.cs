using AmyzFactory.Models;
using AmyzFeed.Domain;
using AmyzFeed.Repository;
using System;
using System.Collections.Generic;


namespace AmyzFeed.Business.interfaces
{
   public interface ICategoriesBusiness
    {
        List<DepartmentDomainModel> getDepartments();
        List<CategoryDomainModel> getCategories();
        CategoryDomainModel getCategoryByID(int id);

        List<CategoryDomainModel> getCategoriesByDepID(int departmentID);
        List<DepartmentDomainModel> getTechniclas();
        DepartmentDomainModel getTechniclByID(int id);

        ResultDomainModel isCategoyExists(string name);
        ResultDomainModel isDepartmentExists(string name);
        ResultDomainModel isTechnicalExists(string name);


        ResultDomainModel createCategory(CategoryDomainModel category);
        ResultDomainModel createDepartment(  DepartmentDomainModel department);
        ResultDomainModel createTechnecalSupport(  DepartmentDomainModel technicalSupport);

        ResultDomainModel editDepartment(DepartmentDomainModel department);
        ResultDomainModel editSubCategory(CategoryDomainModel category);
        ResultDomainModel editTechnical(DepartmentDomainModel technical);

        bool changeDepartmentVisibility(int id);
        bool changeCategoryVisibility(int id);

        bool deleteCategory(int categoryID);
        bool deleteDepartment(int categoryID);
        bool deleteTechnical(int techID);

    }
}
