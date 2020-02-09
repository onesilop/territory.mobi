using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using territory.mobi.Models;

namespace territory.mobi
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapsController : ControllerBase
    {
        private readonly TerritoryContext _context;

        public MapsController(TerritoryContext context)
        {
            _context = context;
        }

        // GET: api/Maps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Map>>> GetMap()
        {
            return await _context.Map.ToListAsync();
        }

        // GET: api/Maps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MapMin>> GetMap(Guid id)
        {
            Map m = await _context.Map.FindAsync(id);

            if (m == null)
            {
                return NotFound();
            }

            Images im = await _context.Images.Where(d => d.ImgId == m.ImgId).FirstOrDefaultAsync();
            Section s = await _context.Section.Where(sc => sc.SectionId == m.SectionId).FirstOrDefaultAsync();
            MapMin mm = new MapMin
            {
                DoNotCalls = m.DoNotCall,
                ImgImage = im.ImgImage,
                MapArea = m.MapArea,
                MapDesc = m.MapDesc,
                MapKey = m.MapKey,
                Section = s.SectionId.ToString(),
                SortOrder = m.SortOrder,
                Display = m.Display,
                Notes = m.Notes,
                Parking = m.Parking
            };

            return mm;
        }

  

        private bool MapExists(Guid id)
        {
            return _context.Map.Any(e => e.MapId == id);
        }

        public string GetCoords(string address)
        {
            var url = "https://nominatim.openstreetmap.org/search?q=" + address.Replace(" ", "+") + "&format=json";
            var client = new WebClient();

            var response = client.DownloadString(url);
            return response;

        }
    }

   
}
