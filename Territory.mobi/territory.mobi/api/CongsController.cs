﻿using System;
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
        [HttpGet]
        public async Task<IEnumerable<Cong>> GetAsync()
        {
            return await _context.Cong.ToListAsync();
        }

        // GET: api/Cong/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<string> GetAsync(Guid id)
        {
            return await _context.Cong.FirstOrDefaultAsync(c => c.CongId == id);
        }

        // GET: api/Cong/5/Maps
        [HttpGet("{CongID}/Maps")]
        public async Task<ActionResult<IEnumerable<MapMin>>> GetCongMaps(Guid CongID)
        {
            DateTime LastCheckedDate = new DateTime(1970, 1, 1);
            return await CongMapsSince(CongID, LastCheckedDate);
        }
        // GET: api/Cong/5/MapSince/123456
        [HttpGet("{CongID}/MapSince/{LastCheckedDate}")]
        public async Task<ActionResult<IEnumerable<MapMin>>> GetCongMapsSince(Guid CongID, Int64 LastCheckedDate)
        {
            return await CongMapsSince(CongID, new DateTime(LastCheckedDate));
        }

        private async Task<ActionResult<IEnumerable<MapMin>>> CongMapsSince(Guid CongID, DateTime LastCheckedDate)
        {
            IList<MapMin> ListOmm = new List<MapMin>();
            IList<Map> mps = await _context.Map.Where(m => m.CongId == CongID && m.UpdateDatetime >= LastCheckedDate).ToListAsync();
            foreach (Map m in mps)
            {
                ListOmm.Add(new MapMin(m, _context));
            }
            return ListOmm.ToList();
        }
        // GET: api/Cong/5/Directory
        [HttpGet("{CongID}/Directory/")]
        public CongDirectory GetCongSections(Guid CongID)
        {
            CongDirectory cd = new CongDirectory(CongID, _context);
            return cd;
        }

    }
}
