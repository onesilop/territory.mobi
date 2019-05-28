using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<ActionResult<Map>> GetMap(Guid id)
        {
            var map = await _context.Map.FindAsync(id);

            if (map == null)
            {
                return NotFound();
            }

            return map;
        }

        // PUT: api/Maps/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMap(Guid id, Map map)
        {
            if (id != map.MapId)
            {
                return BadRequest();
            }

            _context.Entry(map).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MapExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Maps
        [HttpPost]
        public async Task<ActionResult<Map>> PostMap(Map map)
        {
            _context.Map.Add(map);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MapExists(map.MapId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMap", new { id = map.MapId }, map);
        }

        // DELETE: api/Maps/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Map>> DeleteMap(Guid id)
        {
            var map = await _context.Map.FindAsync(id);
            if (map == null)
            {
                return NotFound();
            }

            _context.Map.Remove(map);
            await _context.SaveChangesAsync();

            return map;
        }

        private bool MapExists(Guid id)
        {
            return _context.Map.Any(e => e.MapId == id);
        }
    }
}
