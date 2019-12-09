using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YourPhotoKit.Models.TripModels
{
    public class CreateandEditTripViewModel
    {
        public Trip Trip { get; set; }
        public List<GearItem> GearItems { get; set; }
        public List<SelectListItem> TripGearOptions
        {
            get
            {
                return GearItems?.Select(g => new SelectListItem(g.Title, g.GearItemId.ToString())).ToList();
            }
        }
    }
}
