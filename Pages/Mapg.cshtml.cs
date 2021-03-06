﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using territory.mobi.Models;

namespace territory.mobi.Pages
{


    public class MapgModel : PageModel
    {
        private readonly territory.mobi.Models.TerritoryContext _context;

        public MapgModel(territory.mobi.Models.TerritoryContext context)
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
        public IList<MapFeature> MapPolygons { get; set; }
        public IList<MapFeature> MapMarkers { get; set; }
        public string MapCentreLat { get; set; } = "";
        public string MapCentreLng { get; set; } = "";
        public int MapZoom { get; set; } = 16;



        public ContentResult OnGetPwdCheck(string pwd, Guid congId, Guid mapId)
        {
            if (_context.Dncpword.Count(x => x.PasswordHash == pwd && x.Notinuse == 0 && x.CongId == congId) > 0)
            {
                string res = "";
                IList<DoNotCall> dd = _context.DoNotCall.Where(d => d.MapId == mapId && d.Display == true).ToList();
                foreach (DoNotCall d in dd)
                {
                    string tmp = "";
                    if (d.AptNo != "") { tmp = d.AptNo + "/ "; }
                    tmp = tmp + d.StreetNo + " " + d.StreetName + "</br>";
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
            ParkingHT = HttpUtility.HtmlEncode(Map.Parking);
            NotesHT = HttpUtility.HtmlEncode(Map.Notes);


            Image = await _context.Images.FirstOrDefaultAsync(m => m.ImgId == Map.ImgId).ConfigureAwait(false);

            DNC = await _context.DoNotCall.ToListAsync().ConfigureAwait(false);
            DNC = DNC.Where(d => d.MapId == Map.MapId && d.Display == true).ToList();

            if (Map.MapPolygon != "") { ShowMap = true; }

            Setting set = await _context.Setting.Where(s => s.SettingType == "GoogleAPIKey").FirstOrDefaultAsync().ConfigureAwait(false);
            GoogleKey = set.SettingValue;

            MapPolygons = await _context.MapFeature.Where(m => m.MapId == Map.MapId && m.Type == "Polygon").ToListAsync().ConfigureAwait(false);
            MapMarkers = await _context.MapFeature.Where(m => m.MapId == Map.MapId && m.Type == "Marker").ToListAsync().ConfigureAwait(false);
            MapFeature MapCentre = await _context.MapFeature.Where(m => m.MapId == Map.MapId && m.Type == "Centre").FirstOrDefaultAsync().ConfigureAwait(false);

            if (MapCentre == null)
            {
                return NotFound();
            }
            else
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
