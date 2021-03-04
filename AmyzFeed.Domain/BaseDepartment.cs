using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmyzFeed.Domain
{
    public class BaseDepartment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Boolean visibility { get; set; }
        public string  ImageUrl { get; set; }
        public DateTime creation_date { get; set; }
        public DateTime updated_date { get; set; }

    }
}
