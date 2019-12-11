using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YourPhotoKit.Models.ReportViews
{
    public class AllGearReportViewModel
    {
        [Display(Name = "Gear Item")]
        public List<GearItem> GearItem { get; set; }

        [Display(Name = "Total Purchase Value")]
        public int TotalDollarValue { get; set; }
    }
}
