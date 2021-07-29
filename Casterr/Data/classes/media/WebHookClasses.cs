using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casterr.Data.classes.media
{
    public class WH_Series
    {
        public int id { get; set; }
        public string title { get; set; }
        public string path { get; set; }
        public int tvdbId { get; set; }
        public int tvMazeId { get; set; }
        public string type { get; set; }
    }

    public class WH_Episode
    {
        public int id { get; set; }
        public int episodeNumber { get; set; }
        public int seasonNumber { get; set; }
        public string title { get; set; }
    }
    public enum WebhookEventType
    {
        Test,
        Grab,
        Download,
        Rename,
        SeriesDelete,
        EpisodeFileDelete,
        Health
    }
    public class WH_Sonarr
    {
        public WH_Series series { get; set; }
        public List<WH_Episode> episodes { get; set; }
        public WebhookEventType eventType { get; set; }
    }

    public class WH_Movie
    {
        public int id { get; set; }
        public string title { get; set; }
        public int year { get; set; }
        public string releaseDate { get; set; }
        public string folderPath { get; set; }
        public int tmdbId { get; set; }
    }

    public class WH_RemoteMovie
    {
        public int tmdbId { get; set; }
        public string imdbId { get; set; }
        public string title { get; set; }
        public int year { get; set; }
    }

    public class WH_Release
    {
        public string quality { get; set; }
        public int qualityVersion { get; set; }
        public string releaseGroup { get; set; }
        public string releaseTitle { get; set; }
        public string indexer { get; set; }
        public int size { get; set; }
    }

    public class WH_Radarr
    {
        public WH_Movie movie { get; set; }
        public WH_RemoteMovie remoteMovie { get; set; }
        public WH_Release release { get; set; }
        public string eventType { get; set; }
    }
}
