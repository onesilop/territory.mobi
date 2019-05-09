using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using territory.mobi.Models;

namespace territory.mobi.Pages.Admin.Congregation
{
    [Authorize(Roles = "Admin,TerritoryServant,ServiceOverseer")]
    public class IndexModel : PageModel
    {

        private readonly territory.mobi.Models.TerritoryContext _context;
        
        public IndexModel(territory.mobi.Models.TerritoryContext context)
        {
            _context = context;
        }

        public IList<Cong> Cong { get; set; }
        public IList<AspNetUserClaims> AspNetUserClaims { get; set; }

        public async Task OnGetAsync()
        {
            if (User.Identity.IsAuthenticated == false)
            {
                Response.Redirect("/Admin/Index");
            }
            else
            {
                if (User.IsInRole("Admin"))
                {
                    Cong = await _context.Cong.ToListAsync();
                }
                else
                { 
                    int cnt = 0;               
                    IEnumerable<Claim> claims = User.FindAll("Congregation");
                    foreach (Claim c in claims)
                    {
                        if (cnt == 0)
                        {
                            Cong = await _context.Cong.Where(a => a.CongName == c.Value).ToListAsync();
                        }
                        else
                        { 
                            Cong.Add(await _context.Cong.Where(a => a.CongName == c.Value).FirstOrDefaultAsync());
                        }
                        cnt++;
                    }
                }
            }
        }
    }
}
