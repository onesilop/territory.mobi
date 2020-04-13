using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using territory.mobi.Areas.Identity.Data;
using territory.mobi.Models;

namespace territory.mobi.Areas.Identity.Pages.Account
{

    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly territory.mobi.Models.TerritoryContext _context;

        public ConfirmEmailModel(UserManager<ApplicationUser> userManager,
            TerritoryContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Admin/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            AspNetUsers usr = await _context.AspNetUsers.FirstOrDefaultAsync(u => u.Id == userId);
            Tokeniser tk = new Tokeniser(_context);

            if (await tk.CheckUserToken(usr, code) == false)
            {
                throw new InvalidOperationException($"Error confirming email for user with ID '{userId}':");
            }

            return Page();
        }
    }
}
