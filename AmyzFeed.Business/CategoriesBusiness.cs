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
    public class CategoriesBusiness :ICategoriesBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly CategoryRepository catgRepository;
        private readonly DepartmentsRepository deptRepository;
        private readonly TechnicalRepository techRepository;
 
        private ResultDomainModel resultModel;

        public CategoriesBusiness(IUnitOfWork _unitOfWork,ResultDomainModel _resultModel)
        {
            this.unitOfWork = _unitOfWork;
            this.resultModel = _resultModel;
            this.catgRepository = new CategoryRepository(this.unitOfWork);
            this.techRepository = new TechnicalRepository(this.unitOfWork);
            this.deptRepository = new DepartmentsRepository(this.unitOfWork);
          }


        private ResultDomainModel initResultModel(bool isSuccess,string message,int modelID=0)
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
                DepartmentID = category.DepartmentID.Value,
                Visibility = category.visibility
            };

             var ifCategoryExists = this.catgRepository.SingleOrDefault(m => m.Name.Trim().ToLower() == catg.Name.Trim().ToLower()&&m.IsDeleted==false);

            if (ifCategoryExists!=null)
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
                CreationDate = DateTime.Now,
                Visibility = department.visibility,
            };

             var ifDeparmtentExists = this.deptRepository.SingleOrDefault(m => m.Name.Trim().ToLower() == dept.Name.Trim().ToLower() && m.IsDeleted == false);


            if (ifDeparmtentExists!=null)
            {
                return initResultModel(false, "Department Already Exists Enter Another Name!");
            }

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

        public ResultDomainModel createTechnecalSupport(DepartmentDomainModel technicalSupport)
        {
            var tech = new Technical()
            {
                ID = technicalSupport.Id,
                Name = technicalSupport.Name.Trim(),
                CreationDate = DateTime.Now,
                Visibility = technicalSupport.visibility,
            };
 
             var ifCategoryExists = this.techRepository.SingleOrDefault(m => m.Name.Trim().ToLower() == tech.Name.Trim().ToLower() && m.IsDeleted == false);

            if (ifCategoryExists!=null)
            {
                return initResultModel(false, "Technical Already Exists Enter Another Name!");
            }
            try
            {
                this.techRepository.Insert(tech);
                
                return initResultModel(true, "Technical Inserted Successfully", tech.ID);


            }
            catch (Exception e)
            {
                return initResultModel(false, "Failed To Insert Technical Error is: " + e.Message);

            }


        }

        public bool deleteCategory(int categoryID)
        {
            try
            {
                var categoryObj = this.catgRepository.SingleOrDefault(x => x.ID == categoryID);
                categoryObj.IsDeleted = true;
                this.catgRepository.Update(categoryObj);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void deleteAllCategoriesByDeptId(int departmentID)
        {
            var categories = this.catgRepository.GetAll(x => x.DepartmentID == departmentID);
            if (categories != null && categories.Count() > 0)
            {
                 
                    foreach (var item in categories)
                    {
                        item.IsDeleted = true;
                        catgRepository.Update(item);
                    }
                
            }
        }

        public bool deleteDepartment(int departmentID)
        {
            try
            {
                var deptObj = this.deptRepository.SingleOrDefault(x => x.ID == departmentID);
                deptObj.IsDeleted = true;
                this.deptRepository.Update(deptObj);

                this.deleteAllCategoriesByDeptId(departmentID);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        public bool deleteTechnical(int techID)
        {
            try
            {
                var techObj = this.techRepository.SingleOrDefault(x => x.ID == techID);
                techObj.IsDeleted = true;
                this.techRepository.Update(techObj);
                return true;
            }
            catch (Exception)
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

        public ResultDomainModel editTechnical(DepartmentDomainModel technical)
        {

            var oldTech = this.techRepository.SingleOrDefault(x => x.ID == technical.Id);


            if (oldTech == null)
            {
                return initResultModel(false, "model with his id = " + technical.Id + " not found!", technical.Id);
            }

            try
            {


                bool ifNameExists = this.techRepository.Exists(x => x.Name == technical.Name.Trim() && x.ID != technical.Id);
                if (ifNameExists)
                {
                    return initResultModel(false, "The name already exists!");
                }

                oldTech.Name = technical.Name.Trim();
                oldTech.Visibility = technical.visibility;
                oldTech.UpdatedDate = DateTime.Now;

                this.techRepository.Update(oldTech);
                return initResultModel(true, "Updated Success Technical has updated");

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
                ImageUrl = x.Image!=null ? Constans.ServerFile + x.Image : Constans.LogoPath  
            }).ToList();


            foreach (var item in list)
            {
                item.SubCategoriesList = catgRepository.GetAll(x => x.DepartmentID == item.Id&& x.IsDeleted == false)
                    .Select(s=>new CategoryDomainModel() {
                        Id=s.ID,
                        DepartmentID=s.DepartmentID,
                        Name=s.Name,
                        visibility=s.Visibility
                    }).ToList();
            }

            return list;

        }

        public List<DepartmentDomainModel> getTechniclas()
        {
            return techRepository.GetAll(x => x.IsDeleted == false).Select(x => new DepartmentDomainModel()
            {
                Name = x.Name,Id = x.ID}).ToList();
        }

        public CategoryDomainModel getCategoryByID(int id)
        {
            var catg = this.catgRepository.SingleOrDefault(x => x.ID == id&& x.IsDeleted == false,"Department");
            if (catg == null)
            {
                return null;
            }

            var result = new CategoryDomainModel()
            {
                Id = catg.ID,
                Name = catg.Name,
                DepartmentID = catg.DepartmentID,
                visibility = catg.Visibility
            };
           return result;
        }

        public DepartmentDomainModel getTechniclByID(int id)
        {
            var tech = this.techRepository.SingleOrDefault(x => x.ID == id& x.IsDeleted == false);
            if (tech == null)
            {
                return null;
            }

            var result = new DepartmentDomainModel()
            {
                Id = tech.ID,
                Name = tech.Name,
                 visibility = tech.Visibility
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
    }
}
