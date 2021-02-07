using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmyzFeed.Repository.Data
{
    public class Questionnaire
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [MaxLength(100)]
        public string UserName{ get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Question { get; set; }
    }
}
