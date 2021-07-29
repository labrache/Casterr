using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System.ComponentModel.DataAnnotations;

namespace Casterr.Data.classes
{
    public class EmailSender : IEmailSender
    {
        public EmailSender(CasterrConfig casterr)
        {
            this.Options = casterr.emailOptions();
        }
        public EmailSenderOptions Options { get; set; }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // create message
            MimeMessage mail = new MimeMessage();
            mail.Sender = MailboxAddress.Parse(Options.Sender_EMail);
            if (!string.IsNullOrEmpty(Options.Sender_Name))
                mail.Sender.Name = Options.Sender_Name;
            mail.From.Add(mail.Sender);
            mail.To.Add(MailboxAddress.Parse(email));
            mail.Subject = subject;
            mail.Body = new TextPart(TextFormat.Html) { Text = htmlMessage };

            // send email
            using (var smtp = new SmtpClient())
            {
                smtp.Connect(Options.Host_Address, Options.Host_Port, Options.Host_SecureSocketOptions);
                smtp.Authenticate(Options.Host_Username, Options.Host_Password);
                smtp.Send(mail);
                smtp.Disconnect(true);
            }

            return Task.FromResult(true);
        }
    }
    public class EmailSenderOptions
    {
        public EmailSenderOptions()
        {
            Host_SecureSocketOptions = SecureSocketOptions.Auto;
        }
        [Required]
        [Display(Name = "SMTP Host address")]
        public string Host_Address { get; set; }
        [Required]
        [Display(Name = "SMTP Host port")]
        public int Host_Port { get; set; }
        [Required]
        [Display(Name = "SMTP Username")]
        public string Host_Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "SMTP Password")]
        public string Host_Password { get; set; }

        public SecureSocketOptions Host_SecureSocketOptions { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Sender email address")]
        public string Sender_EMail { get; set; }
        [Display(Name = "Sender name")]
        public string Sender_Name { get; set; }
    }
}
