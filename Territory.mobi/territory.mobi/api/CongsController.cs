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
    public class CongsController : ControllerBase
    {
        private readonly TerritoryContext _context;
        public CongsController(TerritoryContext context)
        {
            _context = context;
        }


        // GET: api/Congs
        [HttpGet]
        public async Task<IEnumerable<Cong>> GetAsync()
        {
            return await _context.Cong.ToListAsync();
        }

        // GET: api/Congs/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<string> GetAsync(Guid id)
        {
            return await _context.Cong.FirstOrDefaultAsync(c => c.CongId == id);
        }

    }
}
