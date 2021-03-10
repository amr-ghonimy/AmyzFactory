using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmyzFeed.Repository.Data
{
    public class FeedsProgram
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string  productType{ get; set; }
        [Required]
        public double ProtienPercentage { get; set; }
        [Required]
        public double Quantity { get; set; }
        [Required]
        public int DayFrom { get; set; }
        [Required]
        public int DayTo{ get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
    }
}
