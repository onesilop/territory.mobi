using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using territory.mobi.Models;

namespace territory.mobi.Pages.Admin.Users.Claims
{
    public class DeleteCongModel : PageModel
    {
        private readonly territory.mobi.Models.TerritoryContext _context;

        public DeleteCongModel(territory.mobi.Models.TerritoryContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AspNetUserClaims AspNetUserClaims { get; set; }

        public async Task<IActionResult> OnGetAsync(string id, int claimid, string returl = "")
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return this.Redirect("/Admin/Index");
            }
            else
            {
                AspNetUserClaims = await _context.AspNetUserClaims
                .Include(a => a.User).FirstOrDefaultAsync(m => m.Id == claimid);

                ViewData["name"] = _context.AspNetUsers.Where(u => u.Id == id).FirstOrDefault().Name + " " + _context.AspNetUsers.Where(u => u.Id == id).FirstOrDefault().Surname;
                if (AspNetUserClaims == null)
                {
                    return NotFound();
                }
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync(string id, int claimid, string returl = "")
        {

            AspNetUserClaims = await _context.AspNetUserClaims.FindAsync(claimid);

            if (AspNetUserClaims != null)
            {
                _context.AspNetUserClaims.Remove(AspNetUserClaims);
                await _context.SaveChangesAsync();
            }

            IDictionary<string, string> args = new Dictionary<string, string>();
            args.Add("id", id);
            if (returl == "")
            {
                returl = "/Admin/Users/Edit";
            } else
            {
                returl = "/Admin/" + returl + "/Edit";
            }
            return RedirectToPage(returl, args);
        }
    }
}
