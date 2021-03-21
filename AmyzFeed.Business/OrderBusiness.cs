using AmyzFeed.Business.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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



        private ResultDomainModel initResult(bool isSuccess, string message)
        {
            return new ResultDomainModel() { IsSuccess = isSuccess, Message = message };
        }



        private int uplaodOrderDetails(OrderDomainModel order)
        {
            if (order != null)
            {
                List<OrderDetailsDomainModel> orderDetails = order.OrderDetailsList;
                if (orderDetails != null && orderDetails.Count > 0)
                {
                    foreach (var item in orderDetails)
                    {
                        OrderDetail detail = new OrderDetail()
                        {
                            OrderID = order.OrderID,
                            ProductID = item.ItemID,
                            Price = item.Price,
                            Quantity = item.Quantity,
                            Total = item.Total
                        };

                        try
                        {
                            this.detailsRepository.Insert(detail);
                        }
                        catch (Exception e)
                        {
                            return 0;
                     
                        }

                    }
                    return 1;
                }

            }




            return 0;
        }

 

        private int uploadOrderHeader(OrderDomainModel model)
        {
            Order order = new Order()
            {
                OrderDate = DateTime.Now,
                OrderNumber = string.Format("{0:ddmmyyyHHmmsss}", DateTime.Now),
                ShippingCost = 30,
                ItemsCount = model.ItemsCount,
                Address = model.Addreess,
                Note = model.Notes,
                TotalCost = model.OrderTotalPrice,
                UserID = model.UserID
            };

            try
            {
                this.repository.Insert(order);
                model.OrderID = order.OrderID;
                return 1;
            }
            catch (Exception)
            {

                return 0;
            }
        }

        private void deleteOrderHeader(int orderId)
        {
            this.repository.Delete(x => x.OrderID == orderId);
        }


        public ResultDomainModel ConfirmOrder(OrderDomainModel model)
        {
            if (model != null)
            {
                model.OrderDate = DateTime.Now;
                int uploadOrderHeaderResult = this.uploadOrderHeader(model);

                if (uploadOrderHeaderResult > 0)
                {
                    // lets uplaod order details

                    int uploadOrderDetailsResult = this.uplaodOrderDetails(model);

                    if (uploadOrderDetailsResult <= 0)
                    {
                        // remove Order header from Db 
                        deleteOrderHeader(model.OrderID);
                        return initResult(false, "هناك مشكلة فى ارسال الطلب");
                    }

                }
                else
                {
                    return initResult(false, "هناك مشكلة فى ارسال الطلب");
                }


            }

            return initResult(true, "تم ارسال طلبك بنجاح .. سنتواصل معك");
        }

        public bool DeleteOrder(int orderID)
        {
            throw new NotImplementedException();
        }

        public ResultDomainModel EditOrder(OrderDomainModel model)
        {
            throw new NotImplementedException();
        }

    }
}
