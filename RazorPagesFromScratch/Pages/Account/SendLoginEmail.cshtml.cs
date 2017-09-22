using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesFromScratch.Models;
using RazorPagesFromScratch.Extensions;
using MimeKit;
using System.IO;
using NETCore.MailKit.Core;

namespace RazorPagesFromScratch.Pages.Account
{
    public class SendLoginEmailModel : PageModel
    {
        private readonly AppDbContext db;
        readonly IEmailService emailService;

        public SendLoginEmailModel(AppDbContext db, IEmailService emailService)
        {
            this.emailService = emailService;
            this.db = db;
        }
        [BindProperty]
        public string Email { get; set; }
        public IActionResult OnPost()
        {
            var uid = System.Guid.NewGuid().ToString("N");
            var token = new Token { Email = Email, Uid = uid };
            db.Tokens.Add(token);
            db.SaveChanges();
            Console.Error.WriteLine($"Saving uid {uid} for email {Email}");
           var url = Url.AbsoluteContent($"/account/login?uid={uid}");
            SendEmailPickup(
                "Your login link for Superlists",
                $"Use this link to log in:\n\n{url}",
              new string[] { Email });
            return RedirectToPage("LoginEmailSent");


        }

        private void SendEmail(string subject, string body,string to_emails)
        {

            emailService.Send(to_emails, subject, body, isHtml: true);
        }

        public void  SendEmailPickup( string subject, string message, string[] emails)
        {
            var emailMessage = new MimeMessage();
            foreach (var email in emails)
            {
                emailMessage.To.Add(new MailboxAddress("", email));
            }

            emailMessage.From.Add(new MailboxAddress("", "noreply@example.com"));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = message };

            using (StreamWriter data = System.IO.File.CreateText("c:\\temp\\mailbox\\email.eml"))
            {
                emailMessage.WriteTo(data.BaseStream);
            }
        }
    }
}