using System;

namespace territory.mobi.Models
{
    public partial class MapFeature
    {
        public Guid MapFeatureId { get; set; }
        public Guid MapId { get; set; }
        public string Type { get; set; }
        public string Position { get; set; }
        public string Color { get; set; }
        public decimal? Opacity { get; set; }
        public int ZIndex { get; set; }
        public int Zoom { get; set; }
        public string Title { get; set; }
        public DateTime Updatedatetime { get; set; }
    }
}
