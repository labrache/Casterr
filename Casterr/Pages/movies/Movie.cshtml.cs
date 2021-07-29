using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text.Json;
using Casterr.Data;
using Casterr.Data.classes.media;
using Casterr.Data.classes;

namespace Casterr.Pages.movies
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

        public Movie Movie { get; set; }
        public Radarr_MovieFile releaseInfo { get; set; }
        public String movieSize { get; set; }
        public List<Movie> MovieSet { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Movie = await _context.movies.FirstOrDefaultAsync(m => m.id == id);

            if (Movie == null)
            {
                return NotFound();
            }
            releaseInfo = JsonSerializer.Deserialize<Radarr_MovieFile>(Movie.movieJson);
            movieSize = Utility.humanReadableSize(releaseInfo.size);
            if (Movie.movieSet != null)
                MovieSet = await _context.movies.Where(s => s.movieSet == Movie.movieSet).OrderByDescending(s => s.premiered).ToListAsync();
            return Page();
        }
    }
}
