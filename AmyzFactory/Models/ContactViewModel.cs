using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmyzFactory.Models
{
    public class ContactViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public ResultViewModel Result{ get; set; }
    }
}