using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using territory.mobi.Models;

namespace territory.mobi.Pages.Admin.Users
{
    public class DefaultModel : PageModel
    {
        private readonly territory.mobi.Models.TerritoryContext _context;

        public DefaultModel(territory.mobi.Models.TerritoryContext context)
        {
            _context = context;
        }

        public string CatUri { get; set; } 
        public IActionResult OnGet()
        {
            Images I = new Images();
            CatUri = I.Cat;
            ViewData["Cong"] = "";
            Claim UserCong = User.FindFirst("TempCong");
            if (UserCong != null) ViewData["Cong"] = UserCong.Value;

            return Page();
        }
    }
}