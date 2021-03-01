using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmyzFactory.Models
{
    public class TextsDomainModel
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

     //   public ResultDomainModel Result { get; set; }

       //   public HttpPostedFileWrapper ImageFile { get; set; }

        // For Delete
        public int Id { get; set; }



    }
}