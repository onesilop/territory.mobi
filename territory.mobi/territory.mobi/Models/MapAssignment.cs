using System;
using System.Collections.Generic;

namespace territory.mobi.Models
{
    public partial class MapAssignment
    {
        public Guid AssignId { get; set; }
        public Guid MapId { get; set; }
        public string UserId { get; set; }
        public string NonUserName { get; set; }
        public DateTime DateAssigned { get; set; }
        public DateTime? DateReturned { get; set; }
        public DateTime Updatedatetime { get; set; }
    }
}
