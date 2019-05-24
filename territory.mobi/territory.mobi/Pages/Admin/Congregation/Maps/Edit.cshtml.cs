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
        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
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

            Setting set = await _context.Setting.Where(s => s.SettingType == "GoogleAPIKey").FirstOrDefaultAsync();
            GoogleKey = set.SettingValue;
            DoNotCall = _context.DoNotCall.Where(d => d.MapId == Map.MapId).ToList();
            IList<Models.Section> slist = await _context.Section.Where(s => s.CongId == Map.CongId).ToListAsync();
            ViewData["Section"] = new SelectList(slist, "SectionId", "SectionTitle", Map.SectionId);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
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
