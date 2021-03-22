using AmyzFactory.App_Start;
using AmyzFactory.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace AmyzFactory.Areas.Admin.Controllers
{

    [AdminAuthorize(Roles = "Admins")]

    public class CategoriesController : BaseController
    {
      
        

        public PartialViewResult _Categories()
        {

            var department = new CategoryViewModel()
            {
                mainDepartmentsDropDown = new SelectList(new List<CategoryViewModel>(), "Id", "Name")
            };

            return PartialView("~/Areas/Admin/Views/Categories/partial/_Categories.cshtml", department);
        }


        public PartialViewResult _Departments()
        {
            return PartialView("~/Areas/Admin/Views/Categories/partial/_Departments.cshtml");
        }
       




        private List<CategoryViewModel> getDepartments()
        {
            string tokenNumber = Session[SessionsModel.Token]?.ToString();

            GlobalVariables.WebApiClient.DefaultRequestHeaders.Clear();
            GlobalVariables.WebApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", tokenNumber);

            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Departments/GetDepartments").Result;
            List<CategoryViewModel> departments = response.Content.ReadAsAsync<List<CategoryViewModel>>().Result;
 
            return departments;
        }

        private List<CategoryViewModel> getCategories()
        {
            string tokenNumber = Session[SessionsModel.Token]?.ToString();

            GlobalVariables.WebApiClient.DefaultRequestHeaders.Clear();
            GlobalVariables.WebApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", tokenNumber);

            HttpResponseMessage response =  GlobalVariables.WebApiClient.GetAsync("Departments/GetCategories").Result;
            List<CategoryViewModel> categories = response.Content.ReadAsAsync<List<CategoryViewModel>>().Result;

            return categories;
        }


    


        public ActionResult Index()
        { 
            return View();
        }

        [HttpGet]
        public JsonResult MainDepartments()
        {
            var departments =  this.getDepartments();

            return Json(departments, JsonRequestBehavior.AllowGet); 
        }

        [HttpGet]
        public JsonResult SubCategories()
        {
            var subCategories = this.getCategories();

            var dep = new CategoryViewModel()
            {
                Categories = subCategories,
                mainDepartmentsDropDown = new SelectList(new List<CategoryViewModel>(), "Id", "Name")
            };

            return Json(dep, JsonRequestBehavior.AllowGet);

         }

    




        // Create


        private ResultViewModel uploadImage(HttpPostedFileBase imageFile, string apiPath)
        {

            using (var content = new MultipartFormDataContent())
            {
                MemoryStream target = new MemoryStream();
                imageFile.InputStream.CopyTo(target);
                byte[] Bytes = target.ToArray();


                imageFile.InputStream.Read(Bytes, 0, Bytes.Length);
                var fileContent = new ByteArrayContent(Bytes);
                fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = imageFile.FileName };


                content.Add(fileContent);


                // content.Add(new StringContent(imageUniqueKey), "FileId");
                //content.Headers.Add("Key", "abc23sdflsdf");

                var response = GlobalVariables.WebApiClient.PostAsync(apiPath, content).Result;
                string resultVm = response.Content.ReadAsStringAsync().Result;
                ResultViewModel result = JsonConvert.DeserializeObject<ResultViewModel>(resultVm);
                return result;
            }

        }

        private JsonResult uploadData(CategoryViewModel model,string apiPath)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync(apiPath, model).Result;
            ResultViewModel result = response.Content.ReadAsAsync<ResultViewModel>().Result;

            model.Id = result.modelID;
            model.Result = result;
            model.ImageFile = null;

            return Json(model, JsonRequestBehavior.AllowGet);
        }


        private ResultViewModel checkIfModelExistsInDb(CategoryViewModel model,string apiPath)
        {
            string tokenNumber = Session[SessionsModel.Token]?.ToString();

            GlobalVariables.WebApiClient.DefaultRequestHeaders.Clear();
            GlobalVariables.WebApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", tokenNumber);

            string url = apiPath + "?name=" + model.Name;
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync(url).Result;
            ResultViewModel result = response.Content.ReadAsAsync<ResultViewModel>().Result;

            return result;
        }


        [HttpPost]
        public JsonResult CreateDepartment(CategoryViewModel categoryModel)
        {

            if(checkIfModelExistsInDb(categoryModel, "Departments/ISDepartmentExists").IsSuccess)
            {
                ResultViewModel resultVm = new ResultViewModel { IsSuccess = false, Message = "Department is already Exists" };
                categoryModel.Result = resultVm;
                return Json(categoryModel, JsonRequestBehavior.AllowGet);
            }



            if (categoryModel.ImageFile != null)
            {
                ResultViewModel imageUploadResult = this.uploadImage(categoryModel.ImageFile, "Departments/UploadImage");

                if (imageUploadResult.IsSuccess)
                {
                    ImagesViewModel img = JsonConvert.DeserializeObject<ImagesViewModel>(imageUploadResult.Data.ToString());
                    categoryModel.ImageUrl = img.ImageUrl;
                    return this.uploadData(categoryModel, "Departments/CreateDepartment");
                }else
                {
                    categoryModel.Result = imageUploadResult;

                    return Json(categoryModel, JsonRequestBehavior.AllowGet);
                }
            }


            return this.uploadData(categoryModel, "Departments/CreateDepartment");
        }


        [HttpPost]
        public JsonResult CreateCategory(CategoryViewModel categoryModel)
        {


            if (checkIfModelExistsInDb(categoryModel, "Departments/ISCategoryExists").IsSuccess)
            {
                ResultViewModel resultVm = new ResultViewModel { IsSuccess = false, Message = "Category is already Exists enter another name" };
                categoryModel.Result = resultVm;
                return Json(categoryModel, JsonRequestBehavior.AllowGet);
            }



            if (categoryModel.ImageFile != null)
            {
                ResultViewModel imageUploadResult = this.uploadImage(categoryModel.ImageFile, "Departments/UploadImage");

                if (imageUploadResult.IsSuccess)
                {
                    ImagesViewModel img = JsonConvert.DeserializeObject<ImagesViewModel>(imageUploadResult.Data.ToString());
                    categoryModel.ImageUrl = img.ImageUrl;
                    return this.uploadData(categoryModel, "Departments/CreateCategory");
                }
                else
                {
                    categoryModel.Result = imageUploadResult;

                    return Json(categoryModel, JsonRequestBehavior.AllowGet);
                }
            }


            return this.uploadData(categoryModel, "Departments/CreateCategory");
        }


     

        // Delete

        private ResultViewModel deleteResponse(Boolean result,string action,string controller,string tblBody,string editAction,string deleteAction) {
            ResultViewModel resultModel = null;
            if (result)
            {
                resultModel = new ResultViewModel()
                {
                    IsSuccess=true,
                    Message= "Deleted Successfully"
                };
            }
            else
            {
                resultModel = new ResultViewModel()
                {
                    IsSuccess = false,
                    Message = "Deleted Failed!!"
                };
            }
            return resultModel;
        }

        public JsonResult DeleteDepartment(int id)
        {

            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("Departments/DeleteDepartment?id="+id).Result;

            ResultViewModel isDeleted = response.Content.ReadAsAsync<ResultViewModel>().Result;

            var result= this.deleteResponse(isDeleted.IsSuccess, "MainDepartments"
                , "Categories", "#TblMainCategoriesBody", "EditDepartment", "DeleteDepartment"
                );

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteCategory(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("Departments/DeleteCategory?id=" + id).Result;

            ResultViewModel isDeleted = response.Content.ReadAsAsync<ResultViewModel>().Result;

            var result = this.deleteResponse(isDeleted.IsSuccess, "SubCategories"
                , "Categories", "#TblSubCategoriesBody", "EditCategory", "DeleteCategory"
                );

            return Json(result, JsonRequestBehavior.AllowGet);
        }

     





        //Edit 
 

        [HttpGet]
        public ActionResult EditCategory(int id)
        {

            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Departments/GetCategoryByID?id=" + id).Result;
            ResultViewModel result = response.Content.ReadAsAsync<ResultViewModel>().Result;

            JavaScriptSerializer js = new JavaScriptSerializer();
            CategoryViewModel catgVm = js.Deserialize<CategoryViewModel>(result.Data.ToString());



            catgVm.mainDepartmentsDropDown = new SelectList(this.getDepartments(), "Id", "Name");

           return PartialView("~/Areas/Admin/Views/categories/_EditCategory.cshtml", catgVm);

        }


        [HttpPost]
        public ActionResult EditCategory(CategoryViewModel category)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("Departments/EditCategory", category).Result;
 
            ResultViewModel result = response.Content.ReadAsAsync<ResultViewModel>().Result;

            return Json(result, JsonRequestBehavior.AllowGet);
         }


       

        [HttpPost]
        public ActionResult EditDepartmentVisibility(int id)
        {

            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Departments/EditDepartmentVisibility?id="+id).Result;
            ResultViewModel result = response.Content.ReadAsAsync<ResultViewModel>().Result;

            // this means current state on item
            bool visibility = result.IsSuccess;
            
            return Json(visibility, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditCategoryVisibility(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Departments/EditCategoryVisibility?id="+ id).Result;
            ResultViewModel result = response.Content.ReadAsAsync<ResultViewModel>().Result;

            // this means current state on item
            bool visibility = result.IsSuccess;


            return Json(visibility, JsonRequestBehavior.AllowGet);
        }



    }

}