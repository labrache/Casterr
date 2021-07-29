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
    public class tmdbModel : PageModel
    {
        private CasterrConfig _casterr;

        [BindProperty]
        public string lang { get; set; }
        [BindProperty]
        public string Key { get; set; }
        public tmdbModel(CasterrConfig casterr)
        {
            _casterr = casterr;
        }
        public void OnGet()
        {
            if (_casterr.config.tmdb != null)
            {
                lang = _casterr.config.tmdb.lang;
                Key = _casterr.config.tmdb.key;
            }
        }
        public void OnPost()
        {
            _casterr.setTmdb(new CasterrConfig_tmdbApi()
            {
                key = Key,
                lang = lang
            });
        }
    }
}

