using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Casterr.Data.classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Casterr.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly CasterrConfig _casterr;

        public IndexModel(ILogger<IndexModel> logger, CasterrConfig casterr)
        {
            _logger = logger;
            _casterr = casterr;
        }

        public void OnGet()
        {

        }
    }
}
