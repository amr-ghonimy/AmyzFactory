using AmyzFactory.Models;
 using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
 using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace AmyzFactory.Controllers
{
    public class ProductController : Controller
    {

        private List<ProductViewModel> cardItemsList;

        public ActionResult ShowProducts(int categoryId,string categoryName)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Product/GetProductsByCategoryID?id=" + categoryId).Result;
            List<ProductViewModel> productsVm = response.Content.ReadAsAsync<List<ProductViewModel>>().Result;
            ViewBag.CategoryName = categoryName;
            return View("~/Views/Product/SearchInProducts.cshtml", productsVm);
        }



        public ActionResult ShowMaterials()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Product/GetAllMaterials").Result;
            List<ProductViewModel> materialsVm = response.Content.ReadAsAsync<List<ProductViewModel>>().Result;

            return View(materialsVm);
        }


        public ActionResult ShowProductsWithCategories()
        {
 
            // first get all categories
            HttpResponseMessage categoriesResponse = GlobalVariables.WebApiClient.GetAsync("Departments/GetDepartments").Result;
            List<CategoryViewModel> categoriesVm = categoriesResponse.Content.ReadAsAsync<List<CategoryViewModel>>().Result;

            // get All Products 

        
            HttpResponseMessage productsResponse = GlobalVariables.WebApiClient.GetAsync("Product/GetAllProducts").Result;
            List<ProductViewModel> productsVm = productsResponse.Content.ReadAsAsync<List<ProductViewModel>>().Result;

            ViewBag.Categories = categoriesVm;

            return View(productsVm);
        }


        public ActionResult ProductDetails(int id)
        {
            HttpResponseMessage categoriesResponse = GlobalVariables.WebApiClient.GetAsync("Product/GetProductById?id="+id).Result;
            ResultViewModel result = categoriesResponse.Content.ReadAsAsync<ResultViewModel>().Result;


            JavaScriptSerializer js = new JavaScriptSerializer();
            ProductViewModel productVm = js.Deserialize<ProductViewModel>(result.Data.ToString());

            return View(productVm);
        }


        public ActionResult ProductsPrices()
        {
            ViewBag.Current = "prices";

            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Product/GetProductsPrices").Result;
            List<ProductViewModel> productsList = response.Content.ReadAsAsync<List<ProductViewModel>>().Result;

            return View(productsList);
        }



        public JsonResult GetAllProducts()
        {
            HttpResponseMessage productsResponse = GlobalVariables.WebApiClient.GetAsync("Product/GetAllProducts").Result;
            List<ProductViewModel> productsVm = productsResponse.Content.ReadAsAsync<List<ProductViewModel>>().Result;

            return Json(productsVm, JsonRequestBehavior.AllowGet);

        }

 
        [HttpGet]
        public ActionResult SearchInProducts(string word)
        {
            HttpResponseMessage productsResponse = GlobalVariables.WebApiClient.GetAsync("Product/SearchInProducts?word=" + word).Result;
            List<ProductViewModel> productsVm = productsResponse.Content.ReadAsAsync<List<ProductViewModel>>().Result;
            ViewBag.CategoryName = "نتائج البحث";

            return View(productsVm);
        }


        public JsonResult GetProductsByCategoryID(int categoryID)
        {

            HttpResponseMessage productsResponse = GlobalVariables.WebApiClient.GetAsync("Product/GetProductsByCategoryID?id="+categoryID).Result;
            List<ProductViewModel> productsVm = productsResponse.Content.ReadAsAsync<List<ProductViewModel>>().Result;

            return Json(productsVm, JsonRequestBehavior.AllowGet);

        }


        public JsonResult AddToCart(int itemId)
        {
            // first step get product from db by its Id
            ProductViewModel tempItem = retrieveItemFromDb(itemId);

            ProductViewModel productVm = new ProductViewModel();


            cardItemsList = Session["cartItems"] as List<ProductViewModel>;

            if(cardItemsList==null)
            {
                this.cardItemsList = new List<ProductViewModel>();
            }



            if (cardItemsList.Any(model=>model.Id== itemId))
            {
                productVm = cardItemsList.Single(model => model.Id == itemId);

                productVm.Quantity = productVm.Quantity + 1;
                productVm.Total = productVm.Quantity * productVm.Price;
            }
            else
            {
                productVm.Id = tempItem.Id;
                productVm.Quantity = 1;
                productVm.Price = tempItem.Price;
                productVm.Total = tempItem.Price;
                productVm.ImageURL = tempItem.ImageURL;
                productVm.Name = tempItem.Name;
            
                cardItemsList.Add(productVm);
            }

            Session["cartCounter"] = cardItemsList.Count;
            Session["cartItems"] = cardItemsList;

            return Json(new { Success=true ,Counter= cardItemsList.Count },JsonRequestBehavior.AllowGet);

        }

        private ProductViewModel retrieveItemFromDb(int itemId)
        {
       
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Product/GetProductById?id=" + itemId).Result;
            ResultViewModel result = response.Content.ReadAsAsync<ResultViewModel>().Result;

            JavaScriptSerializer js = new JavaScriptSerializer();
            ProductViewModel item = js.Deserialize<ProductViewModel>(result.Data.ToString());

            return item;
        }

        [HttpPost]
        public ActionResult ShowItemDetails(ProductViewModel model)
        {

            return PartialView("_ProductDetailsModal", model);
        }
    }
}