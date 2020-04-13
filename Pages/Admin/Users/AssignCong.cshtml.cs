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

        public IActionResult OnGet(string id, string assignType, int claimid = -1)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return this.Redirect("/Admin/Index");
            }
            else
            {
                if (assignType == null)
                {
                    assignType = "Congregation";
                }
                IList<AspNetUserClaims> clm = _context.AspNetUserClaims.Where(u => u.UserId == id && u.ClaimType == assignType).ToList();
                IList<Cong> cng = _context.Cong.ToList();
                foreach (AspNetUserClaims c in clm)
                {
                   cng.Remove(cng.Where(a => a.CongName == c.ClaimValue).FirstOrDefault());
                }
                ViewData["Congs"] = new SelectList(cng, "CongName", "CongName");
                ViewData["name"] = _context.AspNetUsers.Where(u => u.Id == id).FirstOrDefault().FullName;
                if (claimid != -1)
                { 
                    ViewData["cong"] = _context.AspNetUserClaims.Where(u => u.Id == claimid).FirstOrDefault().ClaimValue;
                }
                return Page();
            }
        }

        [BindProperty]
        public AspNetUserClaims AspNetUserClaims { get; set; }

        public async Task<IActionResult> OnPostAsync(string id, string assignType, int claimid = -1, string returl = "", string cong = "")
        {
            AspNetUserClaims.UserId = id;
            if (claimid != -1)
            {
                AspNetUserClaims clm = _context.AspNetUserClaims.Where(a => a.Id == claimid).FirstOrDefault();
                AspNetUserClaims.UserId = clm.UserId;
                AspNetUserClaims.ClaimValue = clm.ClaimValue;
                _context.AspNetUserClaims.Remove(clm);
            }
            if (assignType == null)
            {
                assignType = "Congregation";
            }
            AspNetUserClaims.ClaimType = assignType;
            ModelState.Clear();
            TryValidateModel(AspNetUserClaims);
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.AspNetUserClaims.Add(AspNetUserClaims);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            if (cong.Length == 0) { id = cong; }

            returl = returl.Length == 0 ? "/Admin/Users/Edit" : "/Admin/" + returl + "/Edit";
            IDictionary<string, string> args = new Dictionary<string, string>
            {
                { "id", id }
            };
            return RedirectToPage(returl, args);
        }
    }
}