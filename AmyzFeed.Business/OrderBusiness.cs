using AmyzFeed.Business.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmyzFactory.Models;
using AmyzFeed.Repository;
using AmyzFeed.Repository.Infrastructure.Contract;
using AmyzFeed.Repository.Data;
using AmyzFeed.Domain;

namespace AmyzFeed.Business
{
    public class OrderBusiness : IOrdersBusiness
    {
        private OrderRepository repository;
        private OrderDetailsRepository detailsRepository;
        private ProductRepository productsRepository;

        private ResultDomainModel resultModel;
        private IUnitOfWork unitOfWork;
        public OrderBusiness(IUnitOfWork _unitOfWork, ResultDomainModel _resultModel)
        {
            this.unitOfWork = _unitOfWork;
            this.resultModel = _resultModel;
            this.repository = new OrderRepository(this.unitOfWork);
            this.detailsRepository = new OrderDetailsRepository(this.unitOfWork);
            this.productsRepository = new ProductRepository(this.unitOfWork);
        }


        public bool DeleteOrder(int orderID)
        {
            throw new NotImplementedException();
        }

        public List<OrderDomainModel> GetAllOrders()
        {
            List<Order> orders = this.repository.GetAll().ToList();
            List<OrderDomainModel> list = new List<OrderDomainModel>();

            if (orders.Count > 0)
            {
                foreach (var item in orders)
                {
 
                    OrderDomainModel temp = new OrderDomainModel()
                    {
                        OrderDate = item.OrderDate,
                        OrderID = item.OrderID,
                        OrderNumber = item.OrderNumber,
                        ItemsCount= item.ItemsCount,
                        ShippingCost=item.ShippingCost,
                        OrderTotalPrice=item.TotalCost
                    };

                    list.Add(temp);
                }
            }


            return list;
        }

        public List<OrderDomainModel> getAllOrders(int pageNo, int displayLength)
        {
            return repository.GetAll()
                             .OrderBy(x => x.OrderDate)
                               .Skip((pageNo - 1) * displayLength)
                               .Take(displayLength)
                       .Select(x => new OrderDomainModel()
                       {
                           OrderID = x.OrderID,
                           UserID = x.UserID,
                           OrderNumber = x.OrderNumber,
                           OrderTotalPrice = x.TotalCost,
                           ItemsCount=x.ItemsCount,
                           OrderDate=x.OrderDate,
                           ShippingCost=x.ShippingCost
                       }).ToList();
        }

        public int getAllOrdersCount()
        {
            return repository.GetAll().Count();
        }



        public List<OrderDetailsDomainModel> getOrderDetails(int orderID)
        {

            List<OrderDetailsDomainModel> orderDetails = this.detailsRepository.GetAll(x => x.OrderID == orderID)
                .Select(m=>new OrderDetailsDomainModel()
                {
                    ItemID=m.ProductID,
                    Quantity=m.Quantity,
                    Price=m.Price,
                    Total=m.Total
                })
                .ToList();

            foreach (var item in orderDetails)
            {
                Product product = this.productsRepository.GetAll(x => x.ID == item.ItemID).FirstOrDefault();
                item.ItemImg = product.Image;
                item.ItemName = product.Name;
            }


            return orderDetails;

        }

        public int orderOfUserCount(string userID)
        {
           return this.repository.Count(x => x.UserID == userID);
        }
    }
}
