using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using territory.mobi.Models;

namespace territory.mobi.Pages
{
    public class IndexModel : PageModel
    {

        private readonly territory.mobi.Models.TerritoryContext _context;
        private IList<Cong> cong;

        public IndexModel(territory.mobi.Models.TerritoryContext context)
        {
            _context = context;
        }

        public IList<Cong> Cong { get => cong; set => cong = value; }

        public async Task OnGetAsync()
        {
            Console.WriteLine(_context.Database.GetDbConnection().ConnectionString);
            Cong = await _context.Cong.Where(c => c.Hide == false).ToListAsync().ConfigureAwait(false);
        }
    }
}
