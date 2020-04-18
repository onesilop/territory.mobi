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
    public class CongController : ControllerBase
    {
        private readonly TerritoryContext _context;
        public CongController(TerritoryContext context)
        {
            _context = context;
        }


        // GET: api/Cong
        [HttpGet(Name = "GetAllCongs")]
        public async Task<IEnumerable<Cong>> GetAsync()
        {
            return await _context.Cong.ToListAsync().ConfigureAwait(false);
        }

        // GET: api/Cong/5
        [HttpGet("{id}", Name = "GetCongById")]
        public async Task<string> GetAsync([FromQuery] Guid id)
        {
            return await _context.Cong.FirstOrDefaultAsync(c => c.CongId == id).ConfigureAwait(false);
        }

        // GET: api/Cong/5/Maps
        [HttpGet("{CongID}/Maps", Name = "GetMapsByCongId")]
        public async Task<ActionResult<IEnumerable<MapMin>>> GetCongMaps([FromQuery] Guid CongID)
        {
            DateTime LastCheckedDate = new DateTime(1970, 1, 1);
            return await CongMapsSince(CongID, LastCheckedDate).ConfigureAwait(false);
        }
        // GET: api/Cong/5/MapSince/123456
        [HttpGet("{CongID}/MapSince/{LastCheckedDate}", Name = "GetMapsByCongByLastChanged")]
        public async Task<ActionResult<IEnumerable<MapMin>>> GetCongMapsSince([FromQuery] Guid CongID, [FromQuery] Int64 LastCheckedDate)
        {
            return await CongMapsSince(CongID, new DateTime(LastCheckedDate)).ConfigureAwait(false);
        }

        private async Task<ActionResult<IEnumerable<MapMin>>> CongMapsSince(Guid CongID, DateTime LastCheckedDate)
        {
            IList<MapMin> ListOmm = new List<MapMin>();
            IList<Map> mps = await _context.Map.Where(m => m.CongId == CongID && m.UpdateDatetime >= LastCheckedDate).ToListAsync().ConfigureAwait(false);
            foreach (Map m in mps)
            {
                ListOmm.Add(new MapMin(m, _context));
            }
            return ListOmm.ToList();
        }
        // GET: api/Cong/5/Directory
        [HttpGet("{CongID}/Directory/", Name = "GetCongDirectory")]
        public CongDirectory GetCongSections([FromQuery] Guid CongID)
        {
            CongDirectory cd = new CongDirectory(CongID, _context);
            return cd;
        }

    }
}
