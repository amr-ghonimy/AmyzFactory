using AmyzFactory.Models;
using AmyzFeed.Business.helpers;
using AmyzFeed.Business.interfaces;
using AmyzFeed.Domain;
using AmyzFeed.Repository;
using AmyzFeed.Repository.Data;
using AmyzFeed.Repository.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Linq;


namespace AmyzFeed.Business
{
    public class CategoriesBusiness : ICategoriesBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly CategoryRepository catgRepository;
        private readonly DepartmentsRepository deptRepository;
        private readonly TechnicalRepository techRepository;

        private ResultDomainModel resultModel;

        public CategoriesBusiness(IUnitOfWork _unitOfWork, ResultDomainModel _resultModel)
        {
            this.unitOfWork = _unitOfWork;
            this.resultModel = _resultModel;
            this.catgRepository = new CategoryRepository(this.unitOfWork);
            this.techRepository = new TechnicalRepository(this.unitOfWork);
            this.deptRepository = new DepartmentsRepository(this.unitOfWork);
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
            catch (Exception)
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





        public List<CategoryDomainModel> getCategories()
        {
            return catgRepository.GetAll(x => x.IsDeleted == false).Select(x => new CategoryDomainModel()
            {
                Name = x.Name,
                DepartmentID = x.DepartmentID,
                ImageUrl = Constans.ServerFile + x.Image,
                Id = x.ID,
                visibility = x.Visibility
            }).ToList();
        }

        public List<CategoryDomainModel> getCategoriesByDepID(int departmentID)
        {
            return catgRepository.GetAll(x => x.DepartmentID == departmentID && x.IsDeleted == false).Select(x => new CategoryDomainModel()
            {
                Name = x.Name,
                DepartmentID = x.DepartmentID,
                visibility = x.Visibility,
                ImageUrl = Constans.ServerFile + x.Image,
                Id = x.ID
            }).ToList();
        }

        public List<DepartmentDomainModel> getDepartments()
        {

            List<DepartmentDomainModel> list = deptRepository.GetAll(x => x.IsDeleted == false).Select(x => new DepartmentDomainModel()
            {
                Name = x.Name,
                Id = x.ID,
                visibility = x.Visibility,
                ImageUrl = Constans.ServerFile + x.Image
            }).ToList();


            foreach (var item in list)
            {
                item.Categories = catgRepository.GetAll(x => x.DepartmentID == item.Id && x.IsDeleted == false)
                    .Select(s => new CategoryDomainModel()
                    {
                        Id = s.ID,
                        DepartmentID = s.DepartmentID,
                        Name = s.Name,
                        ImageUrl = Constans.ServerFile + s.Image,
                        visibility = s.Visibility
                    }).ToList();
            }

            return list;

        }


        public CategoryDomainModel getCategoryByID(int id)
        {
            var catg = this.catgRepository.SingleOrDefault(x => x.ID == id && x.IsDeleted == false, "Department");
            if (catg == null)
            {
                return null;
            }

            var result = new CategoryDomainModel()
            {
                Id = catg.ID,
                Name = catg.Name,
                ImageUrl = Constans.ServerFile + catg.Image,
                DepartmentID = catg.DepartmentID,
                visibility = catg.Visibility
            };
            return result;
        }


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
                return false;
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
                return false;
            }
        }

        public ResultDomainModel isCategoyExists(string name)
        {
            var ifCategoryExists = this.catgRepository.SingleOrDefault(m => m.Name.Trim().ToLower() == name.Trim().ToLower() && m.IsDeleted == false);

            if (ifCategoryExists == null)
            {
                return new ResultDomainModel(false, "category not exists");
            }
            else
            {
                return new ResultDomainModel(true, "category is exists");

            }

        }

        public ResultDomainModel isDepartmentExists(string name)
        {
            var ifCategoryExists = this.deptRepository.SingleOrDefault(m => m.Name.Trim().ToLower() == name.Trim().ToLower() && m.IsDeleted == false);

            if (ifCategoryExists == null)
            {
                return new ResultDomainModel(false, "department not exists");
            }
            else
            {
                return new ResultDomainModel(true, "department is exists");

            }
        }
    }
}
