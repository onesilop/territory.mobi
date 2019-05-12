using System;
using System.Collections.Generic;

namespace territory.mobi.Models
{
    public partial class DoNotCall
    {
        public Guid DncId { get; set; }
        public Guid MapId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateValidated { get; set; }
        public string StreetNo { get; set; }
        public string StreetName { get; set; }
        public string AptNo { get; set; }
        public string Suburb { get; set; }
        public string Note { get; set; }
        public int Display { get; set; }
        public DateTime UpdateDatetime { get; set; }

        public virtual Map Map { get; set; }
    }
}
