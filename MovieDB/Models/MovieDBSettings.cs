namespace MovieDB.Models
{
    public class MovieDBSettings
    {
        public ApiDetails RestApi { get; set; }
    }

    public class ApiDetails
    {
        public string BaseUrl { get; set; }
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
            //return BaseUrl + "/" + id + "?api_key=" + ApiKey;
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
