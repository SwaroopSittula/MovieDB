using Newtonsoft.Json;
using System.Collections.Generic;


namespace MovieDB.Models
{
    public class MovieInfo
    {
        [JsonProperty("adult")]
        public bool Adult { get; set; }

        [JsonProperty("backdrop_path")]
        public string BackdropPath { get; set; }

        [JsonProperty("belongs_to_collection")]
        public BelongsToCollection BelongsToCollection { get; set; } //mostly null.......check movie id = 11,21

        [JsonProperty("budget")] 
        public int Budget { get; set; }

        [JsonProperty("genres")] 
        public Genres[] Genres { get; set; }

        [JsonProperty("homepage")] 
        public string Homepage { get; set; }

        [JsonProperty("id")] 
        public int Id { get; set; } //2-957,  0,1,10 - no resources

        [JsonProperty("imdb_id")] 
        public string ImdbId { get; set; }  //minLength = 9, maxLength = 9 , pattern = ^tt[0-9]{7}

        [JsonProperty("original_language")] 
        public string OriginalLanguage { get; set; }

        [JsonProperty("original_title")] 
        public string OriginalTitle { get; set; }

        [JsonProperty("overview")] 
        public string Overview { get; set; }

        [JsonProperty("popularity")] 
        public double Popularity { get; set; } //number

        [JsonProperty("poster_path")] 
        public string PosterPath { get; set; }

        [JsonProperty("production_companies")] 
        public List<ProductionCompanies> ProductionCompanies { get; set; } //array[object]

        [JsonProperty("production_countries")] 
        public List<ProductionCountries> ProductionCountries { get; set; }

        [JsonProperty("release_date")] 
        public string ReleaseDate { get; set; } //format = date

        [JsonProperty("revenue")] 
        public int Revenue { get; set; } //long?

        [JsonProperty("runtime")] 
        public int? Runtime { get; set; }

        [JsonProperty("spoken_languages")]
        public List<SpokenLanguages> SpokenLanguages { get; set; }

        [JsonProperty("status")] 
        public string Status { get; set; } //allowed values = Rumored, Planned, In Production, Post Production, Released, Canceled

        [JsonProperty("tagline")] 
        public string Tagline { get; set; }

        [JsonProperty("title")] 
        public string Title { get; set; }

        [JsonProperty("video")] 
        public bool Video { get; set; }

        [JsonProperty("vote_average")]
        public double VoteAverage { get; set; } //number

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