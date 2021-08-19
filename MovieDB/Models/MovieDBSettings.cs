namespace MovieDB.Models
{
    public class MovieDBSettings
    {
        /// <summary>
        /// ApiDetails type variable is used to store the online TMDB properties like BaseURL and ApiKey
        /// </summary>
        public ApiDetails RestApi { get; set; }
    }

    public class ApiDetails
    {
        /// <summary>
        /// Base Url for Get request of Movie Info based on Movie Id provided
        /// </summary>
        public string BaseUrl { get; set; }
        /// <summary>
        /// Api Key reqired to access the info from TMDB is stored in this property
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// BaseUrl ans ApiKey are applied from application.development.json
        /// Request Url is formed when requested.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string RequestUrl(int id)
        {
            return $"{BaseUrl}/{id}?api_key={ApiKey}";
            //return BaseUrl + "/" + id + "?api_key=" + ApiKey
        }

        /// <summary>
        /// Here get BaseURI from Startup.cs, so we only append id and api_key to the BaseAddress.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string AppendApiKey(int id)
        {
            return $"/{id}?api_key={ApiKey}";
        }
    }
}
