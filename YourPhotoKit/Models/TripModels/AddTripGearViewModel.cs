using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
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
        public virtual ICollection<TripGear> PickedItems { get; set; }

    }
}
