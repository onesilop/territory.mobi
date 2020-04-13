using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using territory.mobi.Models;

namespace territory.mobi.Pages.Admin.Congregation.Maps
{
    public class EditModel : PageModel
    {
        private readonly territory.mobi.Models.TerritoryContext _context;

        public EditModel(territory.mobi.Models.TerritoryContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Map Map { get; set; }
        public Images CurrentImage { get; set; }
        public string MapKeys { get; set; }
        public string GoogleKey { get; set; }
        public IList<DoNotCall> DoNotCall { get; set; }
        public IList<MapFeature> MapPolygons { get; set; }
        public IList<MapFeature> MapMarkers { get; set; }
        public string MapCentreLat { get; set; } = "";
        public string MapCentreLng { get; set; } = "";
        public int MapZoom { get; set; } = 16;
        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            Setting set = await _context.Setting.Where(s => s.SettingType == "GoogleAPIKey").FirstOrDefaultAsync();
            GoogleKey = set.SettingValue;

            if (id == null)
            {
                return NotFound();
            }

            Map = await _context.Map
                .Include(d => d.DoNotCall)
                .FirstOrDefaultAsync(m => m.MapId == id);

            if (Map == null)
            {
                return NotFound();
            }

            MapPolygons = await _context.MapFeature.Where(m => m.MapId == id && m.Type == "Polygon").ToListAsync();
            MapMarkers = await _context.MapFeature.Where(m => m.MapId == id && m.Type == "Marker").ToListAsync();
            MapFeature MapCentre = await _context.MapFeature.Where(m => m.MapId == id && m.Type == "Centre").FirstOrDefaultAsync();

            if (MapCentre != null)
            {
                dynamic Coords = Newtonsoft.Json.JsonConvert.DeserializeObject(MapCentre.Position);
                MapCentreLat = Coords.lat;
                MapCentreLng = Coords.lng;
                MapZoom = MapCentre.Zoom;
            }

            CurrentImage = await _context.Images.FirstOrDefaultAsync(m => m.ImgId == Map.ImgId);
            if (CurrentImage == null)
            {
                CurrentImage = new Images();
                ViewData["catme"] = CurrentImage.Cat;
            }

            List<string> ListOMapKeys = new List<string>();
            foreach (Map c in _context.Map.Where(c => c.CongId == Map.CongId).ToList())
            {
                if (c != Map)
                {
                    ListOMapKeys.Add(c.MapKey);
                }
            }
            MapKeys = Newtonsoft.Json.JsonConvert.SerializeObject(ListOMapKeys);

            DoNotCall = _context.DoNotCall.Where(d => d.MapId == Map.MapId).ToList();
            IList<Models.Section> slist = await _context.Section.Where(s => s.CongId == Map.CongId).ToListAsync();
            ViewData["Section"] = new SelectList(slist, "SectionId", "SectionTitle", Map.SectionId);

            string pId = "Map/Edit";
            ViewData["PageName"] = "Map";
            ViewData["PageHelpID"] = pId;
            if (_context.PageHelp.Count(p => p.PageId == pId) > 0)
            {
                IList<PageHelpText> phl = await _context.PageHelp.Where(p => p.PageId == pId).ToListAsync();
                foreach (PageHelpText ph in phl)
                {
                    if (ph.HtmlHelp != null)
                    {
                        ViewData[string.Concat("PageHelp", (ph.SectionId ?? ""))] = ph.HtmlHelp;
                    }
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Map.Notes != null)
            { 
                Map.Notes = Map.Notes.ToString().Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
            }
            if (Map.Parking != null)
            { 
                Map.Parking = Map.Parking.ToString().Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
            }
            Map.UpdateDatetime = DateTime.UtcNow;
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
            IDictionary<string, string> args = new Dictionary<string, string>
            {
                { "id", Map.CongId.ToString()}
            };
            return RedirectToPage("/Admin/Congregation/Edit", args);


        }

        public async Task<IActionResult> OnPostUploadImage(IList<IFormFile> files, Guid id)
        {

            if (files.Count != 0)
            { 
                Map = await _context.Map.FirstOrDefaultAsync(m => m.MapId == id);
                IFormFile uploadedImage = files.FirstOrDefault();
                if (uploadedImage == null || uploadedImage.ContentType.ToLower().StartsWith("image/"))
                {
                    MemoryStream ms = new MemoryStream();
                    uploadedImage.OpenReadStream().CopyTo(ms);
                    Guid NewImageId = Guid.NewGuid();

                    Images NewImage = new Images()
                    {
                        ImgId = NewImageId,
                        ImgImage = ms.ToArray(),
                        ImgText = Map.MapDesc,
                        MapId = Map.MapId,
                        Updatedatetime = DateTime.UtcNow
                    };
                    _context.Images.Add(NewImage);
                    Map.ImgId = NewImageId;
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
                }
            }
            return await OnGetAsync(id);
        }

        private bool MapExists(Guid id)
        {
            return _context.Map.Any(e => e.MapId == id);
        }
    }
}
