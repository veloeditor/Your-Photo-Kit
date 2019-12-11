using System.Collections.Generic;

namespace YourPhotoKit.Models.TripModels
{
    public class GearForTrip
    {
        public List<GearItem> GearItems { get; set; } = new List<GearItem>();
        public bool IsChecked { get; set; }
    }
}