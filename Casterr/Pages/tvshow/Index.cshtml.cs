using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.Extensions.Configuration;
using Casterr.Data.classes;
using Casterr.Data;

namespace Casterr.Pages.tvshow
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

        public PaginatedList<Tvshow> Tvshow { get;set; }

        public async Task OnGetAsync(string s, string q, int? p)
        {
            IQueryable<Tvshow> showList = _context.tvshows;

            ViewData["q"] = q;
            if (!String.IsNullOrEmpty(q))
            {
                showList = showList.Where(m => m.Title.Contains(q)
                                       || m.OriginalTitle.Contains(q));
            }
            String selSort = "date_desc";
            if (s != null)
                if (sortOptions.Contains(s))
                    selSort = s;
            ViewData["s"] = selSort;
            switch (selSort)
            {
                case "date":
                    showList = showList.OrderBy(m => m.premiered);
                    break;
                case "date_desc":
                    showList = showList.OrderByDescending(m => m.premiered);
                    break;
                case "publish":
                    showList = showList.OrderBy(m => m.publishDate);
                    break;
                case "publish_desc":
                    showList = showList.OrderByDescending(m => m.publishDate);
                    break;
            }

            Tvshow = await PaginatedList<Tvshow>.CreateAsync(showList, p ?? 1, pageSize);
        }
    }
}
