using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using Casterr.Data;

namespace Casterr.Pages.tvshow
{
    public class EpisodesModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _conf;

        public EpisodesModel(ApplicationDbContext context, IConfiguration Configuration)
        {
            _context = context;
            _conf = Configuration;
        }

        public Tvshow Tvshow { get; set; }
        public Seasons Season { get; set; }
        public IList<Episode> Episode { get;set; }
        public async Task<IActionResult> OnGetAsync(int? id, int? season)
        {
            if (id == null)
            {
                return NotFound();
            }

            Tvshow = await _context.tvshows.Where(s => s.id == id).Include(s => s.seasons.OrderBy(s => s.name)).Include(s => s.episodes.OrderBy(e => e.season).ThenBy(e => e.episode)).SingleOrDefaultAsync();

            if (Tvshow == null)
            {
                return NotFound();
            }
            if (season == null)
                Episode = Tvshow.episodes.ToList();
            else
            {
                Season = Tvshow.seasons.Where(s => s.seasonNumber == season).SingleOrDefault();
                if (Season != null)
                    Episode = Tvshow.episodes.Where(s => s.season == Season.seasonNumber).ToList();
                else return NotFound();
            }
            return Page();
        }
    }
}
