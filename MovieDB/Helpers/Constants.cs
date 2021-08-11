using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDB.Helpers
{
    public class Constants
    {
        public const string Json = "apllication/json";

        /// <summary>
        /// Audit Logger input parameters
        /// </summary>
        public const string GetMethod = "Get";
        public const string ApiFind = "Find from The MovieDB API";
        public const string Find = "Find from MovieCache in MongoDB";
        public const string Insert = "Insert into MovieCache in MongoDB";
        public const string GetMovie = "https://localhost:44377/movie/id";
    }
}
