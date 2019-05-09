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

namespace territory.mobi.Pages.Admin.Congregation
{
    [Authorize(Roles = "Admin,TerritoryServant,ServiceOverseer")]
    public class EditModel : PageModel
    {
        private readonly territory.mobi.Models.TerritoryContext _context;

        public EditModel(territory.mobi.Models.TerritoryContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Cong Cong { get; set; }
        public AspNetUsers AspNetUsers { get; set; }
        public IList<AspNetUserRoles> Roles { get; set; }
        public IList<AspNetUserClaims> Claims { get; set; }
        public IList<CongUser> CongUsers { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return this.Redirect("/Admin/Index");
            }
            else
            {
                if (id == null)
                {
                    return NotFound();
                }

                Cong = await _context.Cong.FirstOrDefaultAsync(m => m.CongId == id);

                if (Cong == null)
                {
                    return NotFound();
                }

                Claims = await _context.AspNetUserClaims.Where(c => c.ClaimValue == Cong.CongName).ToListAsync();
                CongUsers = new List<CongUser>();
                foreach (AspNetUserClaims c in Claims)
                {
                    CongUser cnu = new CongUser();
                    cnu.User = c.User;
                    cnu.Claims = c;
                    foreach (AspNetUserRoles r in _context.AspNetUserRoles.ToList().Where(u => u.User.Id == c.User.Id).ToList())
                    {
                        cnu.Role = r.Role;
                        CongUsers.Add(cnu);
                    }
                    
                }
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Cong).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CongExists(Cong.CongId))
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

        private bool CongExists(Guid id)
        {
            return _context.Cong.Any(e => e.CongId == id);
        }
    }
}
