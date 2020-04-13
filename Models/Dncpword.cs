using System;
using System.Collections.Generic;

namespace territory.mobi.Models
{
    public partial class Dncpword
    {
        public Guid PwdId { get; set; }
        public Guid CongId { get; set; }
        public string PasswordHash { get; set; }
        public int Notinuse { get; set; }
    }
}
