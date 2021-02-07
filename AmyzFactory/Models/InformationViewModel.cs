using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AmyzFactory.Models
{
    public class InformationViewModel
    {
        // this for technical texts only
        public int DepartmentID { get; set; }


        [Required(ErrorMessage ="This field is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Value { get; set; }

        [Required(ErrorMessage = "This field is required")]

        public string Content { get; set; }
        public string FilePath { get; set; }

        public ResultViewModel Result { get; set; }
        [JsonIgnore]
        public HttpPostedFileWrapper ImageFile { get; set; }

        public byte[] PicData { get; set; }

        // For Delete
        public int Id{ get; set; }



    }
}