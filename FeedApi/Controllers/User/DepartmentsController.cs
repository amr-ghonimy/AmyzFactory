using AmyzFactory.Models;
using AmyzFeed.Business.interfaces;
using AmyzFeed.Domain;
using AutoMapper;
using FeedApi.Model;
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


        public DepartmentsController(ICategoriesBusiness _catbusiness, IAddressesBusiness _addBusiness, IProductsBusinessUsers _productsBusiness)
        {
            this.catBusiness = _catbusiness;
            this.mapper = AutoMapperConfig.Mapper;
        }






        public IEnumerable<DepartmentsViewModel> GetDepartments()
        {
            List<DepartmentDomainModel> list = this.catBusiness.getDepartments();

            var result = this.mapper.Map<List<DepartmentsViewModel>>(list);

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
