using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Casterr.Data.classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace Casterr.Areas.Admin.Pages.Manage.Connections
{
    [Authorize(Roles = "Admin")]
    public class RadarrModel : PageModel
    {
        private CasterrConfig _casterr;

        [BindProperty]
        public string apiUrl { get; set; }
        [BindProperty]
        public string Key { get; set; }
        public RadarrModel(CasterrConfig casterr)
        {
            _casterr = casterr;
        }
        public void OnGet()
        {
            if(_casterr.config.radarr != null)
            {
                apiUrl = _casterr.config.radarr.url;
                Key = _casterr.config.radarr.key;
            }
        }
        public void OnPost()
        {
            _casterr.setRadarr(new CasterrConfig_ApiKey()
            {
                key = Key,
                url = apiUrl
            });
        }
    }
}
