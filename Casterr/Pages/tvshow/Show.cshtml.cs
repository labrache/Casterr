using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using Casterr.Data;

namespace Casterr.Pages.tvshow
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _conf;
        public DetailsModel(ApplicationDbContext context, IConfiguration Configuration)
        {
            _context = context;
            _conf = Configuration;
        }

        public Tvshow Tvshow { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Tvshow = await _context.tvshows.Include(s => s.seasons.Where(s => s.episodeFileCount > 0).OrderBy(s => s.seasonNumber)).FirstOrDefaultAsync(m => m.id == id);
            if (Tvshow == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
