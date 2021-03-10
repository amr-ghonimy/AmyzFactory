using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmyzFeed.Repository.Data
{
    public class AmyzDBContext:DbContext
    {

        public AmyzDBContext():base("AmyzDBContext")
        {

        }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Technical> Technicals { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Questionnaire> Questionnaires { get; set; }
        public DbSet<FeedsProgram> FeedsPrograms { get; set; }


    }
}
