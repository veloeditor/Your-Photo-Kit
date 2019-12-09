using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace YourPhotoKit.Models
{
    public class GearType
    {
        [Key]
        public int GearTypeId { get; set; }
        
        [Required]
        [Display(Name = "Gear Type")]
        public string Label { get; set; }

        [NotMapped]
        public int Quantity { get; set; }

        public virtual ICollection<GearItem> GearItems { get; set; }
    }
}
