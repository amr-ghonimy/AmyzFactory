using AmyzFactory.App_Start;
using AmyzFactory.Models;

using AmyzFeed.Repository.Data;
 
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
 
using System.Collections.Generic;
 
using System.Net.Http;
 
using System.Web.Mvc;

namespace AmyzFactory.Areas.Admin.Controllers
{
    [AdminAuthorize(Roles = "Admins")]

    public class OrdersController : Controller
    {
        // GET: Admin/Orders
      

        public ActionResult Index()
        {
            return View();
        }


        public PartialViewResult _UserInfo(string UserID)
        {
           
            ApplicationDbContext db = new ApplicationDbContext();

            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(db);

            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);

            var user = userManager.FindById(UserID);
            
            
            

          //  HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Order/GetOrderOfUserCount"+UserID).Result;
           // int ordersCount = response.Content.ReadAsAsync<int>().Result;
           // ViewBag.NumberOfOrders = ordersCount;


            return PartialView("~/Areas/Admin/Views/Orders/_UserInfo.cshtml", user);
        }
        public ActionResult AllOrders(DataTableParams param)
        {

            List<OrderViewModel> ordersListVm = new List<OrderViewModel>();


            int pageNo = 1;

            if (param.iDisplayStart >= param.iDisplayLength)
            {

                pageNo = (param.iDisplayStart / param.iDisplayLength) + 1;

            }


            int totalCount = 0;


            ordersListVm = this.getAllOrders(pageNo, param.iDisplayLength);


            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Order/GetAllOrdersCount").Result;
            totalCount = response.Content.ReadAsAsync<int>().Result;
            



            return Json(new
            {
                aaData = ordersListVm,
                sEcho = param.sEcho,
                iTotalDisplayRecords = totalCount,
                iTotalRecords = totalCount
            }
                    , JsonRequestBehavior.AllowGet);

        }

        public ActionResult OrderDetails(int orderID,string userID)
        {
            ViewBag.UserID = userID;
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Order/GetOrderDetails?id=" + orderID).Result;
            List<OrderDetailsViewModel> orderDetailsVm = response.Content.ReadAsAsync<List<OrderDetailsViewModel>>().Result;
            
            return View(orderDetailsVm);
        }


        private List<OrderViewModel> getAllOrders(int pageNo, int displayLength)
        {
            string url = "Order/GetAllOrders?pageNo=" + pageNo + "&displayLength=" + displayLength;
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync(url).Result;
            List<OrderViewModel> newList = response.Content.ReadAsAsync<List<OrderViewModel>>().Result;
            return newList;
        }



    }
}