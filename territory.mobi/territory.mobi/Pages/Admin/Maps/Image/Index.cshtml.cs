using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using territory.mobi.Models;

namespace territory.mobi.Pages.Admin.Congregation.Maps.Image
{
    public class IndexModel : PageModel
    {
        private readonly territory.mobi.Models.TerritoryContext _context;

        public IndexModel(territory.mobi.Models.TerritoryContext context)
        {
            _context = context;
        }

        public IList<Images> Images { get;set; }

        public async Task OnGetAsync()
        {
            Images = await _context.Images.ToListAsync();
        }
    }
}
