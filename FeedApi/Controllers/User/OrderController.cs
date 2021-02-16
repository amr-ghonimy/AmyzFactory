
using AmyzFactory.Models;
using AmyzFeed.Business.interfaces;
using AutoMapper;
using FeedApi.Models;
using System.Web.Http;

namespace FeedApi.Controllers.User
{
    public class OrderController : ApiController
    {
        private IOrderUsersBusiness business;
        private readonly IMapper mapper;

        public OrderController(IOrderUsersBusiness _business)
        {
            this.business = _business;
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


    }
 
}
