﻿using System;
using System.Collections.Generic;

namespace territory.mobi.Models
{
    public partial class Images
    {
        public Guid ImgId { get; set; }
        public string ImgText { get; set; }
        public string ImgPath { get; set; }
        public byte[] ImgImage { get; set; }
        public Guid? MapId { get; set; }
        public DateTime? Updatedatetime { get; set; }

        public virtual Map Map { get; set; }
    }
}
