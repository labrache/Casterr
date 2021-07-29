using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Casterr.Data
{
    [Table("TVShows")]
    public class Tvshow
    {
        public int id { get; set; }
        [MaxLength(128)]
        public String dirName { get; set; }
        public String Plot { get; set; }
        [MaxLength(128)]
        public String Title { get; set; }
        [MaxLength(128)]
        public String OriginalTitle { get; set; }
        [MaxLength(16)]
        public String status { get; set; }
        [MaxLength(64)]
        public String network { get; set; }
        [MaxLength(256)]
        public String genre { get; set; }
        public DateTime premiered { get; set; }
        public float rating { get; set; }
        public DateTime publishDate { get; set; }
        [MaxLength(16)]
        public String imdbId { get; set; }
        public int tmdbId { get; set; }
        public int tvdbId { get; set; }
        public virtual ICollection<Episode> episodes { get; set; }
        public virtual ICollection<Seasons> seasons { get; set; }
        [MaxLength(64)]
        public String thumbStorage { get; set; }
        [MaxLength(64)]
        public String posterFile { get; set; }
        [MaxLength(64)]
        public String tmdbPosterFile { get; set; }
        [MaxLength(64)]
        public String tmdbBackdropFile { get; set; }
        [MaxLength(64)]
        public String fanartFile { get; set; }
        [MaxLength(64)]
        public String bannerFile { get; set; }
    }
    [Table("Episode")]
    public class Episode
    {
        public int id { get; set; }
        [MaxLength(128)]
        public String fileName { get; set; }
        public int season { get; set; }
        public int episode { get; set; }
        public int absoluteEpisodeNumber { get; set; }
        [MaxLength(64)]
        public String thumbFileName { get; set; }
        public String Plot { get; set; }
        [MaxLength(128)]
        public String Title { get; set; }
        public DateTime premiered { get; set; }
        public DateTime publishDate { get; set; }
        public String episodeJson { get; set; }
        public virtual Tvshow show { get; set; }
    }
    [Table("Seasons")]
    public class Seasons
    {
        public int id { get; set; }
        public int seasonNumber { get; set; }
        [MaxLength(32)]
        public String name { get; set; }
        public String Plot { get; set; }
        [MaxLength(64)]
        public String poster { get; set; }
        public int episodeFileCount { get; set; }
        public int episodeCount { get; set; }
        public int totalEpisodeCount { get; set; }
        public float percentOfEpisodes { get; set; }

        public virtual Tvshow show { get; set; }
    }
}
