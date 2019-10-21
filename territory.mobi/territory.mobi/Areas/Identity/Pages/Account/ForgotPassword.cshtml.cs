using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using territory.mobi.Areas.Identity.Data;
using territory.mobi.Models;

namespace territory.mobi.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly territory.mobi.Models.TerritoryContext _context;


        public ForgotPasswordModel(UserManager<ApplicationUser> userManager, TerritoryContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                // if (user == null || !(await _userManager.IsEmailConfirmedAsync(user))) update email confirmation and then we can use this. 
                if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                Mailer _emailSender = new Mailer(_context);
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: (area: "Identity", code),
                    protocol: Request.Scheme);

                var msg = $"Please reset your password by<a href= '{HtmlEncoder.Default.Encode(callbackUrl)}' > clicking here</ a >.";

                await _emailSender.SendMailAsync(Input.Email,"Reset Password",msg,null);

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
