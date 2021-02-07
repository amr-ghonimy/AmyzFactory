using AmyzFeed.Repository.Data;
using AmyzFeed.Repository.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmyzFeed.Repository.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AmyzDBContext _dbContext;

        public UnitOfWork()
        {
            _dbContext = new AmyzDBContext();
        }

        public DbContext Db
        {
            get { return _dbContext; }
        }

        public void Dispose()
        {
        }
    }
}
