using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
        public IList<Map> Maps { get; set; }
        public IList<Models.Section> Section { get; set; }
        public IList<Setting> Setting { get; set; }
        public IList<AspNetUserClaims> UnapprovedUsers { get; set; }
        public string CongNames { get; set; }
        public IList<Dncpword> Password { get; set; }
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

                Cong = await _context.Cong.FirstOrDefaultAsync(m => m.CongId == id).ConfigureAwait(false);

                if (Cong == null)
                {
                    return NotFound();
                }
                List<string> ListOCongNames = new List<string>();
                foreach (Cong c in _context.Cong.ToList())
                {
                    if (c != Cong)
                    {
                        ListOCongNames.Add(c.CongName);
                    }
                }
                CongNames = Newtonsoft.Json.JsonConvert.SerializeObject(ListOCongNames);
                Claims = await _context.AspNetUserClaims
                    .Include(a => a.User)
                    .Where(c => c.ClaimValue == Cong.CongName && c.ClaimType == "Congregation").ToListAsync().ConfigureAwait(false);

                UnapprovedUsers = await _context.AspNetUserClaims
                   .Include(a => a.User)
                   .Where(c => c.ClaimValue == Cong.CongName && c.ClaimType == "TempCong").ToListAsync().ConfigureAwait(false);

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

                Maps = await _context.Map.OrderBy(m => m.SortOrder).Where(m => m.CongId == Cong.CongId && m.SectionId == null).ToListAsync().ConfigureAwait(false);
                Section = await _context.Section.Where(s => s.CongId == Cong.CongId).ToListAsync().ConfigureAwait(false);
                foreach (Models.Section s in Section)
                {
                    s.Maps = await _context.Map.Where(m => m.CongId == Cong.CongId && m.SectionId == s.SectionId).ToListAsync().ConfigureAwait(false);
                }

                string pId = "Congregation/Edit";
                ViewData["PageName"] = "Congregation";
                ViewData["PageHelpID"] = pId;
                if (_context.PageHelp.Count(p => p.PageId == pId) > 0)
                {
                    IList<PageHelpText> phl = await _context.PageHelp.Where(p => p.PageId == pId).ToListAsync().ConfigureAwait(false);
                    foreach (PageHelpText ph in phl)
                    {
                        if (ph.HtmlHelp != null)
                        {
                            ViewData[string.Concat("PageHelp", (ph.SectionId ?? ""))] = ph.HtmlHelp;
                        }
                    }
                }
                Password = await _context.Dncpword.Where(p => p.CongId == Cong.CongId && p.Notinuse == 0).ToListAsync().ConfigureAwait(false);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostInviteUser(string email, Guid congid)
        {
            Token userInvite = new Token
            {
                TokenId = Guid.NewGuid(),
                UserCong = congid.ToString(),
                UserEmail = email,
                UpdateDateTime = DateTime.UtcNow
            };
            try
            {


                _context.Token.Add(userInvite);

                await _context.SaveChangesAsync().ConfigureAwait(false);
                Cong = await _context.Cong.FirstOrDefaultAsync(m => m.CongId == congid).ConfigureAwait(false);

                Mailer mailer = new Mailer(_context);
                return await mailer.SendUserInviteMail(email, userInvite.TokenId.ToString(), Cong.CongName.ToString()).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return new BadRequestResult();
            }
        }

        public async Task<IActionResult> OnPostSetPassword(string password, Guid congid, bool stopOthers = false)
        {
            Guid newid = Guid.NewGuid();
            Dncpword pword = new Dncpword();
            pword.PwdId = newid;
            pword.PasswordHash = password;
            pword.CongId = congid;
            pword.Notinuse = 0;
            try
            {
                Password = await _context.Dncpword.Where(p => p.CongId == Cong.CongId).ToListAsync().ConfigureAwait(false);
                _context.Dncpword.Add(pword);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                if (stopOthers)
                {
                    foreach (Dncpword pwd in Password)
                    {
                        pwd.Notinuse = 1;
                        _context.Dncpword.Attach(pwd);
                    }
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                }
                return new StatusCodeResult(200);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return new BadRequestResult();
            }
        }


        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (!ModelState.IsValid)
            {
                return await OnGetAsync(id).ConfigureAwait(false);
            }
            Cong.UpdateDatetime = DateTime.UtcNow;
            _context.Attach(Cong).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync().ConfigureAwait(false);
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
