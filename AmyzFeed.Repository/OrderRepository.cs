using AmyzFeed.Repository.Data;
using AmyzFeed.Repository.Infrastructure;
using AmyzFeed.Repository.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmyzFeed.Repository
{
   public class OrderRepository : BaseRepository<Order>
    {
        public OrderRepository(IUnitOfWork unitofWork) : base(unitofWork) { }

    }
}
