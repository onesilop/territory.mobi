using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Octokit;
using ReverseMarkdown;
using territory.mobi.Areas.Identity.Data;

namespace territory.mobi.Pages.Admin
{
    [BindProperties]
    public class ContactModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public InputModel Input { get; set; }

        public ContactModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public class InputModel
        {

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
            [Display(Name = "Subject")]
            public string Subject { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
            [Display(Name = "Body")]
            public string Body { get; set; }
        }
        
        
        public void OnGet()
        {
 
        }

        public async Task<PageResult> OnPostAsync()
        {
            ApplicationUser usr = _userManager.GetUserAsync(User).Result;

            GitHubClient client = new GitHubClient(new ProductHeaderValue("www.territory.mobi"));
            Credentials basicAuth = new Credentials("onesilop", "Onesilop21"); // NOTE: not real credentials
            client.Credentials = basicAuth;
            Config config = new ReverseMarkdown.Config
            {
                UnknownTags = Config.UnknownTagsOption.PassThrough, // Include the unknown tag completely in the result (default as well)
                GithubFlavored = true, // generate GitHub flavoured markdown, supported for BR, PRE and table tags
                RemoveComments = true, // will ignore all comments
                SmartHrefHandling = true // remove markdown output for links where appropriate
            };

            NewIssue createIssue = new NewIssue(Input.Subject);
            Converter converter = new Converter(config);
            createIssue.Body = converter.Convert(string.Concat(Input.Body, "<br/>", usr.FullName, "<br/>", usr.Email));
            createIssue.Labels.Add("question");
            Issue issue = await client.Issue.Create("onesilop", "territory.mobi", createIssue).ConfigureAwait(false);
            if (issue != null)
            {
                TempData["UserMessage"] = $"Your message was sent, we'll have a look soon (contact reference - {issue.Number.ToString()})";
                TempData["UserMessageClass"] = "alert-success";
                Input.Body = "";
                Input.Subject = "";
            }
            else
            {
                TempData["UserMessage"] = "Something went wrong, try again maybe";
                TempData["UserMessageClass"] = "alert-danger";
            }
            return Page();
        }
    }
}