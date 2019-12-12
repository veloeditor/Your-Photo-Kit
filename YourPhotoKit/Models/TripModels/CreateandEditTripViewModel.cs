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
        public TripGear TripGear { get; set; }
        public List<GearItem> GearItems { get; set; }

      

        public List<SelectListItem> GearItemOptions
        {
            get
            {
                return GearItems?.Select(gt => new SelectListItem(gt.Title, gt.GearItemId.ToString())).ToList();
            }
        }
    }
}
