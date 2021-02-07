using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeedApi.Model
{
    public class TextsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string  ImageUrl { get; set; }

    }
}