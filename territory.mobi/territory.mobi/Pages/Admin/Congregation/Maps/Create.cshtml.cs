﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using territory.mobi.Models;

namespace territory.mobi.Pages.Admin.Congregation.Maps
{
    public class CreateModel : PageModel
    {
        private readonly territory.mobi.Models.TerritoryContext _context;

        public CreateModel(territory.mobi.Models.TerritoryContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Map Map { get; set; }

        public IActionResult OnGet(Guid id)
        {
            if (id == null)
            { 
                return NotFound();
            }
            Map.CongId = id;    
            return Page();
        }



        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            if (!ModelState.IsValid || id == null)
            {
                return Page();
            }

            Map.CongId = id;
            _context.Map.Add(Map);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}