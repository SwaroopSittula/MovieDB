using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MovieDB.Helpers;
using MovieDB.Middleware;
using MovieDB.Models;
using MovieDB.Repository.Interfaces;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace MovieDB.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly IHttpClientFactory HttpClientFactory;
        private readonly IOptions<MovieDBSettings> ApiConfig;

        //Mongo cache
        private readonly IMongoCollection<MovieInfo> MovieCache;
        private readonly IOptions<DatabaseSettings> DBSettings;

        public MovieRepository(IHttpClientFactory httpClientFactory, IOptions<MovieDBSettings> apiConfig, IOptions<DatabaseSettings> dbSettings)
        {
            HttpClientFactory = httpClientFactory;
            ApiConfig = apiConfig;

            //MongoCache
            DBSettings = dbSettings;
            var connection = new MongoClient(DBSettings.Value.ConnectionString);
            var database = connection.GetDatabase(DBSettings.Value.DatabaseName);
            MovieCache = database.GetCollection<MovieInfo>(DBSettings.Value.CollectionName);
        }


        public ActionResult GetMovie(int id)
        {
            var result = MovieCache.Find(movie => movie.Id == id).ToList();

            //List is Empty retrieve from API
            if (!result.Any()) 
            {
                using var client = HttpClientFactory.CreateClient("MovieDbAPI");
                using var response = client.GetAsync(client.BaseAddress + ApiConfig.Value.RestApi.AppendApiKey(id));  //ApiConfig.Value.RestApi.RequestUrl(id)
                var responseBody = response.Result.Content.ReadAsStringAsync().Result;

                if (response.Result.StatusCode == HttpStatusCode.OK)
                {
                    //error without {get;set;} in MovieInfo
                    var jsonResult = JsonConvert.DeserializeObject<MovieInfo>(responseBody);


                    if (AuditMiddleware.Logger != null)
                    {
                        var transId = Guid.NewGuid().ToString();
                        AuditLogger.RequestInfo(transId, Constants.GetMethod, Constants.GetMovie, Constants.ApiFind, id.ToString());
                        AuditLogger.ResponseInfo(transId, Constants.GetMethod, Constants.GetMovie, Constants.Insert, DBSettings.Value.DatabaseName, DBSettings.Value.CollectionName, result.ToString());
                    }
                    //Update into Mongo Cache (try using upsert)
                    MovieCache.InsertOneAsync(jsonResult);

                    return new JsonResult(new SuccessResponse() 
                    {
                        Source = "MovieDB API.",
                        ContentType = Constants.Json,
                        StatusCode = 200,
                        Content = jsonResult
                        
                    });
                }
                else
                {
                    //Response = 401(invalid api key) , 404(resouce not found)
                    var jsonResult = JsonConvert.DeserializeObject<FailureRepsonse>(responseBody);
                    var errorResponse = new ErrorResponse();

                    if(response.Result.StatusCode == HttpStatusCode.NotFound)
                    {
                        errorResponse.ErrorMessage = "Resource Not Found.";
                        errorResponse.StatusCode = 404; //NotFound
                    }
                    else
                    {
                        errorResponse.ErrorMessage = "Invalid Api Key.";
                        errorResponse.StatusCode = 401; //Unauthorized
                    }

                    return new JsonResult(errorResponse);

                }
            }

 
            if (AuditMiddleware.Logger != null)
            {
                var transId = Guid.NewGuid().ToString();
                AuditLogger.RequestInfo(transId, Constants.GetMethod, Constants.GetMovie, Constants.Find, id.ToString());
                AuditLogger.ResponseInfo(transId, Constants.GetMethod, Constants.GetMovie, Constants.Find, DBSettings.Value.DatabaseName, DBSettings.Value.CollectionName, result.ToString());
            }

            return new JsonResult(new SuccessResponse()
            {
                Source = "Movie Cache in MongoDB.",
                ContentType = Constants.Json,
                StatusCode = 200,
                Content = result[0]
            });
        }
    }
}
