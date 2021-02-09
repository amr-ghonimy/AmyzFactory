﻿
using System;
using System.Collections.Generic;

using System.Web.Mvc;

using AmyzFactory.Models;

using AmyzFactory.App_Start;
using System.Net.Http;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using System.Web.Script.Serialization;

namespace AmyzFactory.Areas.Admin.Controllers
{
    [AdminAuthorize(Roles = "Admins")]

    public class ProductsController : Controller
    {

        private List<CategoryViewModel> getCategories()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Departments/GetCategories").Result;

            var list = response.Content.ReadAsAsync<List<CategoryViewModel>>().Result;

            return list;
        }

        private List<ProductViewModel> getAllProducts(int pageNo, int displayLength)
        {
            string url = "Product/GetAllProducts?pageNo=" + pageNo + "&displayLength=" + displayLength;

            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync(url).Result;

            var list = response.Content.ReadAsAsync<List<ProductViewModel>>().Result;
            return list;
        }

        private List<ProductViewModel> searchInProducts(string word, int pageNo, int displayLength)
        {
            string url = "Product/SearchInProducts?word=" + word + "&pageNo=" + pageNo + "&displayLength=" + displayLength;

            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync(url).Result;

            var list = response.Content.ReadAsAsync<List<ProductViewModel>>().Result;
            return list;
        }
        private List<ProductViewModel> getAllMaterials(int pageNo, int displayLength)
        {
            string url = "Product/GetAllMaterials?pageNo=" + pageNo + "&displayLength=" + displayLength;

            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync(url).Result;

            var list = response.Content.ReadAsAsync<List<ProductViewModel>>().Result;
            return list;
        }
        private List<ProductViewModel> searchInMaterials(string word, int pageNo, int displayLength)
        {
            string url = "Product/SearchInMaterials?word=" + word + "&pageNo="+ pageNo + "&displayLength=" + displayLength;


            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync(url).Result;

            var list = response.Content.ReadAsAsync<List<ProductViewModel>>().Result;
            return list;

        }

        [HttpGet]
        public ActionResult Products()
        {

            CategoryViewModel categoryModel = new CategoryViewModel
            {
                mainDepartmentsDropDown = new SelectList(getCategories(), "Id", "Name")
            };

            ViewBag.CatModel = categoryModel;

            return View();
        }

        [HttpGet]
        public ActionResult Materials()
        {
            return View();
        }
        
        [HttpGet]
        public JsonResult AllProducts(DataTableParams param)
        {
            List<ProductViewModel> productsList = new List<ProductViewModel>();

            int pageNo = 1;

            if (param.iDisplayStart >= param.iDisplayLength)
            {

                pageNo = (param.iDisplayStart / param.iDisplayLength) + 1;

            }

            int totalCount = 0;

            if (param.sSearch != null)
            {
                productsList = this.searchInProducts(param.sSearch, pageNo, param.iDisplayLength);


                HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Product/GetSearchedProductCount?searchWord="+ param.sSearch).Result;
                totalCount = response.Content.ReadAsAsync<int>().Result;

            }
            else
            {
                productsList = this.getAllProducts(pageNo, param.iDisplayLength);
              

                HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Product/GetAllProductsCount").Result;
                totalCount = response.Content.ReadAsAsync<int>().Result;
            }




            return Json(new
            {
                aaData = productsList,
                sEcho = param.sEcho,
                iTotalDisplayRecords = totalCount,
                iTotalRecords = totalCount
            }
           , JsonRequestBehavior.AllowGet);

         }

        [HttpGet]
        public JsonResult AllMaterials(DataTableParams param)
        {
            List<ProductViewModel> productsList = new List<ProductViewModel>();

            int pageNo = 1;

            if (param.iDisplayStart >= param.iDisplayLength)
            {

                pageNo = (param.iDisplayStart / param.iDisplayLength) + 1;

            }

            int totalCount = 0;

            if (param.sSearch != null)
            {
                productsList = this.searchInMaterials(param.sSearch, pageNo, param.iDisplayLength);

                HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Products/GetSearchedMaterialsCount?searchWord=" + param.sSearch).Result;
                totalCount = response.Content.ReadAsAsync<int>().Result;
            }
            else
            {
                productsList = this.getAllMaterials(pageNo, param.iDisplayLength);

                HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Products/GetAllMaterialsCount").Result;
                totalCount = response.Content.ReadAsAsync<int>().Result;
            }




            return Json(new
            {
                aaData = productsList,
                sEcho = param.sEcho,
                iTotalDisplayRecords = totalCount,
                iTotalRecords = totalCount
            }
           , JsonRequestBehavior.AllowGet);

        }

        public JsonResult Delete(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("Product/DeleteProduct?id="+ id).Result;
            ResultViewModel resultModelVm = response.Content.ReadAsAsync<ResultViewModel>().Result;

            return Json(resultModelVm, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Create(ProductViewModel model)
        {
            // start upload image

            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Product/Create", model).Result;
            var result = response.Content.ReadAsAsync<ResultViewModel>().Result;

            model.Id = result.modelID;
            model.ResponseResult = result;
            
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Product/GetProductByID?id=" + id).Result;

            ResultViewModel result = response.Content.ReadAsAsync<ResultViewModel>().Result;

            ViewBag.Categories = new SelectList(this.getCategories(), "Id", "Name");

            ProductViewModel products = null;

            if (result.Data != null)
            {
                string jsonString = result.Data.ToString();

                JavaScriptSerializer js = new JavaScriptSerializer();
                products = js.Deserialize<ProductViewModel>(jsonString);
            }
            return PartialView("~/Areas/Admin/Views/Products/_EditProduct.cshtml", products);
        }


        [HttpPost]
        public ActionResult EditProduct(ProductViewModel product)
        {

            HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("Product/EditProduct", product).Result;
            ResultViewModel result = response.Content.ReadAsAsync<ResultViewModel>().Result;

            return Json(result, JsonRequestBehavior.AllowGet);
        }


    }
}