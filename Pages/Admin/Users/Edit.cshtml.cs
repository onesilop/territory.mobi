using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using territory.mobi.Models;

namespace territory.mobi.Pages.Admin.Users
{
    [Authorize(Roles= "Admin")]
    public class EditModel : PageModel
    {
        private readonly territory.mobi.Models.TerritoryContext _context;

        public EditModel(territory.mobi.Models.TerritoryContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AspNetUsers AspNetUsers { get; set; }
        public IList<AspNetUserRoles> AspNetUserRoles { get; set; }
        public IList<AspNetUserClaims> AspNetUserClaims { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AspNetUsers = await _context.AspNetUsers.FirstOrDefaultAsync(m => m.Id == id);

            if (AspNetUsers == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            string tmp = "";
            foreach (Cong cng in _context.Cong)
             {
                tmp = tmp + ",{ text: " + cng.CongName + ",value: " + cng.CongName + "}";
                }
            ViewData["Congs"] = tmp;
            ViewData["Id"] = id;

            AspNetUserRoles = await _context.AspNetUserRoles
                            .Include(a => a.Role)
                            .Include(a => a.User).ToListAsync();
            AspNetUserRoles = AspNetUserRoles.Where(m => m.UserId == id).ToList();


            AspNetUserClaims = await _context.AspNetUserClaims.ToListAsync();
            AspNetUserClaims = AspNetUserClaims.Where(m => m.UserId == id).ToList();
  
            return Page();
        }

      /*  public async Task<IActionResult> OnPostAddCong(string Cong, string id)
        {
            private aspNetRoleClaim cng;
            _context.aspNetUserClaim.Add();
            return Page();
        }
        */

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(AspNetUsers).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AspNetUsersExists(AspNetUsers.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool AspNetUsersExists(string id)
        {
            return _context.AspNetUsers.Any(e => e.Id == id);
        }
    }
}
