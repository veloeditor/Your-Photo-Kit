using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YourPhotoKit.Models
{
    public class TripPhoto
    {
        [Key]
        public int TripPhotoId {get; set;}

        [Display(Name = "Trip Gallery Link")]
        public string PhotoUrl { get; set; }

        public int TripId { get; set; }

        public TripPhoto(string photoUrl, int tripId)
        {
            PhotoUrl = photoUrl;
            TripId = tripId;
        }
    }
}
