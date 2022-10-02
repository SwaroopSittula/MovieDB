using Newtonsoft.Json;
using System.Collections.Generic;


namespace MovieDB.Models
{
    /// <summary>
    /// Model class is created based on success result available from The MovieDB(TMDB) API.
    /// This Model class has all the Info available on a movie.
    /// </summary>
    public class MovieInfo
    {
        [JsonProperty("adult")]
        public bool Adult { get; set; }

        [JsonProperty("backdrop_path")]
        public string BackdropPath { get; set; }

        [JsonProperty("belongs_to_collection")]
        public BelongsToCollection BelongsToCollection { get; set; } //mostly null.......check movie id = 11,21

        [JsonProperty("budget")] 
        public long Budget { get; set; }

        [JsonProperty("genres")] 
        public Genres[] Genres { get; set; }

        [JsonProperty("homepage")] 
        public string Homepage { get; set; }

        /// <summary>
        ///  0,1,10 -some no resources id's
        ///  Id goes above 30k start from positive integer
        /// </summary>
        [JsonProperty("id")] 
        public int Id { get; set; }


        /// <summary>
        /// Properties given in TMDB API
        /// minLength = 9, maxLength = 9 , pattern = ^tt[0-9]{7}
        /// </summary>
        [JsonProperty("imdb_id")] 
        public string ImdbId { get; set; }  

        [JsonProperty("original_language")] 
        public string OriginalLanguage { get; set; }

        [JsonProperty("original_title")] 
        public string OriginalTitle { get; set; }

        [JsonProperty("overview")] 
        public string Overview { get; set; }

        /// <summary>
        /// Datatype = number
        /// </summary>
        [JsonProperty("popularity")] 
        public double Popularity { get; set; } 

        [JsonProperty("poster_path")] 
        public string PosterPath { get; set; }

        /// <summary>
        /// array[object]
        /// </summary>
        [JsonProperty("production_companies")] 
        public List<ProductionCompanies> ProductionCompanies { get; set; } 

        [JsonProperty("production_countries")] 
        public List<ProductionCountries> ProductionCountries { get; set; }

        /// <summary>
        /// format = date
        /// </summary>
        [JsonProperty("release_date")] 
        public string ReleaseDate { get; set; }

        /// <summary>
        /// long?
        /// yes for id=19995 value exceeds capacity of int
        /// </summary>
        [JsonProperty("revenue")] 
        public long Revenue { get; set; } 

        [JsonProperty("runtime")] 
        public int? Runtime { get; set; }

        [JsonProperty("spoken_languages")]
        public List<SpokenLanguages> SpokenLanguages { get; set; }

        /// <summary>
        /// allowed values = Rumored, Planned, In Production, Post Production, Released, Canceled
        /// </summary>
        [JsonProperty("status")] 
        public string Status { get; set; } 

        [JsonProperty("tagline")] 
        public string Tagline { get; set; }

        [JsonProperty("title")] 
        public string Title { get; set; }

        [JsonProperty("video")] 
        public bool Video { get; set; }

        /// <summary>
        /// Datatype = number
        /// </summary>
        [JsonProperty("vote_average")]
        public double VoteAverage { get; set; } 

        [JsonProperty("vote_count")] 
        public int VoteCount { get; set; }
    }

    public class BelongsToCollection
    {
        [JsonProperty("id")]    
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }

        [JsonProperty("backdrop_path")]
        public string BackdropPath { get; set; }
    }

    public class Genres
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class ProductionCompanies
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("logo_path")]
        public string LogoPath { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("origin_country")]
        public string OriginCountry { get; set; }
    }

    public class ProductionCountries
    {
        [JsonProperty("iso_3166_1")]
        public string ISO_3166_1 { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class SpokenLanguages
    {
        [JsonProperty("english_name")]
        public string EnglishName { get; set; }

        [JsonProperty("iso_639_1")]
        public string ISO_639_1 { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }


}