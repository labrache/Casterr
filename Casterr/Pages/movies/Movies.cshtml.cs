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
using Casterr.Data.classes;

namespace Casterr.Pages.movies
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _conf;
        private List<String> sortOptions = new List<string>() { "date", "date_desc", "publish", "publish_desc" };
        int pageSize = 12;
        public IndexModel(ApplicationDbContext context, IConfiguration Configuration)
        {
            _context = context;
            _conf = Configuration;
        }

        public PaginatedList<Movie> Movies { get;set; }

        public async Task OnGetAsync(string s, string q, int? p)
        {
            IQueryable<Movie> moviesList = _context.movies;
            //Recherche
            ViewData["q"] = q;
            if (!String.IsNullOrEmpty(q))
            {
                moviesList = moviesList.Where(m => m.Title.Contains(q)
                                       || m.originalTitle.Contains(q));
            }
            //Tri
            String selSort = "date_desc";
            if (s != null)
                if (sortOptions.Contains(s))
                    selSort = s;
            ViewData["s"] = selSort;
            switch (selSort)
            {
                case "date":
                    moviesList = moviesList.OrderBy(m => m.premiered);
                    break;
                case "date_desc":
                    moviesList = moviesList.OrderByDescending(m => m.premiered);
                    break;
                case "publish":
                    moviesList = moviesList.OrderBy(m => m.publishDate);
                    break;
                case "publish_desc":
                    moviesList = moviesList.OrderByDescending(m => m.publishDate);
                    break;
            }
            Movies = await PaginatedList<Movie>.CreateAsync(moviesList, p ?? 1, pageSize);            
        }
    }
}
