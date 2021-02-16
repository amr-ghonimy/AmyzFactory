using AmyzFactory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmyzFeed.Business.interfaces
{
    public interface IAuthBusiness
    {
         ResultDomainModel login(UserDomainModel model);

        ResultDomainModel Register(UserDomainModel model);
    }
}
