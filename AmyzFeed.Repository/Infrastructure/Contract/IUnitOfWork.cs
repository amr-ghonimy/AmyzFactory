using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmyzFeed.Repository.Infrastructure.Contract
{
   public interface IUnitOfWork : IDisposable
    {
        DbContext Db { get; }

    }
}
