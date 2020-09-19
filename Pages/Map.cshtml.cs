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
using NUnit.Framework;

namespace territory.mobi.Pages
{


    public class MapModel : PageModel
    {
        private readonly territory.mobi.Models.TerritoryContext _context;

        public MapModel(territory.mobi.Models.TerritoryContext context)
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
        public bool ShowMap { get; set; } = false;
        public string GoogleKey { get; set; }
        public IList<Setting> Setting { get; set; }
        public IList<MapFeature> MapPolygons { get; set; }
        public IList<MapFeature> MapMarkers { get; set; }
        public string MapCentreLat { get; set; } = "0";
        public string MapCentreLng { get; set; } = "0";
        public int MapZoom { get; set; } = 16;
        public string CookieID { get; set; } = "territory.mobi.dnclogin.";


        public ContentResult OnGetPwdCheck(string pwd,Guid congId, Guid mapId)
        {
            if (_context.Dncpword.Any(x => x.PasswordHash == pwd && x.Notinuse == 0 && x.CongId == congId))
            {
                string res = "No do not calls for this map";
                IList<DoNotCall> dd = _context.DoNotCall.Where(d => d.MapId == mapId && d.Display == true).ToList();
                if (dd.Count > 0) {
                    res = "";
                    foreach (DoNotCall d in dd)
                    {
                        string tmp = "";
                        if (d.AptNo != "" && d.AptNo != null) { tmp = d.AptNo + "/ "; }
                        tmp = tmp + d.StreetNo + " " + d.StreetName + "</br>";
                        res += tmp;
                    }
                }
                return Content(res);
            }
            else
                return Content("false"); 
        }

        public async Task<IActionResult> OnGetAsync(string CongName, string MapNo, Guid? id = null)
        {

            ViewData["Page"] = "Map";
            if (MapNo == null || CongName == null)
            {
                if (id != null)
                {
                    Map = _context.Map.FirstOrDefault(c => c.MapId == id);
                    MapNo = Map.MapKey;
                    Cong = await _context.Cong.FirstOrDefaultAsync(m => m.CongId == Map.CongId).ConfigureAwait(false);
                    CongName = Cong.CongName;
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                Cong = await _context.Cong.FirstOrDefaultAsync(m => m.CongName == CongName).ConfigureAwait(false);
                if (Cong == null)
                {
                    return NotFound();
                }
                Map = await _context.Map.FirstOrDefaultAsync(m => m.MapKey == MapNo && m.CongId == Cong.CongId).ConfigureAwait(false);
                if (Map == null)
                {
                    return NotFound();
                }

            }

            CookieID += Cong.CongName;
            ParkingHT = HttpUtility.HtmlEncode(Map.Parking);
            NotesHT = HttpUtility.HtmlEncode(Map.Notes);


            Image = await _context.Images.FirstOrDefaultAsync(m => m.ImgId == Map.ImgId).ConfigureAwait(false);

            DNC = await _context.DoNotCall.ToListAsync().ConfigureAwait(false);
            DNC = DNC.Where(d => d.MapId == Map.MapId && d.Display == true).ToList();

            if (await _context.MapFeature.Where(m => m.MapId == Map.MapId).CountAsync().ConfigureAwait(false) > 0) { ShowMap = true; }

            if (Map.MapPolygon != null) { ShowMap = true; }

            Setting set = await _context.Setting.Where(s => s.SettingType == "GoogleAPIKey").FirstOrDefaultAsync().ConfigureAwait(false);
            GoogleKey = set.SettingValue;

            MapPolygons = await _context.MapFeature.Where(m => m.MapId == Map.MapId && m.Type == "Polygon").ToListAsync().ConfigureAwait(false);
            MapMarkers = await _context.MapFeature.Where(m => m.MapId == Map.MapId && m.Type == "Marker").ToListAsync().ConfigureAwait(false);
            MapFeature MapCentre = await _context.MapFeature.Where(m => m.MapId == Map.MapId && m.Type == "Centre").FirstOrDefaultAsync().ConfigureAwait(false);

            if (MapCentre != null)
            {
                dynamic Coords = Newtonsoft.Json.JsonConvert.DeserializeObject(MapCentre.Position);
                MapCentreLat = Coords.lat;
                MapCentreLng = Coords.lng;
                MapZoom = MapCentre.Zoom;
            }

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
                await _context.SaveChangesAsync().ConfigureAwait(false);
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
