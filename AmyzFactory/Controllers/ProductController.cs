using AmyzFactory.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

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
            return View(productsVm);
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


        public ActionResult ProductsPrices()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Product/GetProductsPrices").Result;
            List<ProductViewModel> productsList = response.Content.ReadAsAsync<List<ProductViewModel>>().Result;

            return View(productsList);
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
       
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Product/RetrieveItemFromDb"+itemId).Result;
            ProductViewModel item = response.Content.ReadAsAsync<ProductViewModel>().Result;

            return item;
        }

        [HttpPost]
        public ActionResult ShowItemDetails(ProductViewModel model)
        {

            return PartialView("_ProductDetailsModal", model);
        }
    }
}