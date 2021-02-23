﻿using AmyzFactory.App_Start;
using AmyzFactory.Models;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace AmyzFactory.Areas.Admin.Controllers
{
    [AdminAuthorize(Roles = "Admins")]
    public class CategoriesController : Controller
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
        public PartialViewResult _Technicals()
        {
            return PartialView("~/Areas/Admin/Views/Categories/partial/_Technicals.cshtml");
        }



        private List<CategoryViewModel> getDepartments()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Departments/GetDepartments").Result;
            List<CategoryViewModel> departments = response.Content.ReadAsAsync<List<CategoryViewModel>>().Result;
 
            return departments;
        }

        private List<CategoryViewModel> getCategories()
        {
            HttpResponseMessage response =  GlobalVariables.WebApiClient.GetAsync("Departments/GetCategories").Result;
            List<CategoryViewModel> categories = response.Content.ReadAsAsync<List<CategoryViewModel>>().Result;

            return categories;
        }


        private List<CategoryViewModel> getTechnecals()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Departments/GetTechnicals").Result;
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

        [HttpGet]
        public JsonResult Technicals()
        {
            var technicals = getTechnecals();

            return Json(technicals, JsonRequestBehavior.AllowGet);

         }




        // Create

 
        [HttpPost]
        public JsonResult CreateDepartment(CategoryViewModel categoryModel)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Departments/CreateDepartment", categoryModel).Result;
            ResultViewModel result = response.Content.ReadAsAsync<ResultViewModel>().Result;

            categoryModel.Id = result.modelID;
            categoryModel.Result = result;

            return Json(categoryModel, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult CreateCategory(CategoryViewModel categoryModel)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Departments/CreateCategory", categoryModel).Result;
            ResultViewModel result = response.Content.ReadAsAsync<ResultViewModel>().Result;

            categoryModel.Id = result.modelID;
            categoryModel.Result = result;

            return Json(categoryModel, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult CreateTechnical(CategoryViewModel categoryModel)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Departments/CreateTechnical", categoryModel).Result;
            ResultViewModel result = response.Content.ReadAsAsync<ResultViewModel>().Result;

            categoryModel.Id = result.modelID;
            categoryModel.Result = result;

            return Json(categoryModel, JsonRequestBehavior.AllowGet);
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

        public JsonResult DeleteTechnical(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("Departments/DeleteTechnical?id=" + id).Result;

            ResultViewModel isDeleted = response.Content.ReadAsAsync<ResultViewModel>().Result;

            var result = this.deleteResponse(isDeleted.IsSuccess, "Technicals"
                , "Categories", "#TbltechniclasBody", "EditTechnical", "DeleteTechnical"
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
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Categories/EditCategory", category).Result;

            category = response.Content.ReadAsAsync<CategoryViewModel>().Result;

            return Json(category, JsonRequestBehavior.AllowGet);
         }


        [HttpGet]
        public ActionResult EditTechnical(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Categories/GetTechnicalByID?id=" + id).Result;
            CategoryViewModel techVm = response.Content.ReadAsAsync<CategoryViewModel>().Result;

            return PartialView("~/Areas/Admin/Views/categories/_EditTechnical.cshtml" , techVm);
        }

        [HttpPost]
        public ActionResult EditTechnical(CategoryViewModel category)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Categories/EditTechnical", category).Result;

            category = response.Content.ReadAsAsync<CategoryViewModel>().Result;

            return Json(category, JsonRequestBehavior.AllowGet);

           }


        [HttpPost]
        public ActionResult EditDepartmentVisibility(int id)
        {

            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Categories/EditDepartmentVisibility",id).Result;
            bool visibility = response.Content.ReadAsAsync<bool>().Result;

            
            return Json(visibility, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditCategoryVisibility(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Categories/EditCategoryVisibility", id).Result;
            bool visibility = response.Content.ReadAsAsync<bool>().Result;


            return Json(visibility, JsonRequestBehavior.AllowGet);
        }



    }

}