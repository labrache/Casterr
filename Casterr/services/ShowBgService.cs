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
using Microsoft.EntityFrameworkCore;
using Casterr.Data;
using Microsoft.Extensions.DependencyInjection;
using Casterr.Data.classes.media;
using Casterr.Data.classes;

namespace Casterr.services
{
    public class ShowBgService : bgService
    {
        private readonly ILogger<ShowBgService> _logger;
        private ApplicationDbContext _ctx;
        private string _showThumbPath;
        private SonarrAPI sonarr;
        private CasterrConfig _casterr;
        private TmdbApi tmdb;
        private Boolean newInstallation = true;
        public ShowBgService(ILogger<ShowBgService> logger,
                CasterrConfig casterr,
                IServiceProvider serviceProvider,
                IWebHostEnvironment env)
        {
            _logger = logger;
            _ctx = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
            _casterr = casterr;
            _showThumbPath = Path.Combine(env.ContentRootPath, "dataroot", "shows");
            if (!Directory.Exists(_showThumbPath)) Directory.CreateDirectory(_showThumbPath); else newInstallation = false;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug($"ShowBgService is starting.");

            stoppingToken.Register(() =>
                _logger.LogDebug($"ShowBgService is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                if (_casterr.config.sonarr != null && _casterr.config.tmdb != null)
                {
                    sonarr = new SonarrAPI(_casterr.config.sonarr.url, _casterr.config.sonarr.key);
                    tmdb = new TmdbApi(_casterr.config.tmdb.key, _casterr.config.tmdb.lang);
                    if (DateTime.Now.Hour == 18 || newInstallation)
                    {
                        if (newInstallation)
                        {
                            newInstallation = false;
                            await Task.Delay(10000, stoppingToken);
                        }
                        _logger.LogDebug($"ShowBgService doing background work.");
                        try
                        {
                            List<Sonarr_Show> shows = sonarr.getSonarrShows().Result;
                            List<int> grabId = new List<int>();
                            foreach (Sonarr_Show show in shows)
                            {
                                if (!stoppingToken.IsCancellationRequested)
                                    grabId.Add(await executeShowUpdate(show));
                            }
                            if (!stoppingToken.IsCancellationRequested)
                            {
                                List<Tvshow> deleteShows = _ctx.tvshows.Where(s => !grabId.Contains(s.id)).ToList();
                                if (deleteShows.Count > 0)
                                {
                                    foreach (Tvshow delShow in deleteShows)
                                    {
                                        String thumbDir = Path.Combine(_showThumbPath, delShow.thumbStorage);
                                        if (Directory.Exists(thumbDir)) Directory.Delete(thumbDir, true);
                                    }
                                    _ctx.tvshows.RemoveRange(deleteShows);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            _logger.LogError(e.Message);
                        }
                    }
                }
                else
                    _logger.LogError("Sonarr / TMDB is not configured");
                await Task.Delay(60000, stoppingToken);
            }

            _logger.LogDebug($"ShowBgService is stopping.");
        }
        private async Task<int> executeShowUpdate(Sonarr_Show show)
        {
            Boolean update = false;
            TMDB_ShowInfo tSinfo = null;

            Tvshow dbShow = await _ctx.tvshows.Include(s => s.seasons).Include(s => s.episodes).FirstOrDefaultAsync(s => s.id == show.id);
            if (dbShow == null)
            {
                _logger.LogInformation("Add show {0}", show.title);
                dbShow = new Tvshow();
                dbShow.episodes = new List<Episode>();
            }
            else
                update = true;

            dbShow.id = show.id;
            dbShow.dirName = Path.GetFileName(show.path);
            dbShow.Plot = show.overview;
            dbShow.Title = show.title;
            dbShow.status = show.status;
            dbShow.network = show.network;
            dbShow.genre = String.Join(", ", show.genres);
            dbShow.premiered = show.firstAired;
            dbShow.rating = show.ratings.value;
            dbShow.publishDate = !update ? DateTime.Now : dbShow.publishDate;
            dbShow.posterFile = !update ? null : dbShow.posterFile;
            dbShow.fanartFile = !update ? null : dbShow.fanartFile;
            dbShow.bannerFile = !update ? null : dbShow.bannerFile;
            dbShow.imdbId = show.imdbId;
            dbShow.tvdbId = show.tvdbId;

            int? tmdbId = null;

            if (show.imdbId != null)
                tmdbId = await tmdb.findFromImdb(dbShow.imdbId);

            if (tmdbId == null)
                tmdbId = await tmdb.findFromTvdb(dbShow.tvdbId);

            if (tmdbId != null)
            {
                dbShow.tmdbId = (int)tmdbId;
                tSinfo = await tmdb.getShowInfo(dbShow.tmdbId);
                await tmdb.storeShowInfo(tSinfo);

                dbShow.Plot = tSinfo.overview;
                dbShow.Title = tSinfo.name;
                dbShow.OriginalTitle = tSinfo.original_name;
            }


            dbShow.thumbStorage = update ? dbShow.thumbStorage : Guid.NewGuid().ToString();
            String thumbStorage = Path.Combine(_showThumbPath, dbShow.thumbStorage);
            if (!Directory.Exists(thumbStorage)) Directory.CreateDirectory(thumbStorage);

            dbShow.seasons = new List<Seasons>();

            foreach (Sonarr_Season season in show.seasons)
            {
                Seasons aSeason = new Seasons();
                if (tSinfo != null)
                {
                    TMDB_SeasonInfo tmdb_season = tmdb.getSeasonCache(season.seasonNumber);
                    if (tmdb_season != null)
                    {
                        String posterFile = String.Format("S{0:00}_poster_{1}.jpg", season.seasonNumber, Guid.NewGuid().ToString());
                        aSeason.name = tmdb_season.name;
                        aSeason.Plot = tmdb_season.overview;
                        if (tmdb_season.poster_path != null)
                        {
                            await tmdb.getImage(tmdb_season.poster_path, Path.Combine(thumbStorage, posterFile));
                            aSeason.poster = posterFile;
                        }

                    }
                }

                if (aSeason.name == null)
                    aSeason.name = String.Format("{0:00}", season.seasonNumber);

                aSeason.seasonNumber = season.seasonNumber;
                aSeason.episodeFileCount = season.statistics.episodeFileCount;
                aSeason.episodeCount = season.statistics.episodeCount;
                aSeason.totalEpisodeCount = season.statistics.totalEpisodeCount;
                aSeason.percentOfEpisodes = season.statistics.percentOfEpisodes;

                dbShow.seasons.Add(aSeason);
            }

            List<Sonarr_Episode> episodes = sonarr.getSonarrEpisodes(show.id).Result;
            foreach (Sonarr_Episode episode in episodes)
            {
                if (episode.hasFile && episode.episodeFile != null)
                {
                    Episode dbEp = dbShow.episodes.Where(s => s.episode == episode.episodeNumber && s.season == episode.seasonNumber).SingleOrDefault();
                    if (dbEp == null)
                    {
                        _logger.LogInformation("Add episode {0} - S{1:00}E{2:00}", dbShow.Title, episode.seasonNumber, episode.episodeNumber);
                        dbEp = new Episode()
                        {
                            fileName = episode.episodeFile.relativePath,
                            season = episode.seasonNumber,
                            episode = episode.episodeNumber,
                            absoluteEpisodeNumber = episode.absoluteEpisodeNumber,
                            Plot = episode.overview,
                            Title = episode.title,
                            premiered = episode.airDateUtc,
                            publishDate = episode.episodeFile.dateAdded,
                            episodeJson = JsonSerializer.Serialize(episode.episodeFile)
                        };
                        TMDB_Episode tmdb_episode = tmdb.getEpisodeCache(episode.seasonNumber, episode.episodeNumber);
                        if (tmdb_episode != null)
                        {
                            dbEp.Plot = tmdb_episode.overview;
                            dbEp.Title = tmdb_episode.name;

                            String posterFile = String.Format("S{0:00}E{1:00}_{2}.jpg", episode.seasonNumber, episode.episodeNumber, Guid.NewGuid().ToString());
                            if (tmdb_episode.still_path != null)
                            {
                                await tmdb.getImage(tmdb_episode.still_path, Path.Combine(thumbStorage, posterFile));
                                dbEp.thumbFileName = posterFile;
                            }
                        }
                        dbShow.episodes.Add(dbEp);
                    }
                    else
                    {
                        //TODO
                    }
                }
            }

            if (!update)
            {
                String posterFile = String.Format("Show_Poster_{0}.jpg", Guid.NewGuid().ToString());
                String fanartFile = String.Format("Show_Fanart_{0}.jpg", Guid.NewGuid().ToString());
                String bannerFile = String.Format("Show_Banner_{0}.jpg", Guid.NewGuid().ToString());
                String tmdbPosterFile = String.Format("Show_TMDBPoster_{0}.jpg", Guid.NewGuid().ToString());
                String tmdbBackdropFile = String.Format("Show_TMDBBackdrop_{0}.jpg", Guid.NewGuid().ToString());

                String posterUrl = show.images.Where(s => s.coverType == "poster").Select(s => s.url).SingleOrDefault();
                String fanartUrl = show.images.Where(s => s.coverType == "fanart").Select(s => s.url).SingleOrDefault();
                String bannerUrl = show.images.Where(s => s.coverType == "banner").Select(s => s.url).SingleOrDefault();
                if (posterUrl != null)
                {
                    await sonarr.getImage(posterUrl, Path.Combine(thumbStorage, posterFile));
                    dbShow.posterFile = posterFile;
                }
                if (fanartUrl != null)
                {
                    await sonarr.getImage(fanartUrl, Path.Combine(thumbStorage, fanartFile));
                    dbShow.fanartFile = fanartFile;
                }
                if (bannerUrl != null)
                {
                    await sonarr.getImage(bannerUrl, Path.Combine(thumbStorage, bannerFile));
                    dbShow.bannerFile = bannerFile;
                }

                if (tSinfo != null)
                {
                    if (tSinfo.poster_path != null)
                    {
                        await tmdb.getImage(tSinfo.poster_path, Path.Combine(thumbStorage, tmdbPosterFile));
                        dbShow.tmdbPosterFile = tmdbPosterFile;
                    }
                    if (tSinfo.backdrop_path != null)
                    {
                        await tmdb.getImageOriginal(tSinfo.backdrop_path, Path.Combine(thumbStorage, tmdbBackdropFile));
                        dbShow.tmdbBackdropFile = tmdbBackdropFile;
                    }
                }

                _ctx.tvshows.Add(dbShow);
            }
            else
                _ctx.tvshows.Update(dbShow);
            _ctx.SaveChanges();
            return show.id;
        }
    }
}
