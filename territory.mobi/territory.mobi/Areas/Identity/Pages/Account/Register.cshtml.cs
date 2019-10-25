using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using territory.mobi.Areas.Identity.Data;
using territory.mobi.Models;

namespace territory.mobi.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly Mailer _emailSender;
        private readonly territory.mobi.Models.TerritoryContext _context;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            TerritoryContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
            _emailSender = new Mailer(_context);
        }

        [BindProperty]
        public InputModel Input { get; set; }
        [BindProperty]
        public AspNetUserClaims UserClaim { get; set; }

        public string ReturnUrl { get; set; }
        public Token Token { get; set; } = new Token();
        public string Congs { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
            [Display(Name = "Surname")]
            public string Surname { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
            [Display(Name = "Congregation Name")]
            public string NewCongName { get; set; }

        }

        public void OnGet(string returnUrl = null, Guid? token = null)
        {

            ViewData["Congs"] = new SelectList(_context.Cong, "CongName", "CongName");

            if (token != null)
            {
                Token = _context.Token.Find(token);
                if (Token.UpdateDateTime.Add(new System.TimeSpan(24, 0, 0)) >= DateTime.UtcNow)
                {
                    IList<Cong> lst = new List<Cong>();

                    lst = _context.Cong.Where(a => a.CongId.ToString() == Token.UserCong).ToList();
                    ViewData["Congs"] = new SelectList(lst, "CongName", "CongName", lst[0].CongName);
                    Input = new InputModel
                    {
                        Email = Token.UserEmail
                    };

                }
            }

            List<string> ListOCongs = new List<string>();
            foreach (Cong c in _context.Cong.ToList())
            {
                ListOCongs.Add(c.CongName);
            }
            Congs = Newtonsoft.Json.JsonConvert.SerializeObject(ListOCongs);
            
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null, Guid? token = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email, Name = Input.Name, Surname = Input.Surname };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    string type = "New";

                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    _ = await _emailSender.SendUserEmailConfirmation(user, code);

                    UserClaim.ClaimType = "TempCong";
                    UserClaim.UserId = user.Id;

                    if (token != null)
                    {
                        Token = _context.Token.Find(token);
                        if (Token.UpdateDateTime.Add(new System.TimeSpan(24, 0, 0)) >= DateTime.UtcNow)
                        {
                            UserClaim.ClaimType = "Congregation";
                            type = "NewUser";
                        }
                    }


                    if (Input.NewCongName != null && _context.Cong.Count(a => a.CongName.ToUpper() == Input.NewCongName.ToUpper()) == 0)
                    {
                        Cong NewCong = new Cong
                        {
                            CongId = Guid.NewGuid(),
                            CongName = Input.NewCongName,
                            UpdateDatetime = DateTime.UtcNow
                        };

                        _context.Cong.Add(NewCong);
                        UserClaim.ClaimType = "Congregation";
                        UserClaim.ClaimValue = Input.NewCongName;
                        type = "NewCong";
                    }

                    _context.AspNetUserClaims.Add(UserClaim);
                    await _context.SaveChangesAsync();

                    IList<AspNetUserClaims> Usrs = await _context.AspNetUserClaims.Where(c => c.ClaimValue == UserClaim.ClaimValue && c.ClaimType == "Congregation" && c.User.Id != user.Id).ToListAsync();
                    Cong cng = await _context.Cong.FirstOrDefaultAsync(c => c.CongName == UserClaim.ClaimValue);
                    switch (type)
                    {
                        case "New":
                            foreach (AspNetUserClaims u in Usrs)
                            {
                                _ = await _emailSender.SendUserApprovalRequest(u.User.Email, UserClaim.ClaimValue, user.FullName, cng.CongId.ToString());
                            }
                            break;
                        case "NewUser":
                            foreach (AspNetUserClaims u in Usrs)
                            {
                                _ = await _emailSender.SendUserAddition(u.User.Email, UserClaim.ClaimValue , user.FullName , cng.CongId.ToString() );
                            }
                            break;
                        case "NewCong":
                            _ = await _emailSender.SendNewCongregation(user.Email, UserClaim.ClaimValue, cng.CongId.ToString());
                            IList<AspNetUserRoles> Admins = await _context.AspNetUserRoles.Where(r => r.Role.Name == "Admin").ToListAsync();
                            foreach (AspNetUserRoles a in Admins)
                            {
                                _ = await _emailSender.SendUserApprovalRequest(a.User.Email, UserClaim.ClaimValue, user.FullName, cng.CongId.ToString(),true);
                            }
                            break;
                        default:
                            break;
                    }

                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            // If we got this far, something failed, redisplay form with the lists populated like get.
            ViewData["Congs"] = new SelectList(_context.Cong, "CongName", "CongName");

            List<string> ListOCongs = new List<string>();
            foreach (Cong c in _context.Cong.ToList())
            {
                ListOCongs.Add(c.CongName);
            }
            Congs = Newtonsoft.Json.JsonConvert.SerializeObject(ListOCongs);

            return Page();
        }
    }
}
