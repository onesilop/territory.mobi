﻿using Microsoft.AspNetCore.Identity;

namespace territory.mobi.Areas.Identity.Data
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string Name { get; set; }

        [PersonalData]
        public string Surname { get; set; }

        [PersonalData]
        public string FullName => Name + " " + Surname;
    }
}
