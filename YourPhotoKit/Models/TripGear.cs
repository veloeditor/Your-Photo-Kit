using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YourPhotoKit.Models
{
    public class TripGear
    {
        [Key]
        public int TripGearId { get; set; }

        [Required]
        public int GearId { get; set; }
       
        [Required]
        public int TripId { get; set; }

        public Trip Trip { get; set; }
        public string PhotoUrl { get; set; }

        public GearItem GearItem { get; set; }
    }
}
