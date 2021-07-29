using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using Casterr.Hubs;
using Casterr.Data.classes.media;
using Casterr.Data;
using Microsoft.Extensions.DependencyInjection;
using Casterr.Data.classes;

namespace Casterr.services
{
    public class MoviesBgService : bgService
    {
        private readonly ILogger<MoviesBgService> _logger;
        private ApplicationDbContext _ctx;
        private string _movieThumbPath;
        private RadarrAPI radarr;
        private CasterrConfig _casterr;
        private Boolean newInstallation = true;
        public MoviesBgService(ILogger<MoviesBgService> logger,
                        CasterrConfig casterr,
                        IWebHostEnvironment env,
                        IServiceProvider serviceProvider,
                        IHubContext<WebHub> hubContext)
        {
            _logger = logger;
            _ctx = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
            _casterr = casterr;
            _movieThumbPath = Path.Combine(env.ContentRootPath, "dataroot", "movies");
            if (!Directory.Exists(_movieThumbPath)) Directory.CreateDirectory(_movieThumbPath); else newInstallation = false;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
            _logger.LogDebug($"MoviesBgService is starting.");
            
            stoppingToken.Register(() =>
                _logger.LogDebug($"MoviesBgService is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                if(_casterr.config.radarr != null)
                {
                    radarr = new RadarrAPI(_casterr.config.radarr.url, _casterr.config.radarr.key);
                    if (DateTime.Now.Hour == 23 || newInstallation)
                    {
                        if (newInstallation)
                        {
                            newInstallation = false;
                            await Task.Delay(10000, stoppingToken);
                        }

                        _logger.LogDebug($"MoviesBgService doing background work.");
                        try
                        {
                            List<Radarr_Movie> movies = radarr.getRadarrMovies().Result;
                            List<int> grabId = new List<int>();
                            foreach (Radarr_Movie mov in movies)
                            {
                                if (mov.hasFile && !stoppingToken.IsCancellationRequested)
                                {
                                    grabId.Add(await executeMovieUpdate(mov));
                                }
                            }
                            if (!stoppingToken.IsCancellationRequested)
                            {
                                List<Movie> deleteMov = _ctx.movies.Where(s => !grabId.Contains(s.id)).ToList();
                                if (deleteMov.Count > 0)
                                {
                                    foreach (Movie delMov in deleteMov)
                                    {
                                        if (delMov.posterFile != null)
                                        {
                                            String posterFile = Path.Combine(_movieThumbPath, delMov.posterFile);
                                            if (File.Exists(posterFile)) File.Delete(posterFile);
                                        }
                                        if (delMov.fanartFile != null)
                                        {
                                            String fanartFile = Path.Combine(_movieThumbPath, delMov.fanartFile);
                                            if (File.Exists(fanartFile)) File.Delete(fanartFile);
                                        }
                                    }
                                    _ctx.movies.RemoveRange(deleteMov);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            _logger.LogError(e.Message);
                        }
                    }
                } else
                    _logger.LogError("Radarr is not configured");
                await Task.Delay(60000, stoppingToken);
            }

            _logger.LogDebug($"MoviesBgService is stopping.");
        }
        private async Task<int> executeMovieUpdate(Radarr_Movie mov)
        {
            Boolean update = false;
            
            Movie dbMov = _ctx.movies.Find(mov.id);
            if (dbMov == null)
                dbMov = new Movie();
            else
                update = true;
            dbMov.id = mov.id;
            dbMov.dirName = Path.GetFileName(mov.folderName);
            dbMov.fileName = mov.movieFile.relativePath;
            dbMov.Plot = mov.overview;
            dbMov.Title = mov.title;
            dbMov.originalTitle = mov.originalTitle;
            dbMov.movieSet = mov.collection == null ? null : mov.collection.name;
            dbMov.genre = String.Join(", ", mov.genres);
            dbMov.director = null;
            dbMov.studio = mov.studio;
            dbMov.premiered = mov.inCinemas;
            dbMov.rating = mov.ratings.value;
            dbMov.year = mov.year;
            dbMov.duration = mov.runtime;
            dbMov.publishDate = !update ? DateTime.Now : dbMov.publishDate;
            dbMov.keep = !update ? false : dbMov.keep;
            dbMov.posterFile = !update ? null : dbMov.posterFile;
            dbMov.fanartFile = !update ? null : dbMov.fanartFile;
            dbMov.status = mov.status;
            dbMov.movieJson = JsonSerializer.Serialize(mov.movieFile);
            if (!update)
            {
                _logger.LogDebug("Grap new movie: {0}",mov.title);
                String guid = Guid.NewGuid().ToString();
                String posterFile = String.Format("{0}_poster.jpg", guid);
                String fanartFile = String.Format("{0}_fanart.jpg", guid);

                String posterUrl = mov.images.Where(s => s.coverType == "poster").Select(s => s.url).SingleOrDefault();
                String fanartUrl = mov.images.Where(s => s.coverType == "fanart").Select(s => s.url).SingleOrDefault();
                if (posterUrl != null)
                {
                    await radarr.getImage(posterUrl, Path.Combine(_movieThumbPath, posterFile));
                    dbMov.posterFile = posterFile;
                }

                if (fanartUrl != null)
                {
                    await radarr.getImage(fanartUrl, Path.Combine(_movieThumbPath, fanartFile));
                    dbMov.fanartFile = fanartFile;
                }
                _ctx.movies.Add(dbMov);
            }
            else
                _ctx.movies.Update(dbMov);
            _ctx.SaveChanges();
            return mov.id;
        }
    }
}
