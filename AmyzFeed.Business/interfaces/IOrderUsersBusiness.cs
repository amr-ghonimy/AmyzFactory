using AmyzFactory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmyzFeed.Business.interfaces
{
   public interface IOrderUsersBusiness
    {
        ResultDomainModel ConfirmOrder(OrderDomainModel model);
        ResultDomainModel DeleteOrder(int orderID);
        ResultDomainModel EditOrder(OrderDomainModel model);



    }
}
