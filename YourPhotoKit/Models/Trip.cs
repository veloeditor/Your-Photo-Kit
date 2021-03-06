﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;



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

        [StringLength(5, ErrorMessage = "Only use the first 5 digits of the zip code")]
        [Display(Name = "Zip Code")]
        public string Location { get; set; }

        [Display(Name = "Photo URL")]
        public string PhotoUrl { get; set; }

        [Display(Name = "Comments")]
        public string UserComments { get; set; }

        [Display(Name = "Trip Photo Gallery")]
        public string GalleryUrl { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser User { get; set; }
        public virtual ICollection<GearItem> GearItems { get; set; }
        public virtual ICollection<TripGear> TripGear { get; set; }
        //public virtual ICollection<TripGear> PickedItems { get; set; }
    }
}