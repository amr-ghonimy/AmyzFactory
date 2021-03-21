using AmyzFactory.App_Start;
using AmyzFactory.Models;
 
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace AmyzFactory.Areas.Admin.Controllers
{
    [AdminAuthorize(Roles = "Admins")]

    public class PricesController : Controller
    {
      
        // GET: Admin/Prices
        public ActionResult Index()
        {
            return View();
        }

        

    
      [HttpPost]
        public JsonResult UpdatePrices(IEnumerable<ProductViewModel> list)
        {

            HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("Product/UpdatePrices", list).Result;
            ResultViewModel resultVm = response.Content.ReadAsAsync<ResultViewModel> ().Result;

            return Json(resultVm, JsonRequestBehavior.AllowGet);

        }

        public JsonResult AllPrices(DataTableParams param)
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

                HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Product/GetSearchedProductCount?searchWord=" + param.sSearch).Result;
                totalCount = response.Content.ReadAsAsync<int>().Result;

            }
            else
            {
                productsList = this.getAllPrices(pageNo, param.iDisplayLength);
                 
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
        private List<ProductViewModel> getAllPrices(int pageNo, int displayLengt)
        {
            string url = "Product/getAllPrices?pageNo=" + pageNo + "&displayLengt=" + displayLengt;

            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync(url).Result;
            List<ProductViewModel> list = response.Content.ReadAsAsync<List<ProductViewModel>>().Result;

            return list;
        }


        private List<ProductViewModel> searchInProducts(string word, int pageNo, int displayLength)
        {
            string url = "Product/SearchInProducts?word=" + word + "&pageNo=" + pageNo + "&displayLength=" + displayLength;

            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync(url).Result;
            List<ProductViewModel> list = response.Content.ReadAsAsync<List<ProductViewModel>>().Result;

            return list;
        }


    }

 
}