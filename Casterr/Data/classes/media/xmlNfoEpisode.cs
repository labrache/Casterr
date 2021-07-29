using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Casterr.Data.classes.media
{

	[XmlRoot(ElementName = "episodedetails")]
	public class Episodedetails
	{

		[XmlElement(ElementName = "title")]
		public string Title { get; set; }

		[XmlElement(ElementName = "season")]
		public int Season { get; set; }

		[XmlElement(ElementName = "episode")]
		public int Episode { get; set; }

		[XmlElement(ElementName = "aired")]
		public DateTime Aired { get; set; }

		[XmlElement(ElementName = "plot")]
		public string Plot { get; set; }

		[XmlElement(ElementName = "uniqueid")]
		public Uniqueid Uniqueid { get; set; }

		[XmlElement(ElementName = "thumb")]
		public string Thumb { get; set; }

		[XmlElement(ElementName = "watched")]
		public bool Watched { get; set; }

		[XmlElement(ElementName = "rating")]
		public float Rating { get; set; }

		[XmlElement(ElementName = "fileinfo")]
		public Fileinfo Fileinfo { get; set; }
	}

	[XmlRoot(ElementName = "Root")]
	public class EpisodeNfo
	{

		[XmlElement(ElementName = "episodedetails")]
		public List<Episodedetails> Episodedetails { get; set; }
	}

	[XmlRoot(ElementName = "tvshow")]
	public class TvshowNfo
	{

		[XmlElement(ElementName = "title")]
		public string Title { get; set; }

		[XmlElement(ElementName = "rating")]
		public float Rating { get; set; }

		[XmlElement(ElementName = "plot")]
		public string Plot { get; set; }

		[XmlElement(ElementName = "mpaa")]
		public string Mpaa { get; set; }

		[XmlElement(ElementName = "id")]
		public int Id { get; set; }

		[XmlElement(ElementName = "uniqueid")]
		public List<Uniqueid> Uniqueid { get; set; }

		[XmlElement(ElementName = "genre")]
		public List<string> Genre { get; set; }

		[XmlElement(ElementName = "premiered")]
		public DateTime Premiered { get; set; }

		[XmlElement(ElementName = "studio")]
		public string Studio { get; set; }

		[XmlElement(ElementName = "actor")]
		public List<Actor> Actor { get; set; }
	}
}
