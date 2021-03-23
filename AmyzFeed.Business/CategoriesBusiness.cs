using AmyzFactory.Models;
using AmyzFeed.Business.interfaces;
using AmyzFeed.Domain;
using AmyzFeed.Repository;
using AmyzFeed.Repository.Data;
using AmyzFeed.Repository.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Configuration;

namespace AmyzFeed.Business
{
    public class CategoriesBusiness : ICategoriesBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly CategoryRepository catgRepository;
        private readonly DepartmentsRepository deptRepository;
        private readonly TechnicalRepository techRepository;

        private ResultDomainModel resultModel;

        private string baseUrl;


        public CategoriesBusiness(IUnitOfWork _unitOfWork, ResultDomainModel _resultModel)
        {
            this.unitOfWork = _unitOfWork;
            this.resultModel = _resultModel;
            this.catgRepository = new CategoryRepository(this.unitOfWork);
            this.techRepository = new TechnicalRepository(this.unitOfWork);
            this.deptRepository = new DepartmentsRepository(this.unitOfWork);
 
             this.baseUrl = WebConfigurationManager.AppSettings["baseUrl"];

        }


        private ResultDomainModel initResultModel(bool isSuccess, string message, int modelID = 0)
        {
            resultModel.IsSuccess = isSuccess;
            resultModel.Message = message;
            resultModel.modelID = modelID;
            return resultModel;
        }





        public ResultDomainModel createCategory(CategoryDomainModel category)
        {

            var catg = new Category()
            {
                ID = category.Id,
                Name = category.Name.Trim(),
                CreationDate = DateTime.Now,
                Image = category.ImageUrl,
                DepartmentID = category.DepartmentID.Value,
                Visibility = category.visibility
            };

            var ifCategoryExists = this.catgRepository.SingleOrDefault(m => m.Name.Trim().ToLower() == catg.Name.Trim().ToLower() && m.IsDeleted == false);

            if (ifCategoryExists != null)
            {

                return initResultModel(false, "Category Already Exists Enter Another Name!");
            }
            try
            {
                catgRepository.Insert(catg);

                return initResultModel(true, "Category Inserted Successfully", catg.ID);

            }
            catch (Exception e)
            {
                return initResultModel(false, "Failed To Insert Category Error is: " + e.Message);
            }

        }

        public ResultDomainModel createDepartment(DepartmentDomainModel department)
        {
            var dept = new Department()
            {
                ID = department.Id,
                Name = department.Name.Trim(),
                Image = department.ImageUrl,
                CreationDate = DateTime.Now,
                Visibility = department.visibility,
            };


            try
            {
                deptRepository.Insert(dept);

                return initResultModel(true, "Department Inserted Successfully", dept.ID);
            }
            catch (Exception e)
            {
                return initResultModel(false, "Failed To Insert Department Error is: " + e.Message);
            }

        }


        public bool deleteCategory(int categoryID)
        {
            try
            {
                this.catgRepository.Delete(x => x.ID == categoryID);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public bool deleteDepartment(int departmentID)
        {
            try
            {
                this.deptRepository.Delete(x => x.ID == departmentID);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }


        public ResultDomainModel editDepartment(DepartmentDomainModel department)
        {

            var oldDeprtment = this.deptRepository.SingleOrDefault(x => x.ID == department.Id);

            if (oldDeprtment == null)
            {
                return initResultModel(false, "model with his id = " + department.Id + " not found!", department.Id);
            }

            try
            {
                bool ifNameExists = this.deptRepository.Exists(x => x.Name == department.Name.Trim() && x.ID != department.Id);

                if (ifNameExists)
                {
                    return initResultModel(false, "The name already exists!");
                }

                oldDeprtment.Name = department.Name.Trim();
                oldDeprtment.Visibility = oldDeprtment.Visibility;
                oldDeprtment.UpdatedDate = DateTime.Now;

                this.deptRepository.Update(oldDeprtment);
                return initResultModel(true, "Updated Success department has updated");

            }
            catch (Exception e)
            {
                return initResultModel(false, e.Message.ToString());
            }

        }

        public ResultDomainModel editSubCategory(CategoryDomainModel category)
        {
            var oldCatg = this.catgRepository.SingleOrDefault(x => x.ID == category.Id);

            if (oldCatg == null)
            {
                return initResultModel(false, "model with his id = " + category.Id + " not found!", category.Id);
            }

            try
            {
                bool ifNameExists = this.catgRepository.Exists(x => x.Name == category.Name.Trim() && x.ID != category.Id);

                if (ifNameExists)
                {
                    return initResultModel(false, "The name already exists!");
                }

                oldCatg.Name = category.Name.Trim();
                oldCatg.Visibility = category.visibility;
                oldCatg.DepartmentID = category.DepartmentID.Value;
                oldCatg.UpdatedDate = DateTime.Now;

                this.catgRepository.Update(oldCatg);
                return initResultModel(true, "Updated Success category has updated");
            }
            catch (Exception e)
            {
                return initResultModel(false, "Updated failed " + e.Message);
            }
        }


      


   


       


        public List<CategoryDomainModel> getCategoriesByDepID(int departmentID)
        {

            return catgRepository.GetAll(x => x.DepartmentID == departmentID).Select(x => new CategoryDomainModel()
            {
                Name = x.Name,
                DepartmentID = x.DepartmentID,
                visibility = x.Visibility,
                ImageUrl = baseUrl + x.Image,
                Id = x.ID
            }).ToList();
        }






        // tested completed.....
        public List<CategoryDomainModel> getCategories(string role)
        {

            // string query
            Expression<Func<Category, bool>> whereCondition = x => true;

            if (role != "Admins")
                whereCondition = x => x.Visibility == true;

 


            return catgRepository.GetAll(whereCondition).Select(x => new CategoryDomainModel()
            {
                Name = x.Name,
                DepartmentID = x.DepartmentID,
                ImageUrl = baseUrl + x.Image,
                Id = x.ID,
                visibility = x.Visibility
            }).ToList();
        }

        public List<DepartmentDomainModel> getDepartments(string role)
        {
            Expression<Func<Department, bool>> whereCondition = x => true;

            if (role != "Admins")
                whereCondition = x => x.Visibility == true;
 

            List<DepartmentDomainModel> list = deptRepository.GetAll(whereCondition).Select(x => new DepartmentDomainModel()
            {
                Name = x.Name,
                Id = x.ID,
                visibility = x.Visibility,
                ImageUrl = baseUrl + x.Image
            }).ToList();


            foreach (var item in list)
            {

                Expression<Func<Category, bool>> whereConditionSubCatgs =x=> x.DepartmentID == item.Id;

                if (role != "Admins")
                    whereConditionSubCatgs = x => x.Visibility == true && x.DepartmentID == item.Id;
                                
 
                item.Categories = catgRepository.GetAll(whereConditionSubCatgs)
                    .Select(s => new CategoryDomainModel()
                    {
                        Id = s.ID,
                        DepartmentID = s.DepartmentID,
                        Name = s.Name,
                        ImageUrl = baseUrl + s.Image,
                        visibility = s.Visibility
                    }).ToList();
            }

            return list;

        }

        public CategoryDomainModel getCategoryByID(int id, string role)
        {
            Expression<Func<Category, bool>> whereCondition = x => x.ID == id;

            if (role != "Admins")
                whereCondition = x => x.Visibility == true && x.ID == id;


            var catg = this.catgRepository.SingleOrDefault(whereCondition, "Department");
            if (catg == null)
            {
                return null;
            }

            var result = new CategoryDomainModel()
            {
                Id = catg.ID,
                Name = catg.Name,
                ImageUrl = baseUrl + catg.Image,
                DepartmentID = catg.DepartmentID,
                visibility = catg.Visibility
            };
            return result;
        }

        public ResultDomainModel isCategoyExists(string name, string role)
        {
            Expression<Func<Category, bool>> whereCondition = m => m.Name.Trim().ToLower() == name.Trim().ToLower();

            if (role != "Admins")
                whereCondition = m => m.Name.Trim().ToLower() == name.Trim().ToLower() && m.Visibility == true;



            var ifCategoryExists = this.catgRepository.SingleOrDefault(whereCondition);

            if (ifCategoryExists == null)
            {
                return new ResultDomainModel(false, "category not exists");
            }
            else
            {
                return new ResultDomainModel(true, "category is exists");

            }

        }

        public ResultDomainModel isDepartmentExists(string name, string role)
        {

            Expression<Func<Department, bool>> whereCondition = m => m.Name.Trim().ToLower() == name.Trim().ToLower();

            if (role != "Admins")
                whereCondition = m => m.Name.Trim().ToLower() == name.Trim().ToLower() && m.Visibility == true;


            var ifCategoryExists = this.deptRepository.SingleOrDefault(whereCondition);

            if (ifCategoryExists == null)
            {
                return new ResultDomainModel(false, "department not exists");
            }
            else
            {
                return new ResultDomainModel(true, "department is exists");

            }
        }


        // end tested






        public bool changeDepartmentVisibility(int id)
        {

            Department obj = this.deptRepository.SingleOrDefault(x => x.ID == id);
            if (obj == null)
            {
                return false;
            }

            try
            {
                obj.Visibility = !obj.Visibility;
                this.deptRepository.Update(obj);

                return obj.Visibility;
            }
            catch (Exception)
            {
                return obj.Visibility;
            }
        }

        public bool changeCategoryVisibility(int id)
        {

            Category obj = this.catgRepository.SingleOrDefault(x => x.ID == id);
            if (obj == null)
            {
                return false;
            }
            try
            {
                obj.Visibility = !obj.Visibility;
                this.catgRepository.Update(obj);
                return obj.Visibility;
            }
            catch (Exception)
            {
                return obj.Visibility;
            }
        }

     
    }
}
