using AmyzFactory.Models;

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using AmyzFactory.App_Start;
using Microsoft.AspNet.Identity;
using System.Net.Http;
using System.Web.Script.Serialization;
using AmyzFeed.Repository.Data;

namespace AmyzFactory.Controllers
{
    public class OrderController : Controller
    {

      

        // GET: Order
         public ActionResult Index()
        {
            var itemsList = Session["cartItems"] as List<ProductViewModel>;
            return View(itemsList);
        }



        [HttpGet]
        [UserAuthorize(Roles = "Users")]
        public ActionResult ContinueOrder()
        {
            ApplicationUser user = null;
            OrderViewModel order = null;


            // first get current user data
            string userID = User.Identity.GetUserId();
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Accounts/GetUserByID?id=" + userID).Result;
            ResultViewModel result = response.Content.ReadAsAsync<ResultViewModel>().Result;

            if (result.Data != null)
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                user = js.Deserialize<ApplicationUser>(result.Data.ToString());
                order = new OrderViewModel
                {
                    Addreess=user.Address,
                    Governorate = user.Governorate,
                    Phone=user.PhoneNumber
                };
            }
          
            var cardItemsList = Session["cartItems"] as List<ProductViewModel>;
            ViewBag.Products = cardItemsList;



            return View(order);
        }
        [HttpGet]
        public PartialViewResult _GetOrderProducts()
        {
            var cardItemsList = Session["cartItems"] as List<ProductViewModel>;

            return PartialView("~/Views/Order/OrderHeader.cshtml", cardItemsList);
        }
        [HttpPost]
        public ActionResult ContinueOrder(IEnumerable<ProductViewModel> list)
        {
 
            Session["cartItems"] = list;
            Session["cartCounter"] = list.Count();



            return Json("/Order/ContinueOrder", JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [UserAuthorize(Roles = "Users")]
        public ActionResult ConfirmOrder(OrderViewModel orderHeaderModel)
        {
            List<OrderDetailsViewModel> orderDetails = new List<OrderDetailsViewModel>();
            var list = Session["cartItems"] as List<ProductViewModel>;


            var totalPrice = 0.0;
            foreach (var item in list)
            {
                OrderDetailsViewModel orderDetailsVm = new OrderDetailsViewModel()
                {
                    ItemID=item.Id,
                    Price=item.Price,
                    Quantity=item.Quantity,
                    Total=item.Total
                };

                totalPrice += item.Total;

                orderDetails.Add(orderDetailsVm);
            }

            var user = User.Identity.GetUserId();

            OrderViewModel orderVm = new OrderViewModel()
            {
                OrderDetailsList = orderDetails,
                UserID = user,
                ShippingCost = 30,
                ItemsCount= orderDetails.Count,
                Addreess=orderHeaderModel.Addreess,
                Notes=orderHeaderModel.Notes,
                OrderTotalPrice= totalPrice,
                OrderNumber = string.Format("{0:ddmmyyyHHmmsss}",DateTime.Now)
            };

            // confirm order from api
           ResultViewModel resultVm= this.confirmOrderFromApi(orderVm);


            if (resultVm.IsSuccess)
            {
                // remove products from session
                Session["cartCounter"] = 0;
                Session["cartItems"] = null;
            }

            return Json(resultVm, JsonRequestBehavior.AllowGet);
        }


        private ResultViewModel confirmOrderFromApi(OrderViewModel order)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Order/ConfirmOrder", order).Result;
            return response.Content.ReadAsAsync<ResultViewModel>().Result;
        }
        public JsonResult Delete(int itemID)
        {
            var cardItemsList = Session["cartItems"] as List<ProductViewModel>;

            var itemVm = cardItemsList.Single(model => model.Id == itemID);

            bool result = cardItemsList.Remove(itemVm);

            ResultViewModel resultVm = new ResultViewModel();

            resultVm.IsSuccess = result;
            resultVm.Message = cardItemsList.Count.ToString(); //to pass cart items count to view

            Session["cartCounter"] = cardItemsList.Count;
            Session["cartItems"] = cardItemsList;

            return Json(resultVm, JsonRequestBehavior.AllowGet);
        }
    }
}       