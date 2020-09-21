using System;

namespace territory.mobi.Models
{
    public partial class Token
    {
        public Guid TokenId { get; set; }
        public string UserEmail { get; set; }
        public string UserCong { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
