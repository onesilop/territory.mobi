using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using territory.mobi.Areas.Identity.Data;
using territory.mobi.Models;

namespace territory.mobi
{
    public class Tokeniser
    {

        private readonly territory.mobi.Models.TerritoryContext _context;
        public Tokeniser(territory.mobi.Models.TerritoryContext Context)
        {
            _context = Context;
        }
        
        public async Task<string> GetUserToken(ApplicationUser user)
        {
            AspNetUserTokens tk = new AspNetUserTokens();
            tk.LoginProvider = "Email";
            tk.UserId = user.Id;
            tk.Name = "UserEmailConfirmation";
            tk.Value = Guid.NewGuid().ToString();

            _context.AspNetUserTokens.Add(tk);
            try
            {
                _ = await _context.SaveChangesAsync();
            }
            catch
            { 
                return "";
            }
            return tk.Value;
        }

        public async Task<Boolean> CheckUserToken(AspNetUsers user, string token)
        {
            AspNetUserTokens tk = await _context.AspNetUserTokens.FirstOrDefaultAsync(t => t.Value == token && t.UserId == user.Id);
            if (tk != null)
            {
                _context.AspNetUserTokens.Remove(tk);
                user.EmailConfirmed = true;
                _context.AspNetUsers.Attach(user);
            
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    throw new Exception("Couldnt Save the bleeding user");
                }
                return true;
            }
            return false;
        }

    }
}
