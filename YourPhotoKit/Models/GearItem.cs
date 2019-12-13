using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YourPhotoKit.Models
{
    public class GearItem
    {
        [Key]
        public int GearItemId { get; set; }

        [Required]
        [StringLength(55, ErrorMessage = "Please shorten the title to 55 characters or less")]
        [Display(Name = "Gear Name")]
        public string Title { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Please shorten the description to 255 characters or less")]
        [Display(Name = "Gear Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Gear Type")]
        public int GearTypeId { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Price Paid")]
        public decimal Cost { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date Purchased")]
        public DateTime DatePurchased { get; set; }

        [Display(Name = "Serial Number")]
        public string SerialNumber { get; set; }

        [Display(Name = "Photo URL")]
        public string PhotoUrl { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser User { get; set; }

        public Trip Trip { get; set; }
  
        public List<TripGear> TripGear { get; set; } = new List<TripGear>();

       public GearType gearType { get; set; }


    }
}