using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using territory.mobi.Models;

namespace territory.mobi.Pages.Admin.Users
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

            if (assignType == null)
            {
                ViewData["Assign"] = "Congregation";
            }
            else
            {
                ViewData["Assign"] = assignType;
            }
            ViewData["UserID"] = id;
            ViewData["Congs"] = new SelectList(_context.Cong, "CongName", "CongName");
            return Page();
        }

        [BindProperty]
        public AspNetUserClaims AspNetUserClaims { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            AspNetUserClaims.UserId = ViewData["UserId"].ToString();
            AspNetUserClaims.ClaimType = ViewData["Assign"].ToString();
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.AspNetUserClaims.Add(AspNetUserClaims);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}