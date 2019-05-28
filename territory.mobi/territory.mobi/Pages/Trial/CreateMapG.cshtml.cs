using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using territory.mobi.Models;
using static System.Net.WebRequestMethods;
using System.Security.Cryptography;
using System.Xml;
using Newtonsoft.Json;

namespace territory.mobi.Pages
{


    public class CreateMapgModel : PageModel
    {
        private readonly territory.mobi.Models.TerritoryContext _context;

        public CreateMapgModel(territory.mobi.Models.TerritoryContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Map Map { get; set; }
        public Cong Cong { get; set; }
        public Images Image { get; set; }
        public IList<DoNotCall> DNC { get; set; }
        public string ParkingHT { get; set; }
        public string NotesHT { get; set; }
        public string DncHT { get; set; }
        public string GoogleKey { get; set; }
        public bool ShowMap { get; set; } = false;
        public IList<Setting> Setting { get; set; }



        
        public async Task<IActionResult> OnGetAsync()
        {
            Setting set  = await _context.Setting.Where(s => s.SettingType == "GoogleAPIKey").FirstOrDefaultAsync();
            GoogleKey = set.SettingValue;

            return Page();
        }

        public IActionResult OnPost()
        {
            return new BadRequestResult();
        }

        private bool MapExists(Guid id)
        {
            return _context.Map.Any(e => e.MapId == id);
        }
    }
}
