using AmyzFeed.Business.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmyzFactory.Models;
using AmyzFeed.Repository.Infrastructure.Contract;
using AmyzFeed.Repository;
using AmyzFeed.Domain;
using AmyzFeed.Repository.Data;

namespace AmyzFeed.Business
{
   public class OrderUserBusiness : IOrderUsersBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private ResultDomainModel resultModel;
        private OrderRepository orderRepo;
        private OrderDetailsRepository detailsRepo;

        public OrderUserBusiness(IUnitOfWork _unitOfWork, ResultDomainModel _resultModel)
        {
            this.unitOfWork = _unitOfWork;
            this.resultModel = _resultModel;
            this.orderRepo = new OrderRepository(this.unitOfWork);
            this.detailsRepo = new OrderDetailsRepository(this.unitOfWork);
        }

        private ResultDomainModel initResult(bool isSuccess,string message)
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
                            OrderID=order.OrderID,
                            ProductID= item.ItemID,
                            Price=item.Price,
                            Quantity=item.Quantity,
                            Total=item.Total
                        };

                        try
                        {
                            this.detailsRepo.Insert(detail);
                        }
                        catch (Exception e)
                        {
                            return 0;
                            throw;
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
                OrderDate = model.OrderDate,
                OrderNumber = model.OrderNumber,
                ShippingCost = model.ShippingCost,
                ItemsCount=model.ItemsCount,
                Address=model.Addreess,
                Note=model.Notes,
                TotalCost=model.OrderTotalPrice,
                UserID = model.UserID
            };

            try
            {
                this.orderRepo.Insert(order);
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
            this.orderRepo.Delete(x => x.OrderID == orderId);
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

                }else
                {
                    return initResult(false, "هناك مشكلة فى ارسال الطلب");
                }


            }

           return initResult(true, "تم ارسال طلبك بنجاح .. سنتواصل معك");
        }

        public ResultDomainModel DeleteOrder(int orderID)
        {
            throw new NotImplementedException();
        }

        public ResultDomainModel EditOrder(OrderDomainModel model)
        {
            throw new NotImplementedException();
        }
    }
}
