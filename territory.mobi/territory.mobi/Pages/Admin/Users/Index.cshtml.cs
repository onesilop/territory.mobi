using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using territory.mobi.Areas.Identity.Data;
using territory.mobi.Models;

namespace territory.mobi.Pages.Admin.Users
{
    

    [Authorize(Roles = "Admin")]

    [BindProperties]
    public class IndexModel : PageModel
    {
        private readonly territory.mobi.Models.TerritoryContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public const string MessageKey = nameof(MessageKey);
        public const string ErrorKey = nameof(ErrorKey);
        public List<string> AreChecked { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public IndexModel(territory.mobi.Models.TerritoryContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
           
        }

        public IList<AspNetUsers> AspNetUsers { get;set; }

        public async Task OnGetAsync()
        {
        if (User.Identity.IsAuthenticated == false)
        {
            Response.Redirect("/Admin/Index");
        }
        else
        {
            AspNetUsers = await _context.AspNetUsers.ToListAsync();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            List<string> notsentto = new List<string>();
            Mailer ml = new Mailer(_context);
            if (Body != null)
            {
                Body = Body.ToString().Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
            }
            foreach (string e in AreChecked)
            {
                try
                {
                   await ml.SendMailAsync(e, Subject,Body , null);
                }
                catch
                {
                    notsentto.Add(e);
                }
            }
            if (notsentto.Count > 0)
                TempData[MessageKey] = TempData[ErrorKey] + "</br>But not to"+ string.Join(", ", notsentto);
            else if (notsentto.Count >= AreChecked.Count)
                TempData[ErrorKey] = "Email failed to send";
            else
                TempData[MessageKey] = "Email sent successfully";

            return Redirect(Request.Path);    
        }

        public async Task<IActionResult> OnPostResetPAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            Mailer _emailSender = new Mailer(_context);
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ResetPassword",
                pageHandler: null,
                values: new { area = "Identity", code },
                protocol: Request.Scheme);

            var msg = $"Please reset your password by<a href= '{HtmlEncoder.Default.Encode(callbackUrl)}' > clicking here</ a >.";

            return await _emailSender.SendMailAsync(email, "Reset Password", msg, null);

        }
    }
}

