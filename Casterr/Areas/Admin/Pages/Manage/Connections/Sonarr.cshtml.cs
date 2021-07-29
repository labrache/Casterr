using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Casterr.Data.classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Casterr.Areas.Admin.Pages.Manage.Connections
{
    [Authorize(Roles = "Admin")]
    public class SonarrModel : PageModel
    {
        private CasterrConfig _casterr;

        [BindProperty]
        public string apiUrl { get; set; }
        [BindProperty]
        public string Key { get; set; }
        public SonarrModel(CasterrConfig casterr)
        {
            _casterr = casterr;
        }
        public void OnGet()
        {
            if (_casterr.config.sonarr != null)
            {
                apiUrl = _casterr.config.sonarr.url;
                Key = _casterr.config.sonarr.key;
            }
        }
        public void OnPost()
        {
            _casterr.setSonarr(new CasterrConfig_ApiKey()
            {
                key = Key,
                url = apiUrl
            });
        }
    }
}
