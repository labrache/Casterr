using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Casterr.Data.classes.media
{
    public class TmdbApi
    {
        private String _apiKey;
        private String _lang;
        private HttpClient client;
        private HttpClient imageClient;
        private Dictionary<int, TMDB_SeasonInfo> storedSeasons = new Dictionary<int, TMDB_SeasonInfo>();
        public TmdbApi(String apiKey, String language) //fr-FR
        {
            client = new HttpClient();
            imageClient = new HttpClient();
            client.BaseAddress = new Uri("https://api.themoviedb.org");
            imageClient.BaseAddress = new Uri("https://image.tmdb.org/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            _apiKey = apiKey;
            _lang = language;
        }
        public async Task<int?> findFromImdb(string imdbId)
        {
            HttpResponseMessage response = await client.GetAsync(String.Format("3/find/{0}?api_key={1}&external_source=imdb_id&language={2}", imdbId, _apiKey, _lang));
            if (response.IsSuccessStatusCode)
            {
                TMDB_SearchResult rs = await response.Content.ReadAsAsync<TMDB_SearchResult>();
                if (rs.tv_results.Count == 1)
                    return rs.tv_results[0].id;
                else return null;
            }
            else return null;
        }
        public async Task<int?> findFromTvdb(int tvdbId)
        {
            HttpResponseMessage response = await client.GetAsync(String.Format("3/find/{0}?api_key={1}&external_source=tvdb_id&language={2}", tvdbId, _apiKey, _lang));
            if (response.IsSuccessStatusCode)
            {
                TMDB_SearchResult rs = await response.Content.ReadAsAsync<TMDB_SearchResult>();
                if (rs.tv_results.Count == 1)
                    return rs.tv_results[0].id;
                else return null;
            }
            else return null;
        }
        public async Task<TMDB_EpisodeSearch> getEpisodeInfo(int showId, int season, int episode)
        {
            HttpResponseMessage response = await client.GetAsync(String.Format("3/tv/{0}/season/{1}/episode/{2}?api_key={3}&language={4}", showId, season, episode, _apiKey,_lang));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<TMDB_EpisodeSearch>();
        }
        public async Task<TMDB_SeasonInfo> getSeasonInfo(int showId, int season)
        {
            HttpResponseMessage response = await client.GetAsync(String.Format("3/tv/{0}/season/{1}?api_key={2}&language={3}", showId, season, _apiKey, _lang));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<TMDB_SeasonInfo>();
        }
        public async Task<TMDB_ShowInfo> getShowInfo(int showId)
        {
            HttpResponseMessage response = await client.GetAsync(String.Format("3/tv/{0}?api_key={1}&language={2}", showId, _apiKey, _lang));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<TMDB_ShowInfo>();
        }
        public async Task storeShowInfo(TMDB_ShowInfo show)
        {
            storedSeasons.Clear();
            foreach (TMDB_Season aSeason in show.seasons)
                storedSeasons.Add(aSeason.season_number, await getSeasonInfo(show.id, aSeason.season_number));
        }
        public TMDB_SeasonInfo getSeasonCache(int seasonNum)
        {
            if (storedSeasons.ContainsKey(seasonNum))
                return storedSeasons[seasonNum];
            else return null;
        }
        public TMDB_Episode getEpisodeCache(int seasonNum, int episodeNum)
        {
            if (storedSeasons.ContainsKey(seasonNum))
                return storedSeasons[seasonNum].episodes.Where(s => s.episode_number == episodeNum).SingleOrDefault();
            else return null;
        }

        public async Task getImage(String urlPath, String localDir)
        {
            await File.WriteAllBytesAsync(localDir, await imageClient.GetByteArrayAsync(String.Format("t/p/w500{0}", urlPath)));
        }
        public async Task getImageOriginal(String urlPath, String localDir)
        {
            await File.WriteAllBytesAsync(localDir, await imageClient.GetByteArrayAsync(String.Format("t/p/original{0}", urlPath)));
        }
    }

    public class TMDB_TvResult
    {
        public string name { get; set; }
        public string poster_path { get; set; }
        public int id { get; set; }
        public string backdrop_path { get; set; }
        public int vote_count { get; set; }
        public string first_air_date { get; set; }
        public string original_name { get; set; }
        public List<string> origin_country { get; set; }
        public double vote_average { get; set; }
        public string overview { get; set; }
        public List<int> genre_ids { get; set; }
        public string original_language { get; set; }
        public double popularity { get; set; }
    }

    public class TMDB_SearchResult
    {
        public List<object> movie_results { get; set; }
        public List<object> person_results { get; set; }
        public List<TMDB_TvResult> tv_results { get; set; }
        public List<object> tv_episode_results { get; set; }
        public List<object> tv_season_results { get; set; }
    }
    public class TMDB_Crew
    {
        public string job { get; set; }
        public string department { get; set; }
        public string credit_id { get; set; }
        public bool adult { get; set; }
        public int gender { get; set; }
        public int id { get; set; }
        public string known_for_department { get; set; }
        public string name { get; set; }
        public string original_name { get; set; }
        public double popularity { get; set; }
        public string profile_path { get; set; }
    }
    public class TMDB_GuestStar
    {
        public string character { get; set; }
        public string credit_id { get; set; }
        public int order { get; set; }
        public bool adult { get; set; }
        public int gender { get; set; }
        public int id { get; set; }
        public string known_for_department { get; set; }
        public string name { get; set; }
        public string original_name { get; set; }
        public double popularity { get; set; }
        public string profile_path { get; set; }
    }
    public class TMDB_EpisodeSearch
    {
        public string air_date { get; set; }
        public List<TMDB_Crew> crew { get; set; }
        public int episode_number { get; set; }
        public List<TMDB_GuestStar> guest_stars { get; set; }
        public string name { get; set; }
        public string overview { get; set; }
        public int id { get; set; }
        public string production_code { get; set; }
        public int season_number { get; set; }
        public string still_path { get; set; }
        public double vote_average { get; set; }
        public int vote_count { get; set; }
    }
    public class TMDB_Episode
    {
        public string air_date { get; set; }
        public int episode_number { get; set; }
        public List<TMDB_Crew> crew { get; set; }
        public List<TMDB_GuestStar> guest_stars { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string overview { get; set; }
        public string production_code { get; set; }
        public int season_number { get; set; }
        public string still_path { get; set; }
        public double vote_average { get; set; }
        public int vote_count { get; set; }
    }

    public class TMDB_SeasonInfo
    {
        public string _id { get; set; }
        public string air_date { get; set; }
        public List<TMDB_Episode> episodes { get; set; }
        public string name { get; set; }
        public string overview { get; set; }
        public int id { get; set; }
        public string poster_path { get; set; }
        public int season_number { get; set; }
    }

    public class TMDB_CreatedBy
    {
        public int id { get; set; }
        public string credit_id { get; set; }
        public string name { get; set; }
        public int gender { get; set; }
        public string profile_path { get; set; }
    }

    public class TMDB_Genre
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class TMDB_LastEpisodeToAir
    {
        public string air_date { get; set; }
        public int episode_number { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string overview { get; set; }
        public string production_code { get; set; }
        public int season_number { get; set; }
        public string still_path { get; set; }
        public double vote_average { get; set; }
        public int vote_count { get; set; }
    }

    public class TMDB_NextEpisodeToAir
    {
        public string air_date { get; set; }
        public int episode_number { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string overview { get; set; }
        public string production_code { get; set; }
        public int season_number { get; set; }
        public object still_path { get; set; }
        public double vote_average { get; set; }
        public int vote_count { get; set; }
    }

    public class TMDB_Network
    {
        public string name { get; set; }
        public int id { get; set; }
        public string logo_path { get; set; }
        public string origin_country { get; set; }
    }

    public class TMDB_ProductionCompany
    {
        public int id { get; set; }
        public string logo_path { get; set; }
        public string name { get; set; }
        public string origin_country { get; set; }
    }

    public class TMDB_ProductionCountry
    {
        public string iso_3166_1 { get; set; }
        public string name { get; set; }
    }

    public class TMDB_Season
    {
        public string air_date { get; set; }
        public int episode_count { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string overview { get; set; }
        public string poster_path { get; set; }
        public int season_number { get; set; }
    }

    public class TMDB_SpokenLanguage
    {
        public string english_name { get; set; }
        public string iso_639_1 { get; set; }
        public string name { get; set; }
    }

    public class TMDB_ShowInfo
    {
        public string backdrop_path { get; set; }
        public List<TMDB_CreatedBy> created_by { get; set; }
        public List<int> episode_run_time { get; set; }
        public string first_air_date { get; set; }
        public List<TMDB_Genre> genres { get; set; }
        public string homepage { get; set; }
        public int id { get; set; }
        public bool in_production { get; set; }
        public List<string> languages { get; set; }
        public string last_air_date { get; set; }
        public TMDB_LastEpisodeToAir last_episode_to_air { get; set; }
        public string name { get; set; }
        public TMDB_NextEpisodeToAir next_episode_to_air { get; set; }
        public List<TMDB_Network> networks { get; set; }
        public int number_of_episodes { get; set; }
        public int number_of_seasons { get; set; }
        public List<string> origin_country { get; set; }
        public string original_language { get; set; }
        public string original_name { get; set; }
        public string overview { get; set; }
        public double popularity { get; set; }
        public string poster_path { get; set; }
        public List<TMDB_ProductionCompany> production_companies { get; set; }
        public List<TMDB_ProductionCountry> production_countries { get; set; }
        public List<TMDB_Season> seasons { get; set; }
        public List<TMDB_SpokenLanguage> spoken_languages { get; set; }
        public string status { get; set; }
        public string tagline { get; set; }
        public string type { get; set; }
        public double vote_average { get; set; }
        public int vote_count { get; set; }
    }


}
