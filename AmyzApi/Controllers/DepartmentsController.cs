using AmyzApi.Helpers;
using AmyzFactory.Models;
using AmyzFeed.AmyzApi.Helpers;
using AmyzFeed.Business.interfaces;
using AmyzFeed.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace FeedApi.Controllers.User
{
    public class DepartmentsController : ApiController
    {

         private readonly ICategoriesBusiness catBusiness;
        private readonly IImageBusiness imgBusiness;


        public DepartmentsController(ICategoriesBusiness _catbusiness, IImageBusiness _imgBusiness)
        {
            this.imgBusiness = _imgBusiness;
            this.catBusiness = _catbusiness;

            this.catBusiness.changeDepartmentVisibility(5);
            this.catBusiness.changeDepartmentVisibility(6);

        }

        private string GetRole()
        {
            string role = "";

            HttpRequestMessage request = this.ActionContext.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;

            if (authorization == null)
            {
                role = "Users";
                return role;
            }

            if (!string.IsNullOrEmpty(authorization.Parameter))
            {
                string token = authorization.Parameter;

                role = TokenManager.GetRoleByToken(token);

                if (role == null)
                {
                    role = "Users";
                }
            }

            return role;
        }






        // Tested Completed
        public List<DepartmentDomainModel> GetDepartments()
        {
            List<DepartmentDomainModel> list = this.catBusiness.getDepartments(GetRole());

 
            return list;
        }

        public List<CategoryDomainModel> GetCategories()
        {
            List<CategoryDomainModel> list = this.catBusiness.getCategories(GetRole());


            return list;
        }

        public IHttpActionResult GetCategoryByID(int id)
        {
            CategoryDomainModel dm = this.catBusiness.getCategoryByID(id, GetRole());

            if (dm == null)
            {
                return Content(HttpStatusCode.NotFound,
                    new ResultDomainModel(false, "category with id = " + id + " is not exist", id));
            }



            return Ok(new ResultDomainModel(true, "category exist", Data: dm));
        }



        // end Tested Completed








        public IEnumerable<CategoryDomainModel> GetCategoriesByDeptID(int id)
        {
            List<CategoryDomainModel> list = this.catBusiness.getCategoriesByDepID(id);

            return list;
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
                    return Ok(new ResultDomainModel(true, "Department deleted successfully!", id));
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



        [HttpPost]
        public IHttpActionResult CreateDepartment([FromBody]DepartmentDomainModel model)
        {
            if (model == null)
            {

                return Content(HttpStatusCode.BadRequest,
                     new ResultDomainModel(false, "you send department with null value!")
                     );
            }

            if (string.IsNullOrEmpty(model.ImageUrl))
            {
                model.ImageUrl = Constans.defaultDeptImage;
            }

 
            try
            {
                ResultDomainModel result = this.catBusiness.createDepartment(model);

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
        public IHttpActionResult CreateCategory(CategoryDomainModel model)
        {
            if (model == null)
            {
                return Content(HttpStatusCode.BadRequest,
                     new ResultDomainModel(false, "you send category with null value!")
                     );

            }

            if (string.IsNullOrEmpty(model.ImageUrl))
            {
                model.ImageUrl = Constans.defaultCatgImage;
            }



            if (model.DepartmentID == 0)
            {
                return Content(HttpStatusCode.BadRequest,
                    new ResultDomainModel(false, "you send category with null department id value!")
                    );
            }

 
            try
            {
                ResultDomainModel result = this.catBusiness.createCategory(model);

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
        public IHttpActionResult EditDepartment(CategoryDomainModel model)
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

 
            try
            {
                ResultDomainModel result = this.catBusiness.editSubCategory(model);

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
        public IHttpActionResult EditCategory(CategoryDomainModel model)
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

 
            try
            {
                ResultDomainModel result = this.catBusiness.editSubCategory(model);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest,
              new ResultDomainModel(false, ex.Message)
              );


            }

        }

        [HttpGet]
        public IHttpActionResult ISDepartmentExists(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Content(HttpStatusCode.BadRequest, "you enter empty value");
            }

            ResultDomainModel result = this.catBusiness.isDepartmentExists(name, GetRole());

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }
        }

        [HttpGet]
        public IHttpActionResult ISCategoryExists(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Content(HttpStatusCode.BadRequest, "you enter empty value");
            }

            ResultDomainModel result = this.catBusiness.isCategoyExists(name,GetRole());

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }
        }

        [HttpPost]
        public IHttpActionResult UploadImage()
        {
            ResultDomainModel result = new ResultDomainModel();

            var httpRequest = HttpContext.Current.Request;

            if (httpRequest.Files.Count < 1)
            {
                result.IsSuccess = false;
                result.Message = "حدث مشكلة فى رفع الصورة";
                return Content(HttpStatusCode.BadRequest, result);
            }


            result = this.imgBusiness.uploadImage(httpRequest, Constans.deptsImageFolderPath, Constans.deptsImageResponse, 1000);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }

        }

        [HttpGet]
        public IHttpActionResult EditDepartmentVisibility(int id)
        {
            if (id == 0)
            {
                return Content(HttpStatusCode.BadRequest, new ResultDomainModel(false, "You enter valid id", modelID: id));
            }

            bool isSuccess = this.catBusiness.changeDepartmentVisibility(id);

            ResultDomainModel result = new ResultDomainModel(isSuccess, "Visisbility Cahnged Successfully", modelID: id);

            return Ok(result);

        }

        [HttpGet]
        public IHttpActionResult EditCategoryVisibility(int id)
        {
            if (id == 0)
            {
                return Content(HttpStatusCode.BadRequest, new ResultDomainModel(false, "You enter valid id", modelID: id));
            }

            bool isSuccess = this.catBusiness.changeCategoryVisibility(id);

            ResultDomainModel result = new ResultDomainModel(isSuccess, "Visisbility Cahnged Successfully", modelID: id);

            return Ok(result);

        }

    }
}
