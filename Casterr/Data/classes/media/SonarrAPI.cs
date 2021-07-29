using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Casterr.Data.classes.media
{
    public class SonarrAPI
    {
        private String _apiKey;
        private HttpClient client;
        public SonarrAPI(String hostname, String apiKey)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(hostname);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            _apiKey = apiKey;
        }
        public async Task<List<Sonarr_Show>> getSonarrShows()
        {
            HttpResponseMessage response = await client.GetAsync(String.Format("api/series?apiKey={0}", _apiKey));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<Sonarr_Show>>();
        }
        public async Task<List<Sonarr_Show>> getSonarrShowsLookup(String term)
        {
            HttpResponseMessage response = await client.GetAsync(String.Format("api/series/lookup?apiKey={0}&term={1}", _apiKey, term));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<Sonarr_Show>>();
        }
        public async Task<List<Sonarr_Episode>> getSonarrEpisodes(int showId)
        {
            HttpResponseMessage response = await client.GetAsync(String.Format("api/episode?apiKey={0}&seriesId={1}", _apiKey, showId));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<Sonarr_Episode>>();
        }
        public async Task getImage(String urlPath, String localDir)
        {
            await File.WriteAllBytesAsync(localDir, await client.GetByteArrayAsync(String.Format("api{1}&apiKey={0}", _apiKey, urlPath)));
        }
    }
    public class Sonarr_AlternateTitle
    {
        public string title { get; set; }
        public int seasonNumber { get; set; }
    }

    public class Sonarr_Image
    {
        public string coverType { get; set; }
        public string url { get; set; }
        public string remoteUrl { get; set; }
    }

    public class Sonarr_Statistics
    {
        public int episodeFileCount { get; set; }
        public int episodeCount { get; set; }
        public int totalEpisodeCount { get; set; }
        public object sizeOnDisk { get; set; }
        public float percentOfEpisodes { get; set; }
        public DateTime? previousAiring { get; set; }
        public DateTime? nextAiring { get; set; }
    }

    public class Sonarr_Season
    {
        public int seasonNumber { get; set; }
        public bool monitored { get; set; }
        public Sonarr_Statistics statistics { get; set; }
    }

    public class Sonarr_Ratings
    {
        public int votes { get; set; }
        public float value { get; set; }
    }

    public class Sonarr_Show
    {
        public string title { get; set; }
        public List<Sonarr_AlternateTitle> alternateTitles { get; set; }
        public string sortTitle { get; set; }
        public int seasonCount { get; set; }
        public int totalEpisodeCount { get; set; }
        public int episodeCount { get; set; }
        public int episodeFileCount { get; set; }
        public object sizeOnDisk { get; set; }
        public string status { get; set; }
        public string overview { get; set; }
        public DateTime previousAiring { get; set; }
        public string network { get; set; }
        public string airTime { get; set; }
        public List<Sonarr_Image> images { get; set; }
        public List<Sonarr_Season> seasons { get; set; }
        public int year { get; set; }
        public string path { get; set; }
        public int profileId { get; set; }
        public int languageProfileId { get; set; }
        public bool seasonFolder { get; set; }
        public bool monitored { get; set; }
        public bool useSceneNumbering { get; set; }
        public int runtime { get; set; }
        public int tvdbId { get; set; }
        public int tvRageId { get; set; }
        public int tvMazeId { get; set; }
        public DateTime firstAired { get; set; }
        public DateTime lastInfoSync { get; set; }
        public string seriesType { get; set; }
        public string cleanTitle { get; set; }
        public string imdbId { get; set; }
        public string titleSlug { get; set; }
        public string certification { get; set; }
        public List<string> genres { get; set; }
        public List<object> tags { get; set; }
        public DateTime added { get; set; }
        public Sonarr_Ratings ratings { get; set; }
        public int qualityProfileId { get; set; }
        public int id { get; set; }
        public DateTime? nextAiring { get; set; }
    }

    public class Sonarr_Quality
    {
        public Sonarr_Quality2 quality { get; set; }
        public Sonarr_Revision revision { get; set; }
    }
    public class Sonarr_Quality2
    {
        public int id { get; set; }
        public string name { get; set; }
        public string source { get; set; }
        public int resolution { get; set; }
        public string modifier { get; set; }
    }
    public class Sonarr_Revision
    {
        public int version { get; set; }
        public int real { get; set; }
        public bool isRepack { get; set; }
    }

    public class Sonarr_Language
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Sonarr_MediaInfo
    {
        public double audioChannels { get; set; }
        public string audioCodec { get; set; }
        public string videoCodec { get; set; }
    }
    public class Sonarr_EpisodeFile
    {
        public int seriesId { get; set; }
        public int seasonNumber { get; set; }
        public string relativePath { get; set; }
        public string path { get; set; }
        public double size { get; set; }
        public DateTime dateAdded { get; set; }
        public Sonarr_Quality quality { get; set; }
        public Sonarr_Language language { get; set; }
        public Sonarr_MediaInfo mediaInfo { get; set; }
        public bool qualityCutoffNotMet { get; set; }
        public int id { get; set; }
    }
    public class Sonarr_Episode
    {
        public int seriesId { get; set; }
        public int episodeFileId { get; set; }
        public int seasonNumber { get; set; }
        public int episodeNumber { get; set; }
        public string title { get; set; }
        public string airDate { get; set; }
        public DateTime airDateUtc { get; set; }
        public string overview { get; set; }
        public Sonarr_EpisodeFile episodeFile { get; set; }
        public bool hasFile { get; set; }
        public bool monitored { get; set; }
        public int absoluteEpisodeNumber { get; set; }
        public bool unverifiedSceneNumbering { get; set; }
        public int id { get; set; }
    }
}
