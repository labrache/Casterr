using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Casterr.Data.classes.media
{
    public class RadarrAPI
    {
        private String _apiKey;
        private HttpClient client;
        public RadarrAPI(String hostname, String apiKey)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(hostname);
            /*
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            */
            _apiKey = apiKey;
        }

        public async Task<List<Radarr_Movie>> getRadarrMovies()
        {
            HttpResponseMessage response = await client.GetAsync(String.Format("api/v3/movie?apiKey={0}", _apiKey));
            response.EnsureSuccessStatusCode();
            return await response.Content. ReadAsAsync<List<Radarr_Movie>>();
        }
        public async Task<List<Radarr_Movie>> getRadarrMoviesLookup(String term)
        {
            HttpResponseMessage response = await client.GetAsync(String.Format("api/v3/movie/lookup?apiKey={0}&term={1}", _apiKey,term));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<Radarr_Movie>>();
        }
        public async Task getImage(String urlPath, String localDir)
        {
            await File.WriteAllBytesAsync(localDir, await client.GetByteArrayAsync(String.Format("api{1}&apiKey={0}", _apiKey, urlPath)));
        }


    }
    public class Radarr_Language
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Radarr_AlternateTitle
    {
        public string sourceType { get; set; }
        public int movieId { get; set; }
        public string title { get; set; }
        public int sourceId { get; set; }
        public int votes { get; set; }
        public int voteCount { get; set; }
        public Radarr_Language language { get; set; }
        public int id { get; set; }
    }

    public class Radarr_Image
    {
        public string coverType { get; set; }
        public string url { get; set; }
        public string remoteUrl { get; set; }
    }

    public class Radarr_Ratings
    {
        public int votes { get; set; }
        public float value { get; set; }
    }
    public class Radarr_Quality
    {
        public Radarr_Quality2 quality { get; set; }
        public Radarr_Revision revision { get; set; }
    }
    public class Radarr_Quality2
    {
        public int id { get; set; }
        public string name { get; set; }
        public string source { get; set; }
        public int resolution { get; set; }
        public string modifier { get; set; }
    }

    public class Radarr_Revision
    {
        public int version { get; set; }
        public int real { get; set; }
        public bool isRepack { get; set; }
    }

    public class Radarr_MediaInfo
    {
        public string audioAdditionalFeatures { get; set; }
        public int audioBitrate { get; set; }
        public double audioChannels { get; set; }
        public string audioCodec { get; set; }
        public string audioLanguages { get; set; }
        public int audioStreamCount { get; set; }
        public int videoBitDepth { get; set; }
        public int videoBitrate { get; set; }
        public string videoCodec { get; set; }
        public double videoFps { get; set; }
        public string resolution { get; set; }
        public string runTime { get; set; }
        public string scanType { get; set; }
        public string subtitles { get; set; }
    }

    public class Radarr_Language2
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Radarr_MovieFile
    {
        public int movieId { get; set; }
        public string relativePath { get; set; }
        public string path { get; set; }
        public double size { get; set; }
        public DateTime dateAdded { get; set; }
        public int indexerFlags { get; set; }
        public Radarr_Quality quality { get; set; }
        public Radarr_MediaInfo mediaInfo { get; set; }
        public bool qualityCutoffNotMet { get; set; }
        public List<Radarr_Language> languages { get; set; }
        public string edition { get; set; }
        public int id { get; set; }
        public string releaseGroup { get; set; }
        public string originalFilePath { get; set; }
        public string sceneName { get; set; }
    }

    public class Radarr_Collection
    {
        public string name { get; set; }
        public int tmdbId { get; set; }
        public List<object> images { get; set; }
    }

    public class Radarr_Movie
    {
        public string title { get; set; }
        public string originalTitle { get; set; }
        public List<Radarr_AlternateTitle> alternateTitles { get; set; }
        public int secondaryYearSourceId { get; set; }
        public string sortTitle { get; set; }
        public object sizeOnDisk { get; set; }
        public string status { get; set; }
        public string overview { get; set; }
        public DateTime inCinemas { get; set; }
        public DateTime physicalRelease { get; set; }
        public DateTime digitalRelease { get; set; }
        public List<Radarr_Image> images { get; set; }
        public string website { get; set; }
        public int year { get; set; }
        public bool hasFile { get; set; }
        public string youTubeTrailerId { get; set; }
        public string studio { get; set; }
        public string path { get; set; }
        public int qualityProfileId { get; set; }
        public bool monitored { get; set; }
        public string minimumAvailability { get; set; }
        public bool isAvailable { get; set; }
        public string folderName { get; set; }
        public int runtime { get; set; }
        public string cleanTitle { get; set; }
        public string imdbId { get; set; }
        public int tmdbId { get; set; }
        public string titleSlug { get; set; }
        public string certification { get; set; }
        public List<string> genres { get; set; }
        public List<object> tags { get; set; }
        public DateTime added { get; set; }
        public Radarr_Ratings ratings { get; set; }
        public Radarr_MovieFile movieFile { get; set; }
        public int id { get; set; }
        public Radarr_Collection collection { get; set; }
        public int? secondaryYear { get; set; }
    }
}
