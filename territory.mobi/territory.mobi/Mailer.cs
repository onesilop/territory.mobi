using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SendGrid;
using SendGrid.Helpers.Mail;
using territory.mobi.Models;

namespace territory.mobi
{
    public class Mailer
    {
        private readonly territory.mobi.Models.TerritoryContext _context;
        public EmailAddress From { get; set; }
        public SendGridClient Client { get; set; }

        public Mailer(territory.mobi.Models.TerritoryContext Context)
        {
            _context = Context;
            Setting SendGridKey =  _context.Setting.Where(a => a.SettingType == "SendGridKey").FirstOrDefault();
            Setting FromAddress =  _context.Setting.Where(a => a.SettingType == "FromEmailAddress").FirstOrDefault();
            Setting FromName = _context.Setting.Where(a => a.SettingType == "FromName").FirstOrDefault();
            Client = new SendGridClient(SendGridKey.SettingValue);
            From = new EmailAddress(FromAddress.SettingValue, FromName.SettingValue);                       
        }


        public async Task<IActionResult> SendMailAsync(string addressee,string subject, string htmlContent, string plainTextContent)
        { 
            var to = new EmailAddress(addressee);
            var msg = MailHelper.CreateSingleEmail(From, to, subject, plainTextContent, htmlContent);
            var res = await Client.SendEmailAsync(msg);
            if (res.StatusCode == System.Net.HttpStatusCode.Accepted)
                { return new OkResult();}
            else
                { return new BadRequestResult(); }
        }
    }


}
