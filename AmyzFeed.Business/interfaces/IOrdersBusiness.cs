using AmyzFactory.Models;
using AmyzFeed.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmyzFeed.Business.interfaces
{
   public interface IOrdersBusiness
    {
        List<OrderDomainModel> GetAllOrders();
        bool DeleteOrder(int orderID);

        int orderOfUserCount(string userID);
          List<OrderDomainModel> getAllOrders(int pageNo, int displayLength);

        List<OrderDetailsDomainModel> getOrderDetails(int orderID);
        int getAllOrdersCount();

        ResultDomainModel ConfirmOrder(OrderDomainModel model);
        ResultDomainModel EditOrder(OrderDomainModel model);


    }
}
