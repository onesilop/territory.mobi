using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using territory.mobi.Models;

namespace territory.mobi.Pages.Admin.Users.Claims
{
    public class AssignCongModel : PageModel
    {
        private readonly territory.mobi.Models.TerritoryContext _context;

        public AssignCongModel(territory.mobi.Models.TerritoryContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(string id, string assignType)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return this.Redirect("/Admin/Index");
            }
            else
            {
                IList<AspNetUserClaims> clm = _context.AspNetUserClaims.Where(u => u.UserId == id).ToList();
                IList<Cong> cng = _context.Cong.ToList();
                foreach (AspNetUserClaims c in clm)
                {
                   cng.Remove(cng.Where(a => a.CongName == c.ClaimValue).FirstOrDefault());
                }
                ViewData["Congs"] = new SelectList(cng, "CongName", "CongName");
                ViewData["name"] = _context.AspNetUsers.Where(u => u.Id == id).FirstOrDefault().Name + " " + _context.AspNetUsers.Where(u => u.Id == id).FirstOrDefault().Surname;
                return Page();
            }
        }

        [BindProperty]
        public AspNetUserClaims AspNetUserClaims { get; set; }

        public async Task<IActionResult> OnPostAsync(string id, string assignType)
        {
            AspNetUserClaims.UserId = id;
            if (assignType == null)
            {
                assignType = "Congregation";
            }
            AspNetUserClaims.ClaimType = assignType;
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.AspNetUserClaims.Add(AspNetUserClaims);
            await _context.SaveChangesAsync();

            IDictionary<string, string> args = new Dictionary<string, string>();
            args.Add("id", id);
            return RedirectToPage("/Admin/Users/Edit", args);
        }
    }
}