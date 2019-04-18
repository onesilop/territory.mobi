using System;
using System.Collections.Generic;

namespace territory.mobi.Models
{
    public partial class tmPage
    {
        public Guid PageId { get; set; }
        public Guid CongId { get; set; }
        public string PageName { get; set; }
        public string PageTitle { get; set; }
        public Guid? PageParent { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
