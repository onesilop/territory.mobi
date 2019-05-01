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

namespace territory.mobi.Pages
{


    public class mapModel : PageModel
    {
        private readonly territory.mobi.Models.TerritoryContext _context;

        public mapModel(territory.mobi.Models.TerritoryContext context)
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

        public ContentResult OnGetPwdCheck(string pwd,Guid congId, Guid mapId)
        {
            if (_context.Dncpword.Count(x => x.PasswordHash == pwd && x.Notinuse == 0 && x.CongId == congId ) > 0)
            {
                string res = "";
                IList<DoNotCall> dd = _context.DoNotCall.Where(d => d.MapId == mapId && d.Display == 1).ToList();
                foreach (DoNotCall d in dd)
                {
                    string tmp = "";
                    if (d.AptNo != "") { tmp = d.AptNo + "/ "; }
                    tmp = tmp + d.StreetNo + " " + d.StreetName+"</br>";
                    res = res + tmp;
                }
                return Content(res);
            }
            else
                return Content("false"); 
        }

        public async Task<IActionResult> OnGetAsync(string CongName, string MapNo)
        {
            if (MapNo == null)
            {
                return NotFound();
            }

            if (CongName == null)
            {
                return NotFound();
            }

            Cong = await _context.Cong.FirstOrDefaultAsync(m => m.CongName == CongName);
            if (Cong == null)
            {
                return NotFound();
            }

            Map = await _context.Map.FirstOrDefaultAsync(m => m.MapKey == MapNo && m.CongId == Cong.CongId);
            if (Map == null)
            {
                return NotFound();
            }
            ParkingHT = HttpUtility.HtmlEncode(Map.Parking);
            NotesHT = HttpUtility.HtmlEncode(Map.Notes);


            Image = await _context.Images.FirstOrDefaultAsync(m => m.ImgId == Map.ImgId);

            DNC = await _context.DoNotCall.ToListAsync();
            DNC = DNC.Where(d => d.MapId == Map.MapId && d.Display == 1).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Map).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MapExists(Map.MapId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool MapExists(Guid id)
        {
            return _context.Map.Any(e => e.MapId == id);
        }
    }
}
