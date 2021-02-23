using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace AmyzFactory.Models
{
    public class TextsViewModel
    {
        // this for technical texts only
        // public int DepartmentID { get; set; }


            // like api

        public int Id { get; set; }

        [Required(ErrorMessage ="This field is required")]
        
        public string Title { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string SubTitle { get; set; }

        [Required(ErrorMessage = "This field is required")]

        public string Description { get; set; }

        public string ImageUrl { get; set; }


        // end 






        public string FilePath { get; set; }

        [JsonIgnore]
        public ResultViewModel Result { get; set; }
        [JsonIgnore]
        public HttpPostedFileBase ImageFile { get; set; }

        public byte[] PicData { get; set; }


    }
}