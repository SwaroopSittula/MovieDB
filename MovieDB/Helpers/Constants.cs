using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDB.Helpers
{
    public static class Constants
    {
        public const string Json = "application/json";


        #region AudiLoggingConstants

        /// <summary>
        /// Audit Logger input request & response parameters
        /// GetMethod is type of Http Endpoint request (Get/Post/Put/Delete)
        /// </summary>
        public const string GetMethod = "Get";
        /// <summary>
        /// Request Query is to Fing Movie based on MovieID
        /// </summary>
        public const string FindMovie = "Find Movie for the provided Id";
        /// <summary>
        /// The query parameter for response is Find Movie from the online API source
        /// And Insert into MovieCache in Mongo DB
        /// </summary>
        public const string ApiFind = "Find(Get) from The Movie DB(TMDB) Api online and Insert into MovieCache in MongoDB";
        /// <summary>
        /// Requested Movie info response is retrieved from Movie cahce in MongoDB
        /// </summary>
        public const string Find = "Find from MovieCache in MongoDB";
        /// <summary>
        /// End Point for the Get request of Movie info
        /// </summary>
        public const string GetMovie = "https://localhost:port/movie/id";

        #endregion


        /// <summary>
        /// Health Constants
        /// </summary>
        public const string Health = "Health";
        public const string HealthCommand = "HealthCommand";


        /// <summary>
        /// Testing ValidateAttribute Filter
        /// </summary>
        public const string ID_Required = "Id is required";
    }
}
