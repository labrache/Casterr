using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Casterr.Data.classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Casterr.Areas.Admin.Pages.Manage.Server
{
    [Authorize(Roles = "Admin")]
    public class MailSendModel : PageModel
    {
        private CasterrConfig _casterr;
        private readonly IEmailSender _emailSender;

        [BindProperty]
        [Required]
        [EmailAddress]
        [Display(Name = "Email address")]
        public String testMail { get; set; }
        public MailSendModel(CasterrConfig casterr, IEmailSender emailSender)
        {
            _casterr = casterr;
            _emailSender = emailSender;
        }
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await _emailSender.SendEmailAsync(testMail,"Confirm your email",_casterr.getMailTemplate("testmail"));
            return Page();
        }
    }
}
