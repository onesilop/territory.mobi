using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace territory.mobi.Models
{
    public partial class Cong
    {
        public Guid CongId { get; set; }
        [Display(Name = "Congregation")]
        public string CongName { get; set; }
        [Display(Name = "Territory Servent")]
        public string ServId { get; set; }
        public DateTime UpdateDatetime { get; set; }

        public bool Hide { get; set; }

        public static implicit operator string(Cong v)
        {
            throw new NotImplementedException();
        }
    }
}
