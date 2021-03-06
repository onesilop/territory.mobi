﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        [HttpGet(Name = "GetAllMaps")]
        public async Task<ActionResult<IEnumerable<MapMin>>> GetMap()
        {
            IList<MapMin> MapMins = new List<MapMin>();
            IList<Map> mps = await _context.Map.ToListAsync().ConfigureAwait(false);
            foreach (Map m in mps)
            {
                MapMins.Add(new MapMin(m, _context));
            }
            return MapMins.ToList();
        }

        // GET: api/Maps/5
        [HttpGet("{id}", Name = "GetMapByID")]
        public async Task<ActionResult<MapMin>> GetMap([FromRoute] Guid id)
        {
            Map m = await _context.Map.FindAsync(id);

            if (m == null)
            {
                return NotFound();
            }

            MapMin mm = new MapMin(m, _context);

            return mm;
        }



        private bool MapExists([FromRoute] Guid id)
        {
            return _context.Map.Any(e => e.MapId == id);
        }

        private string GetCoords(string address)
        {
            //var url = "https://nominatim.openstreetmap.org/search?q=" + address.Replace(" ", "+") + "&format=json";
            //var client = new WebClient();

            //var response = client.DownloadString(url);
            return null;

        }
    }


}
