﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YourPhotoKit.Models.GearItems
{
    public class GearItemCreateViewModel
    {
        public GearItem GearItem { get; set; }
        public List<GearType> GearTypes { get; set; }

        [Display(Name = "Gear Image")]
        public IFormFile Img { get; set; }
        public List<SelectListItem> GearTypeOptions
        {
            get
            {
                return GearTypes?.Select(gt => new SelectListItem(gt.Label, gt.GearTypeId.ToString())).ToList();
            }
        }
    }
}
