using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Casterr.Data;
using Casterr.Data.classes.media;
using Casterr.Data.classes;

namespace Casterr.Pages.tvshow
{
    public class EpisodeModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EpisodeModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Episode Episode { get; set; }
        public Episode NextEpisode { get; set; }
        public Episode PrevEpisode { get; set; }
        public Sonarr_EpisodeFile releaseInfo { get; set; }
        public String episodeSize { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Episode = await _context.episodes.Include(s => s.show).FirstOrDefaultAsync(m => m.id == id);

            if (Episode == null)
            {
                return NotFound();
            }

            NextEpisode = await _context.episodes.Where(s => s.show == Episode.show && s.season == Episode.season && s.episode == Episode.episode + 1).SingleOrDefaultAsync();
            PrevEpisode = await _context.episodes.Where(s => s.show == Episode.show && s.season == Episode.season && s.episode == Episode.episode - 1).SingleOrDefaultAsync();

            releaseInfo = JsonSerializer.Deserialize<Sonarr_EpisodeFile>(Episode.episodeJson);
            episodeSize = Utility.humanReadableSize(releaseInfo.size);
            return Page();
        }
    }
}
