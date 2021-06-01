using System;

namespace territory.mobi.Models
{
    public partial class Setting
    {
        public Guid SettingId { get; set; }
        public string SettingType { get; set; }
        public string SettingValue { get; set; }
        public Guid? CongId { get; set; }
    }
}
