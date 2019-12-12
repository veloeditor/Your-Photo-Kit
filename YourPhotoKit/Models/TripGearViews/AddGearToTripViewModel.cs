using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YourPhotoKit.Models.TripGearViews
{
    public class AddGearToTripViewModel
    {
        public Trip Trip { get; set; }
        public int TripId { get; set; }
        public int GearItemId { get; set; }
        public GearItem GearItem { get; set; }
        public TripGear TripGear { get; set; }
        public virtual ICollection<GearItem> GearItems { get; set; }
        public virtual ICollection<TripGear> PickedItems { get; set; }


    }
}
