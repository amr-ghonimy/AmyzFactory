
using AmyzApi.Helpers;
using AmyzFactory.Models;
using AmyzFeed.Business;
using AmyzFeed.Business.interfaces;
using AmyzFeed.Domain;

using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace FeedApi.Controllers.User
{
    public class OrderController : ApiController
    {
        private IOrdersBusiness orderAdminBusiness;
 
        public OrderController(IOrdersBusiness _orderAdminBusiness)
        {
            this.orderAdminBusiness = _orderAdminBusiness;
         }


        private bool isAuthurized()
        {
            HttpRequestMessage request = this.ActionContext.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;


            if (authorization == null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(authorization.Parameter))
            {
                string token = authorization.Parameter;

                string isValid = TokenManager.validToken(token);

                if (string.IsNullOrEmpty(isValid))
                {
                    return false;
                }

                return true;
            }

            return false;


        }

        [HttpPost]
        public IHttpActionResult ConfirmOrder(OrderDomainModel model)
        {
            if (!isAuthurized())
            {
                return Content(System.Net.HttpStatusCode.BadRequest, new ResultDomainModel(false, "You Unauthorized"));
            }

            if (string.IsNullOrEmpty(model.Addreess))
            {
                return Content(System.Net.HttpStatusCode.BadRequest, new ResultDomainModel(false, "enter address"));
            }


      
            ResultDomainModel resultDm = this.orderAdminBusiness.ConfirmOrder(model);

            if (resultDm.IsSuccess)
            {

                return Ok(resultDm);
            }

            return Content(System.Net.HttpStatusCode.BadRequest, resultDm);
        }

        [HttpGet]
        public List<OrderDomainModel> GetAllOrders(int pageNo, int displayLength)
        {
            List<OrderDomainModel> dm = this.orderAdminBusiness.getAllOrders(pageNo, displayLength);
            return dm;
        }





        [HttpGet]
        public List<OrderDetailsDomainModel> GetOrderDetails(int id)
        {
            List<OrderDetailsDomainModel> dm = this.orderAdminBusiness.getOrderDetails(id);

            return dm;
        }

        [HttpGet]
        public int GetAllOrdersCount()
        {
            return this.orderAdminBusiness.getAllOrdersCount();

        }

    }

}
