using AmyzFactory.Models;
using AmyzFeed.Business.interfaces;
using AmyzFeed.Domain;
using AutoMapper;
using FeedApi.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace FeedApi.Controllers.User
{
    public class DepartmentsController : ApiController
    {

        private readonly IMapper mapper;
        private readonly ICategoriesBusiness catBusiness;


        public DepartmentsController(ICategoriesBusiness _catbusiness)
        {
            this.catBusiness = _catbusiness;
            this.mapper = AutoMapperConfig.Mapper;
        }






        public List<DepartmentsViewModel> GetDepartments()
        {
            List<DepartmentDomainModel> list = this.catBusiness.getDepartments();

            var result = this.mapper.Map<List<DepartmentsViewModel>>(list);

            return result;
        }

        public List<CategoriesViewModel> GetCategories()
        {
            List<CategoryDomainModel> list = this.catBusiness.getCategories();

            var result = this.mapper.Map<List<CategoriesViewModel>>(list);

            return result;
        }

        public IEnumerable<CategoriesViewModel> GetCategoriesByDeptID(int id)
        {
            List<CategoryDomainModel> list = this.catBusiness.getCategoriesByDepID(id);

            return this.mapper.Map<List<CategoriesViewModel>>(list);
        }

        public IHttpActionResult GetCategoryByID(int id)
        {
            CategoryDomainModel dm = this.catBusiness.getCategoryByID(id);

            if (dm == null)
            {
                return Content(HttpStatusCode.NotFound,
                    new ResultDomainModel(false, "category with id = " + id + " is not exist",id));
            }

            CategoriesViewModel vm = this.mapper.Map<CategoriesViewModel>(dm);
            
            return Ok(new ResultDomainModel(true, "category exist", Data: vm));
        }

        public IEnumerable<DepartmentsViewModel> GetTechnicals()
        {
            List<DepartmentDomainModel> dm = this.catBusiness.getTechniclas();

            var result = this.mapper.Map<List<DepartmentsViewModel>>(dm);

            return result;
        }

        
        [HttpDelete]
        public IHttpActionResult DeleteDepartment(int id)
        {

            if (id == 0)
            {
                return Content(HttpStatusCode.BadRequest, new ResultDomainModel(false, "you enter not valid id=" + id, modelID: id));
            }

            try
            {
                bool isSuccess = this.catBusiness.deleteDepartment(id);
                if (isSuccess)
                {
                    return Ok(new ResultDomainModel(true, "Department deleted successfully!",id));
                }else
                {
                    return Content(HttpStatusCode.BadRequest, new ResultDomainModel(false, "you enter not valid id=" + id, modelID: id));

                }
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.BadRequest, new ResultDomainModel(false, e.Message, modelID: id));
            }
            
        }
        [HttpDelete]
        public IHttpActionResult DeleteCategory(int id)
        {

            if (id == 0)
            {
                return Content(HttpStatusCode.BadRequest, new ResultDomainModel(false, "you enter not valid id=" + id, modelID: id));
            }

            try
            {
                bool isSuccess = this.catBusiness.deleteCategory(id);
                if (isSuccess)
                {
                    return Ok(new ResultDomainModel(true, "Category deleted successfully!", id));
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, new ResultDomainModel(false, "you enter not valid id=" + id, modelID: id));

                }
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.BadRequest, new ResultDomainModel(false, e.Message, modelID: id));
            }

        }
        [HttpDelete]
        public IHttpActionResult DeleteTechnical(int id)
        {

            if (id == 0)
            {
                return Content(HttpStatusCode.BadRequest, new ResultDomainModel(false, "you enter not valid id=" + id, modelID: id));
            }

            try
            {
                bool isSuccess = this.catBusiness.deleteTechnical(id);
                if (isSuccess)
                {
                    return Ok(new ResultDomainModel(true, "Technical deleted successfully!", id));
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, new ResultDomainModel(false, "you enter not valid id=" + id, modelID: id));

                }
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.BadRequest, new ResultDomainModel(false, e.Message, modelID: id));
            }

        }



        [HttpPost]
        public IHttpActionResult CreateDepartment([FromBody]DepartmentsViewModel model)
        {
            if (model == null)
            {

                return Content(HttpStatusCode.BadRequest,
                     new ResultDomainModel(false, "you send department with null value!")
                     );
            }

            DepartmentDomainModel dm = this.mapper.Map<DepartmentDomainModel>(model);

            try
            {
                ResultDomainModel result = this.catBusiness.createDepartment(dm);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest,
                    new ResultDomainModel(false, ex.Message)
                    );

            }

        }

        [HttpPost]
        public IHttpActionResult CreateCategory(CategoriesViewModel model)
        {
            if (model == null)
            {
                return Content(HttpStatusCode.BadRequest,
                     new ResultDomainModel(false, "you send category with null value!")
                     );

            }

            if (model.DepartmentID == 0)
            {
                return Content(HttpStatusCode.BadRequest,
                    new ResultDomainModel(false, "you send category with null department id value!")
                    );
            }

            CategoryDomainModel dm = this.mapper.Map<CategoryDomainModel>(model);

            try
            {
                ResultDomainModel result = this.catBusiness.createCategory(dm);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest,
                   new ResultDomainModel(false, ex.Message)
                   );
            }

        }

        [HttpPost]
        public IHttpActionResult CreateTechnical(DepartmentsViewModel model)
        {
            if (model == null)
            {
                return Content(HttpStatusCode.BadRequest,
                     new ResultDomainModel(false, "you send Technical with null value!")
                     );
            }

        
            DepartmentDomainModel dm = this.mapper.Map<DepartmentDomainModel>(model);

            try
            {
                ResultDomainModel result = this.catBusiness.createTechnecalSupport(dm);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest,
                   new ResultDomainModel(false, ex.Message)
                   );
            }

        }

        [HttpPut]
        public IHttpActionResult EditDepartment(DepartmentsViewModel model)
        {
            if (model == null)
            {
                return Content(HttpStatusCode.BadRequest,
                   new ResultDomainModel(false, "you send department with null value!")
                   );
            }

            if (model.Id == 0)
            {
                return Content(HttpStatusCode.BadRequest,
                   new ResultDomainModel(false, "department with his id = " + model.Id + " not found!")
                   );
            }

            CategoryDomainModel dm = this.mapper.Map<CategoryDomainModel>(model);

            try
            {
                ResultDomainModel result = this.catBusiness.editSubCategory(dm);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest,
                  new ResultDomainModel(false, ex.Message)
                  );
            }

        }

        [HttpPut]
        public IHttpActionResult EditCategory(CategoriesViewModel model)
        {
            if (model == null)
            {
                return Content(HttpStatusCode.BadRequest,
                  new ResultDomainModel(false, "you send category with null value!")
                  );
            }

            if (model.Id == 0)
            {
                return Content(HttpStatusCode.BadRequest,
                  new ResultDomainModel(false, "category with his id = " + model.Id + " not found!")
                  );
            }
            if (model.DepartmentID == 0)
            {
                return Content(HttpStatusCode.BadRequest,
                 new ResultDomainModel(false, "category with his department id = 0 not found!")
                 );
            }

            CategoryDomainModel dm = this.mapper.Map<CategoryDomainModel>(model);

            try
            {
                ResultDomainModel result = this.catBusiness.editSubCategory(dm);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest,
              new ResultDomainModel(false, ex.Message)
              );


            }

        }


    }
}
