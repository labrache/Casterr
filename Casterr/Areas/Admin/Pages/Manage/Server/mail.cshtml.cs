using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Casterr.Data.classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Casterr.Areas.Admin.Pages.Manage.Server
{
    [Authorize(Roles = "Admin")]
    public class mailModel : PageModel
    {
        private CasterrConfig _casterr;
        [BindProperty]
        public EmailSenderOptions emailConfig { get; set; }
        public mailModel(CasterrConfig casterr)
        {
            _casterr = casterr;
        }
        public void OnGet()
        {
            if (_casterr.config.mailOptions != null)
            {
                emailConfig = _casterr.config.mailOptions;
            }
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _casterr.setEmail(emailConfig);
            return Page();
        }
    }
}
