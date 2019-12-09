using System;
using System.ComponentModel.DataAnnotations;

namespace YourPhotoKit.Models
{
    public class GearItem
    {
        [Key]
        public int GearItemId { get; set; }

        [Required]
        [Display(Name = "Gear Name")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Gear Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Gear Type")]
        public int GearTypeId { get; set; }

        [Display(Name = "Price Paid")]
        public decimal Price { get; set; }

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


    }
}