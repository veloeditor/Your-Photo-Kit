using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YourPhotoKit.Models.TripModels
{
    public class AddTripGearViewModel
    {
        public Trip Trip { get; set; }

        public TripGear GearForTrip { get; set; }

        public GearItem GearYouOwn { get; set; }

        public virtual ICollection<GearItem> GearItems { get; set; }

        [Display(Name = "Packed Gear")]
        public virtual ICollection<TripGear> PickedItems { get; set; }
        public GearType GearType { get; set; }
        public IFormFile Img { get; set; }
    }
}
