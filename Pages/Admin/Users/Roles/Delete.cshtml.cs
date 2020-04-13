using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using territory.mobi.Models;

namespace territory.mobi.Pages.Admin.Users.Roles
{
    public class DeleteModel : PageModel
    {
        private readonly territory.mobi.Models.TerritoryContext _context;

        public DeleteModel(territory.mobi.Models.TerritoryContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AspNetUserRoles AspNetUserRoles { get; set; }

        public async Task<IActionResult> OnGetAsync(string roleid, string id)
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

                AspNetUserRoles = await _context.AspNetUserRoles
                    .Include(a => a.Role) 
                    .Include(a => a.User).FirstOrDefaultAsync(m => m.UserId == id && m.RoleId == roleid);

                if (AspNetUserRoles == null)
                {
                    return NotFound();
                }
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync(string roleid, string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AspNetUserRoles = await _context.AspNetUserRoles.FirstOrDefaultAsync(m => m.UserId == id && m.RoleId == roleid);

            if (AspNetUserRoles != null)
            {
                _context.AspNetUserRoles.Remove(AspNetUserRoles);
                await _context.SaveChangesAsync();
            }
            IDictionary<string, string> args = new Dictionary<string, string>();
            args.Add("id", id);
            return RedirectToPage("/Admin/Users/Edit",args);
        }
    }
}
