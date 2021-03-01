
using AmyzFactory.Models;
using AmyzFeed.Business;
using AmyzFeed.Business.interfaces;
using AutoMapper;
using FeedApi.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace FeedApi.Controllers.User
{
    public class OrderController : ApiController
    {
        private IOrdersBusiness orderAdminBusiness;
        private IOrderUsersBusiness business;
        private readonly IMapper mapper;

        public OrderController(IOrderUsersBusiness _business, IOrdersBusiness _orderAdminBusiness)
        {
            this.business = _business;
            this.orderAdminBusiness = _orderAdminBusiness;
            this.mapper = AutoMapperConfig.Mapper;
        }

        [HttpPost]
        public ResultDomainModel ConfirmOrder(OrderViewModel model)
        {
            OrderDomainModel orderDm = this.mapper.Map<OrderDomainModel>(model);

            ResultDomainModel resultDm = this.business.ConfirmOrder(orderDm);

            ResultDomainModel resultVm = this.mapper.Map<ResultDomainModel>(resultDm);

            return resultVm;
        }
        [HttpGet]
        public List<OrderDomainModel> getAllOrders(int pageNo, int displayLength)
        {
            List<OrderDomainModel> dm = this.orderAdminBusiness.getAllOrders(pageNo,displayLength);
            return dm;
        }

        [HttpGet]
        public int GetAllOrdersCount()
        {
          return this.orderAdminBusiness.getAllOrdersCount();

        }

    }
 
}
