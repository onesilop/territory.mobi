using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
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
        public IList<Map> Maps { get; set;}
        public IList<Models.Section> Section { get; set; }
        public IList<Setting> Setting { get; set; }
        public IList<AspNetUserClaims> UnapprovedUsers { get; set; }
        public string CongNames { get; set; }

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
                List<string> ListOCongNames = new List<string>();
                foreach (Cong c in _context.Cong.ToList())
                {
                    if (c != Cong) { 
                        ListOCongNames.Add(c.CongName);
                    }
                }
                CongNames = Newtonsoft.Json.JsonConvert.SerializeObject(ListOCongNames);
                Claims = await _context.AspNetUserClaims
                    .Include(a => a.User)
                    .Where(c => c.ClaimValue == Cong.CongName && c.ClaimType=="Congregation").ToListAsync();

                UnapprovedUsers = await _context.AspNetUserClaims
                   .Include(a => a.User)
                   .Where(c => c.ClaimValue == Cong.CongName && c.ClaimType == "TempCong").ToListAsync();

                CongUsers = new List<CongUser>();
                IList<AspNetUsers> userListTS = new List<AspNetUsers>();
                foreach (AspNetUserClaims c in Claims)
                {
                    CongUser cnu = new CongUser
                    {
                        User = c.User,
                        Claims = c
                    };
                    userListTS.Add(c.User);
                    CongUsers.Add(cnu);
                }
                ViewData["TS"] = new SelectList(userListTS, "Id", "FullName", Cong.ServId);

                Maps = await _context.Map.OrderBy(m => m.SortOrder).Where(m => m.CongId == Cong.CongId && m.SectionId == null).ToListAsync();
                Section = await _context.Section.Where(s => s.CongId == Cong.CongId).ToListAsync();
                foreach (Models.Section s in Section)
                {
                    s.Maps = await _context.Map.Where(m => m.CongId == Cong.CongId && m.SectionId == s.SectionId).ToListAsync();
                }
                return Page();
            }
        }

        public async Task<IActionResult> OnPostInviteUser(string email, string congid)
        {
            Token userInvite = new Token
            {
                TokenId = Guid.NewGuid(),
                UserCong = congid,
                UserEmail = email,
                UpdateDateTime = DateTime.UtcNow
            };
            try
                {
                    Setting SiteAddress = _context.Setting.Where(a => a.SettingType == "SiteAddress").FirstOrDefault();
                    var RedirectURL = SiteAddress.SettingValue.ToString() + "/Identity/Account/Register?token=" + userInvite.TokenId.ToString();

                    _context.Token.Add(userInvite);
           
                
                    await _context.SaveChangesAsync();
                    Mailer mailer = new Mailer(_context);
                    
                    var subject = "territory.mobi Invitation";
                    var htmlContent = "<h5>Hey there</h5></br>Please select this link to register for territory.mobi</br><a href='" + RedirectURL + "' >Register</a>";
                    return await mailer.SendMailAsync(email, subject, htmlContent, "");
                }
            catch (Exception ex)
                {
                    Debug.WriteLine(ex.InnerException);
                    return new BadRequestResult();
                }
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Cong.UpdateDatetime = DateTime.UtcNow; 
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
