using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Casterr.Data
{
    [Table("Movies")]
    public class Movie
    {
        public int id { get; set; }
        [MaxLength(128)]
        public String dirName { get; set; }
        [MaxLength(128)]
        public String fileName { get; set; }
        public String Plot { get; set; }
        [MaxLength(128)]
        public String Title { get; set; }
        [MaxLength(128)]
        public String originalTitle { get; set; }
        [MaxLength(128)]
        public String movieSet { get; set; }
        [MaxLength(256)]
        public String genre { get; set; }
        [MaxLength(128)]
        public String director { get; set; }
        public String studio { get; set; }
        public DateTime premiered { get; set; }
        public float rating { get; set; }
        public int year { get; set; }
        [MaxLength(16)]
        public int duration { get; set; }
        public DateTime publishDate { get; set; }
        public Boolean keep { get; set; }

        [MaxLength(64)]
        public String posterFile { get; set; }
        [MaxLength(64)]
        public String fanartFile { get; set; }

        [MaxLength(16)]
        public String status { get; set; }

        public String movieJson { get; set; }
    }

}
