using System.Collections.Generic;
using System.Web.Mvc;
using AmyzFactory.Models;
using AmyzFactory.App_Start;
using System.Net.Http;
using System.Web.Script.Serialization;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace AmyzFactory.Areas.Admin.Controllers
{

    [AdminAuthorize(Roles = "Admins")]

    public class ProductsController : BaseController
    {

 

        private List<CategoryViewModel> getCategories()
        {
            base.ChangeHeader();
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Departments/GetCategories").Result;

            var list = response.Content.ReadAsAsync<List<CategoryViewModel>>().Result;

            return list;
        }

        private List<ProductViewModel> getAllProducts(int pageNo, int displayLength)
        {
            string url = "Product/GetAllProducts?pageNo=" + pageNo + "&displayLength=" + displayLength;

            string tokenNumber = Session[SessionsModel.Token]?.ToString();
            GlobalVariables.WebApiClient.DefaultRequestHeaders.Clear();
            GlobalVariables.WebApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
               "Bearer", tokenNumber);

            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync(url).Result;

            var list = response.Content.ReadAsAsync<List<ProductViewModel>>().Result;
            return list;
        }

        private List<ProductViewModel> searchInProducts(string word, int pageNo, int displayLength)
        {
            string url = "Product/SearchInProducts?word=" + word + "&pageNo=" + pageNo + "&displayLength=" + displayLength;
            base.ChangeHeader();
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

                base.ChangeHeader();
                HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Product/GetSearchedProductCount?searchWord="+ param.sSearch).Result;
                totalCount = response.Content.ReadAsAsync<int>().Result;

            }
            else
            {
                productsList = this.getAllProducts(pageNo, param.iDisplayLength);



                base.ChangeHeader();
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

      


        public JsonResult Delete(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("Product/DeleteProduct?id="+ id).Result;
            ResultViewModel resultModelVm = response.Content.ReadAsAsync<ResultViewModel>().Result;

            return Json(resultModelVm, JsonRequestBehavior.AllowGet);
        }



        public ResultViewModel UploadProductImage(HttpPostedFileBase imageFile, string apiPath)
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
                string result = response.Content.ReadAsStringAsync().Result;
                ResultViewModel resultVm = JsonConvert.DeserializeObject<ResultViewModel>(result);

 
                return resultVm;
            }
        }


        private ResultViewModel checkIfModelExistsInDb(ProductViewModel model, string apiPath)
        {
            string url = apiPath + "?name=" + model.Name;
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync(url).Result;
            ResultViewModel result = response.Content.ReadAsAsync<ResultViewModel>().Result;

            return result;
        }


        [HttpPost]
        public JsonResult Create(ProductViewModel model)
        {


            if (checkIfModelExistsInDb(model, "Product/isProductExists").IsSuccess)
            {
                ResultViewModel resultVm = new ResultViewModel { IsSuccess = false, Message = "Product is already Exists" };
                model.ResponseResult = resultVm;
                return Json(model, JsonRequestBehavior.AllowGet);
            }


            // start upload image
            if (model.ImageFile != null)
            {
                // this means user select image to upload image
                HttpPostedFileBase imageFile = model.ImageFile;
                ResultViewModel imageUploadResult = this.UploadProductImage(imageFile, "Product/UploadImage");


                if (imageUploadResult.IsSuccess == false)
                {
                    model.ResponseResult = imageUploadResult;
                    return Json(model, JsonRequestBehavior.AllowGet);
                }

                model.ImageURL = (string)imageUploadResult.Data;
            }

            
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Product/Create", model).Result;
            var result = response.Content.ReadAsAsync<ResultViewModel>().Result;

            model.Id = result.modelID;
            model.ResponseResult = result;
            model.ImageFile = null;

            return Json(model, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            ProductViewModel product = this.getProductByID(id);

            ViewBag.Categories = new SelectList(this.getCategories(), "Id", "Name");


            return PartialView("~/Areas/Admin/Views/Products/_EditProduct.cshtml", product);
        }


        private ProductViewModel getProductByID(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Product/GetProductByID?id=" + id).Result;

            ResultViewModel result = response.Content.ReadAsAsync<ResultViewModel>().Result;


            ProductViewModel product = null;

            if (result.Data != null)
            {
                string jsonString = result.Data.ToString();

                JavaScriptSerializer js = new JavaScriptSerializer();
                product = js.Deserialize<ProductViewModel>(jsonString);
            }

            return product;
        }

        [HttpGet]
        public ActionResult ProductDetails(int id)
        {
            ProductViewModel product = this.getProductByID(id);

            return View(product);
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