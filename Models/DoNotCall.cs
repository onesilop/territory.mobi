using System;
using System.ComponentModel.DataAnnotations;

namespace territory.mobi.Models
{
    public partial class DoNotCall
    {
        public Guid DncId { get; set; }
        public Guid MapId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateValidated { get; set; }
        [Required]
        [Display(Name = "Street Number")]
        public string StreetNo { get; set; }

        [Required]
        [Display(Name = "Street Name")]
        public string StreetName { get; set; }

        [Display(Name = "Apt/Unit Number")]
        public string AptNo { get; set; }
        [Required]
        public string Suburb { get; set; }
        public string Note { get; set; }
        [Required]
        [Display(Name = "Show on map")]
        public Boolean Display { get; set; }

        public string Geocode { get; set; }

        public DateTime UpdateDatetime { get; set; }

        public virtual Map Map { get; set; }
    }
}
