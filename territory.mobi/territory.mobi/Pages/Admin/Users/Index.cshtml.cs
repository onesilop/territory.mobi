using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using territory.mobi.Models;

namespace territory.mobi.Pages.Admin.Users
{
    

    [Authorize(Roles = "Admin")]

    [BindProperties]
    public class IndexModel : PageModel
    {
        private readonly territory.mobi.Models.TerritoryContext _context;


        public const string MessageKey = nameof(MessageKey);
        public const string ErrorKey = nameof(ErrorKey);
        public List<string> AreChecked { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public IndexModel(territory.mobi.Models.TerritoryContext context)
        {

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
            foreach (string e in AreChecked)
            {
                try
                {
                   await ml.SendMailAsync(e, Subject, Body,null);
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
    }
}

