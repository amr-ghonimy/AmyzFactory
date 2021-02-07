
using System;
using System.Collections.Generic;

using System.Web.Mvc;
 
using AmyzFactory.Models;
 
using AmyzFactory.App_Start;
using System.Net.Http;

namespace AmyzFactory.Areas.Admin.Controllers
{
    [AdminAuthorize(Roles = "Admins")]

    public class ProductsController : Controller
    {
      



        private List<CategoryViewModel> getCategories()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Products/GetCategories").Result;

            var list = response.Content.ReadAsAsync<List<CategoryViewModel>>().Result;

            return list;
        }

        private List<ProductViewModel> getAllProducts(int pageNo,int displayLength)
        {
            string url = "Products/GetAllProducts?pageNo=" + pageNo + "&displayLength=" + displayLength;

            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync(url).Result;

            var list = response.Content.ReadAsAsync<List<ProductViewModel>>().Result;
            return list;
        }

        private List<ProductViewModel> searchInProducts(string word, int pageNo, int displayLength)
        {
            string url = "Products/SearchInProducts?word=" + word + "&pageNo=" + pageNo + "&displayLength=" + displayLength;

            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync(url).Result;

            var list = response.Content.ReadAsAsync<List<ProductViewModel>>().Result;
            return list;
        }
        private List<ProductViewModel> getAllMaterials(int pageNo, int displayLength)
        {
            string url = "Products/GetAllMaterials?pageNo=" + pageNo + "&displayLength=" + displayLength;

            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync(url).Result;

            var list = response.Content.ReadAsAsync<List<ProductViewModel>>().Result;
            return list;
        }
        private List<ProductViewModel> searchInMaterials(string word, int pageNo, int displayLength)
        {
            string url = "Products/SearchInMaterials?word=" + word + "&pageNo="+ pageNo + "&displayLength=" + displayLength;


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


                HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Products/GetSearchedProductCount?searchWord="+ param.sSearch).Result;
                totalCount = response.Content.ReadAsAsync<int>().Result;

            }
            else
            {
                productsList = this.getAllProducts(pageNo, param.iDisplayLength);
              

                HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Products/GetAllProductsCount").Result;
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

            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Products/Delete",id).Result;
            ResultViewModel resultModelVm = response.Content.ReadAsAsync<ResultViewModel>().Result;

            return Json(resultModelVm, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Create(ProductViewModel model)
        {
            // start upload image

            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Products/Create", model).Result;
            model = response.Content.ReadAsAsync<ProductViewModel>().Result;

            
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Products/GetProductByID?id=" + id).Result;

            ProductViewModel prodVm = response.Content.ReadAsAsync<ProductViewModel>().Result;

            ViewBag.Categories = new SelectList(this.getCategories(), "Id", "Name");

            return PartialView("~/Areas/Admin/Views/Products/_EditProduct.cshtml", prodVm);
        }


        [HttpPost]
        public ActionResult EditProduct(ProductViewModel product)
        {

            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Products/EditProduct", product).Result;
            ResultViewModel result = response.Content.ReadAsAsync<ResultViewModel>().Result;

            return Json(result, JsonRequestBehavior.AllowGet);
        }


    }
}