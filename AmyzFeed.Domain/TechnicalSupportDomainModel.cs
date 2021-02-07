using AmyzFeed.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmyzFactory.Models
{
    public class TechnicalSupportDomainModel
    {
        public  int Id { get; set; }
        public string  Name { get; set; }
        public List<TechnicalTextDomainModel> TechTextsList { get; set; }
    }
}