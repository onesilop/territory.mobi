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
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                ViewData["RoleId"] = new SelectList(_context.AspNetRoles, "Id", "Name");
                ViewData["name"] = _context.AspNetUsers.Where(u => u.Id == id).FirstOrDefault().Name + " " + _context.AspNetUsers.Where(u => u.Id == id).FirstOrDefault().Surname;
                return Page();
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
            await _context.SaveChangesAsync();

            return RedirectToPage("~/Admin/Users/Edit?id=" + id);
        }
    }
}