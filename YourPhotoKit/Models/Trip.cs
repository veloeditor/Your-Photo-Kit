using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YourPhotoKit.Models
{
    public class Trip
    {
        [Key]
        public int TripId { get; set; }

        [Required]
        [StringLength(55, ErrorMessage = "Please shorten the title to 55 characters or less")]
        [Display(Name = "Trip Name")]
        public string Title { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Please shorten the description to 255 characters or less")]
        [Display(Name = "Trip Description")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Zip Code")]
        public string Location { get; set; }

        [Display(Name = "Photo URL")]
        public string PhotoUrl { get; set; }

        [Display(Name = "Comments")]
        public string UserComments { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser User { get; set; }
        public virtual ICollection<GearItem> GetCollection { get; set; }

    }
}