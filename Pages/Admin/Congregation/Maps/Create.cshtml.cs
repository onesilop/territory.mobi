using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using territory.mobi.Models;

namespace territory.mobi.Pages.Admin.Congregation.Maps
{
    public class CreateModel : PageModel
    {
        private readonly territory.mobi.Models.TerritoryContext _context;

        public CreateModel(territory.mobi.Models.TerritoryContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Map Map { get; set; }
        public string MapKeys { get; set; }

        public IActionResult OnGet(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<string> ListOMapKeys = new List<string>();
            foreach (Map c in _context.Map.Where(c => c.CongId == id).ToList())
            {
                if (c != Map)
                {
                    ListOMapKeys.Add(c.MapKey);
                }
            }
            MapKeys = Newtonsoft.Json.JsonConvert.SerializeObject(ListOMapKeys);
            Map = new Map();

            IList<Models.Section> slist = _context.Section.Where(s => s.CongId == id).ToList();
            ViewData["Section"] = new SelectList(slist, "SectionId", "SectionTitle");
            ViewData["MapType"] = new SelectList(Map.MapTypeVal);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IList<IFormFile> files, Guid id)
        {

            Map.MapId = Guid.NewGuid();
            Map.CongId = id;
            Map.UpdateDatetime = DateTime.UtcNow;
            if (files.Count != 0)
            {

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
                }
            }

            if (!ModelState.IsValid || id == null)
            {
                return Page();
            }

            Map.CongId = id;
            _context.Map.Add(Map);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            IDictionary<string, string> args = new Dictionary<string, string>
            {
                { "id", id.ToString()}
            };
            return RedirectToPage("/Admin/Congregation/Edit", args);
        }
    }
}