using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Casterr.Data.classes.media
{
    [ApiController]
    public class MediaServiceController : ControllerBase
    {
        private ApplicationDbContext _ctx;
        private IConfiguration _conf;
        private IWebHostEnvironment _env;
        public MediaServiceController(ApplicationDbContext context, IConfiguration configuration, IWebHostEnvironment env)
        {
            _ctx = context;
            _conf = configuration;
            _env = env;
        }

        [Route("get/movie/{id}")]
        public async Task<IActionResult> GetMovieAsync(int id)
        {
            Movie video = await _ctx.movies.Where(s => s.id == id).SingleOrDefaultAsync();
            if (video != null && video.dirName != null && video.fileName != null)
                return serveFile(Path.Combine(_conf["datavolume:movies"], video.dirName, video.fileName),video.fileName);
            else return NotFound();
        }
        [Route("get/episode/{id}")]
        public async Task<IActionResult> GetEpisodeAsync(int id)
        {
            Episode video = await _ctx.episodes.Include(s => s.show).Where(s => s.id == id).SingleOrDefaultAsync();
            if (video != null && video.fileName != null)
            {
                String filePath = Path.Combine(_conf["datavolume:tvshow"], video.show.dirName, video.fileName);
                return serveFile(filePath,video.fileName);
            }
            else return NotFound();
        }
        private IActionResult serveFile(String filePath, String name)
        {
            if (System.IO.File.Exists(filePath))
                return PhysicalFile(filePath, getMime(Path.GetExtension(filePath)), name, true);
            else return NotFound();
        }
        private string getMime(string ext)
        {
            string result = "application/octet-stream";
            switch (ext)
            {
                case ".mkv":
                    result = "video/x-matroska";
                    break;
                case ".aac":
                    result = "audio/aac";
                    break;
                case ".avi":
                    result = "video/x-msvideo";
                    break;
                case ".mpeg":
                    result = "video/mpeg";
                    break;
                case ".oga":
                    result = "audio/ogg";
                    break;
                case ".ogv":
                    result = "video/ogg";
                    break;
                case ".ogx":
                    result = "application/ogg";
                    break;
                case ".wav":
                    result = "audio/x-wav";
                    break;
                case ".weba":
                    result = "audio/webm";
                    break;
                case ".webm":
                    result = "video/webm";
                    break;
                case ".3gp":
                    result = "video/3gpp";
                    break;
                case ".3g2":
                    result = "video/3gpp2";
                    break;
                case ".mp3":
                    result = "audio/mpeg";
                    break;
                case ".wma":
                    result = "audio/x-ms-wma";
                    break;
                case ".mp4":
                    result = "video/mp4";
                    break;
                case ".wmv":
                    result = "video/x-ms-wmv";
                    break;
                case ".flv":
                    result = "video/x-flv";
                    break;
                case ".mov":
                    result = "video/quicktime";
                    break;
                case ".divx":
                    result = "video/divx";
                    break;
            }
            return result;
        }
    }
}
