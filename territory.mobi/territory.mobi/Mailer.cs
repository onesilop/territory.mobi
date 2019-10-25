using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using territory.mobi.Areas.Identity.Data;
using territory.mobi.Models;

namespace territory.mobi
{

    public class Mailer
    {
        private class UserInviteTemplateData
        {
            [JsonProperty("CongName")]
            public string CongName { get; set; }

            [JsonProperty("URL")]
            public string URL { get; set; }
        }


        private readonly territory.mobi.Models.TerritoryContext _context;

        private string SiteAddress { get; set; }
        private EmailAddress From { get; set; }
        private SendGridClient Client { get; set; }
        private string UserInviteTemplateID { get; set; }


        public Mailer(territory.mobi.Models.TerritoryContext Context)
        {
            _context = Context;
            Setting SendGridKey = _context.Setting.Where(a => a.SettingType == "SendGridKey").FirstOrDefault();
            Setting FromAddress = _context.Setting.Where(a => a.SettingType == "FromEmailAddress").FirstOrDefault();
            Setting FromName = _context.Setting.Where(a => a.SettingType == "FromName").FirstOrDefault();
            Setting UserInviteT = _context.Setting.Where(a => a.SettingType == "UserInviteTemp").FirstOrDefault();
            Setting SiteAdd = _context.Setting.Where(a => a.SettingType == "SiteAddress").FirstOrDefault();

            Client = new SendGridClient(SendGridKey.SettingValue);
            From = new EmailAddress(FromAddress.SettingValue, FromName.SettingValue);
            UserInviteTemplateID = UserInviteT.SettingValue;
            SiteAddress = SiteAdd.SettingValue.ToString();
            SiteAddress = "https://localhost:44339";
        }


        public async Task<IActionResult> SendMailAsync(string addressee, string subject, string htmlContent, string plainTextContent)
        {
            return (await Client.SendEmailAsync(MailHelper.CreateSingleEmail(From, new EmailAddress(addressee), subject, plainTextContent, htmlContent))).StatusCode == System.Net.HttpStatusCode.Accepted ? new OkResult() : (IActionResult)new BadRequestResult();
        }

       
        public async Task<IActionResult> SendUserInviteMail (string addressee, string tokenId, string CName)
        {
            UserInviteTemplateData dynamicTemplateData = new UserInviteTemplateData
            {
                CongName = CName,
                URL = SiteAddress + "/Identity/Account/Register?token=" + tokenId,
            };

            SendGridMessage msg = MailHelper.CreateSingleTemplateEmail(From, new EmailAddress(addressee), UserInviteTemplateID, dynamicTemplateData);
            return (await Client.SendEmailAsync(msg)).StatusCode == System.Net.HttpStatusCode.Accepted ? new OkResult() : (IActionResult)new BadRequestResult();
        }

        public async Task<IActionResult> SendUserApprovalRequest(string addressee, string UserName, string CName, string CongId, Boolean NewCong = false)
        {

            string RedirectURL = SiteAddress + "/Admin/Congregation/Edit?id=" + CongId;
            if (NewCong)
            {
                return await SendMailAsync(addressee, "A new congregation " + CName + " has been created.",
                                          UserName + " has created and requested to be added to the " + CName + " congregation.<br>Please log into <a href='" + HtmlEncoder.Default.Encode(RedirectURL) + "'>terrirtory.mobi</a> and approve or reject their request.", null);
            }
            return await SendMailAsync(addressee, "A new user has requested to be added to the " + CName + " congregation on territory.mobi.",
                                              UserName + " has requested to be added to the " + CName + " congregation.<br>Please log into <a href='" + HtmlEncoder.Default.Encode(RedirectURL) + "'>terrirtory.mobi</a> and approve or reject their request.", null);
        }

        public async Task<IActionResult> SendUserAddition(string addressee, string UserName, string CName, string CongId)
        {

            string RedirectURL = SiteAddress + "/Admin/Congregation/Edit?id=" + CongId;


            return await SendMailAsync(addressee, "A new user has been added to the " + CName + " congregation on territory.mobi.",
                                         UserName + " has been added to the " + CName + " congregation.<br>Please log into <a href='" + HtmlEncoder.Default.Encode(RedirectURL) + "'>terrirtory.mobi</a> if this has been incorrectly provided and remove thier access.", null);

        }

        public async Task<IActionResult> SendNewCongregation(string addressee, string CName, string CongId)
        {

            string RedirectURL = SiteAddress + "/Admin/Congregation/Edit?id=" + CongId;


            return await SendMailAsync(addressee, CName + " congregation has been created on territory.mobi.",
                                         CName + " congregation has been created on territori.mobi.<br>Please log into <a href='" + HtmlEncoder.Default.Encode(RedirectURL) + "'>terrirtory.mobi</a> to begin administering your maps.", null);

        }

        public async Task<IActionResult> SendUserEmailConfirmation(ApplicationUser user, string code)
        {

            string callbackUrl = SiteAddress + "/Identity/Account/ConfirmEmail?userId=" + user.Id + "&code=" + code;

            return await SendMailAsync(user.Email, "Confirm your email",
                                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.", null);
            
        }

       

    }
}
 