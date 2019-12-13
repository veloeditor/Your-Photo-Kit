using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace YourPhotoKit.Models
{
    public class TripGear
    {
        [Key]
        public int TripGearId { get; set; }

        [Required]
        public int GearItemId { get; set; }
       
        [Required]
        public int TripId { get; set; }

        public bool IsPacked { get; set; }

        public Trip Trip { get; set; }

        [Display(Name = "Packed Gear")]
        public GearItem GearItem { get; set; }
        
    }
}
