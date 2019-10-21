using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using territory.mobi.Models;

namespace territory.mobi.Pages.Admin.PageHelp
{
    public class EditModel : PageModel
    {
        private readonly territory.mobi.Models.TerritoryContext _context;

        public EditModel(territory.mobi.Models.TerritoryContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PageHelpText PageHelp { get; set; }

        public async Task<IActionResult> OnGetAsync(string ReturnURL, string ReturnQ,string id = "", string Section = "")
        {
            if (id == "")
            {
                return NotFound();
            }

            PageHelp = await _context.PageHelp.FirstOrDefaultAsync(m => m.PageId == id && m.SectionId == Section);

            if (PageHelp == null)
            {
                PageHelp = new PageHelpText
                {
                    PageId = id,
                    SectionId = Section
                };
            }
            if (ReturnQ != null)
            {
                ViewData["ReturnURL"] = string.Concat(ReturnURL, "?", ReturnQ);
            }
            else
            {
                ViewData["ReturnURL"] = ReturnURL;
            }
            string str = string.Concat("Help for ", PageHelp.PageId);
            if (PageHelp.SectionId != "")
            {
                str = string.Concat(str," Section ", PageHelp.SectionId);
            }
            ViewData["HelpFor"] = str;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string ReturnURL,string ReturnQ,string Section = "")
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            PageHelp.SectionId = Section;
            if (PageHelp.HtmlHelp != null)
            {
                PageHelp.HtmlHelp = PageHelp.HtmlHelp.ToString().Replace("\r\n", "<br>").Replace("\r", "<br>").Replace("\n", "<br>");
            }
                if (!PageHelpExists(PageHelp.PageId,PageHelp.SectionId))
            {
                _context.PageHelp.Add(PageHelp);
            }
            else
            { 
                _context.Attach(PageHelp).State = EntityState.Modified;
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PageHelpExists(PageHelp.PageId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            if (ReturnQ == null)
            {
                return RedirectToPage(ReturnURL);
            }
            else
            { 
                IDictionary<string, string> args = ParseQueryString(ReturnQ);
                return RedirectToPage(ReturnURL, args);
            }
        }

        private bool PageHelpExists(string id,string section="")
        {
            return _context.PageHelp.Any(e => e.PageId == id && e.SectionId == section);
        }

        public static Dictionary<string, string> ParseQueryString(string queryString)
        {
            var nvc = HttpUtility.ParseQueryString(queryString);
            return nvc.AllKeys.ToDictionary(k => k, k => nvc[k]);
        }
    }
}
