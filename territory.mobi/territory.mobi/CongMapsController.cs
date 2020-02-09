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
    public class CongMapsController : ControllerBase
    {
        private readonly TerritoryContext _context;
        public CongMapsController(TerritoryContext context)
        {
            _context = context;
        }

        [HttpGet("{CongID}")]
        public async Task<ActionResult<IEnumerable<Guid>>> GetCongMaps(Guid CongID)
        {
            DateTime LastCheckedDate = new DateTime(1970, 1, 1);
            return await CongMapsSince(CongID, LastCheckedDate);
        }

        [HttpGet("{CongID}/{LastCheckedDate}")]
        public async Task<ActionResult<IEnumerable<Guid>>> GetCongMapsSince(Guid CongID, Int64 LastCheckedDate)
        {
            return await CongMapsSince(CongID, new DateTime(LastCheckedDate));
        }

        private async Task<ActionResult<IEnumerable<Guid>>> CongMapsSince(Guid CongID, DateTime LastCheckedDate)
        {
            IList<Guid> ListOmm = new List<Guid>();
            IList <Map> mps = await _context.Map.Where(m => m.CongId == CongID && m.UpdateDatetime >= LastCheckedDate).ToListAsync();
            for (int i = 0; i < mps.Count(); i++)
            {
                ListOmm.Add(mps[i].MapId);
            }
            return ListOmm.ToList();
        }

    }
}
 