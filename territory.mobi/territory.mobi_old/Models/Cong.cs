using System;
using System.Collections.Generic;

namespace territory.mobi.Models
{
    public partial class Cong
    {
        public Guid CongId { get; set; }
        public string CongName { get; set; }
        public string ServId { get; set; }
        public Guid? PageId { get; set; }
        public DateTime UpdateDatetime { get; set; }
    }
}
