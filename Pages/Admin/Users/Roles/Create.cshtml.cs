using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using territory.mobi.Models;
using territory.mobi.Areas.Identity.Data;

namespace territory.mobi.Pages.Admin.Users.Roles
{
    public class CreateModel : PageModel
    {
        private readonly territory.mobi.Models.TerritoryContext _context;

        public CreateModel(territory.mobi.Models.TerritoryContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(string id)
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
                else
                {
                    IList<AspNetRoles> roles = _context.AspNetRoles.ToList();
                    IList<AspNetUserRoles> usersroles = _context.AspNetUserRoles.Where(u => u.UserId == id).ToList();
                    foreach (AspNetUserRoles role in usersroles)
                    {
                        roles.Remove(roles.Where(r => r.Id == role.Role.Id).FirstOrDefault());
                    }
                    ViewData["RoleId"] = new SelectList(roles, "Id", "Name");
                    ViewData["name"] = _context.AspNetUsers.Where(u => u.Id == id).FirstOrDefault().Name + " " + _context.AspNetUsers.Where(u => u.Id == id).FirstOrDefault().Surname;
                    return Page();
                }
            }

        }

        [BindProperty]
        public AspNetUserRoles AspNetUserRoles { get; set; }

        public async Task<IActionResult> OnPostAsync(string id)
        {

            AspNetUserRoles.UserId = id;
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _context.AspNetUserRoles.Add(AspNetUserRoles);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            IDictionary<string, string> args = new Dictionary<string, string>();
            args.Add("id", id);
            return RedirectToPage("/Admin/Users/Edit", args);
        }
    }
}