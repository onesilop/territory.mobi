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
            Cong = new List<Cong>();
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
             
                    IEnumerable<Claim> claims = User.FindAll("Congregation");
                    if (claims.Count() == 0)
                    {
                        Response.Redirect("/Admin/Users/Default");
                    }
                    else {

                        foreach (Claim c in claims)
                        {

                            Cong.Add(await _context.Cong.Where(a => a.CongName == c.Value).FirstOrDefaultAsync());

                        }
                    }
                }
                string pId = "Congregation/List";
                ViewData["PageName"] = "Congregations";
                ViewData["PageHelpID"] = pId;
                if (_context.PageHelp.Count(p => p.PageId == pId) > 0)
                {
                    IList<PageHelpText> phl = await _context.PageHelp.Where(p => p.PageId == pId).ToListAsync();
                    foreach (PageHelpText ph in phl)
                    { 
                    if (ph.HtmlHelp != null)
                        {
                            ViewData[string.Concat("PageHelp",(ph.SectionId ?? ""))] = ph.HtmlHelp;
                        }
                    }
                }
            }
        }
    }
}
