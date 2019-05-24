using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace territory.mobi.Models
{
    public partial class Map
    {

        
        public Map()
        {
            DoNotCall = new HashSet<DoNotCall>();
            Images = new HashSet<Images>();
        }

        public Guid MapId { get; set; }
        public Guid CongId { get; set; }
        [Display(Name = "Map Key")]
        [Required]
        public string MapKey { get; set; }
        [Display(Name = "Description")]
        [Required]
        public string MapDesc { get; set; }
        [Display(Name = "Area")]
        [Required]
        public string MapArea { get; set; }
        public string MapPolygon { get; set; }
        public string MapType { get; set; }
        [Display(Name = "Image")]
        public Guid? ImgId { get; set; }
        [Display(Name = "Link to Navigation")]
        public string GoogleRef { get; set; }
        public string Notes { get; set; }
        public string Parking { get; set; }
        [Display(Name = "Section to display Map Under")]
        public Guid? SectionId { get; set; }
        [Display(Name = "Order of Apperance")]
        public int SortOrder { get; set; }
        [Display(Name = "Show this map?")]
        public Boolean Display { get; set; }
        public DateTime UpdateDatetime { get; set; }

        public virtual ICollection<DoNotCall> DoNotCall { get; set; }
        public virtual ICollection<Images> Images { get; set; }

        public IList<String> MapTypeVal
            {

                get
                {
                    IList<String> lst = new List<string>
                    {
                        "Business",
                        "Residential",
                        "Carts",
                        "Other"
                    };
                    return lst;
                }
            }
    }
}
