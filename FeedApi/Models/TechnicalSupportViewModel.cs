using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeedApi.Models
{
    public class TechnicalSupportViewModel
    {
        public  int Id { get; set; }
        public string  Name { get; set; }
        public List<TechnicalTextViewModel> TechTextsList { get; set; }
    }
}