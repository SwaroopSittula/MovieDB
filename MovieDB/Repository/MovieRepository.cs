using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MovieDB.Helpers;
using MovieDB.Middleware;
using MovieDB.Models;
using MovieDB.Repository.Interfaces;
using Newtonsoft.Json;
using Serilog;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

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


        public async Task<ActionResult> GetMovie(int id)
        {
            //Checking Movie Cache in Mongo
            List<MovieInfo> result;
            using (MiniProfiler.Current.Step("Time taken to retrieve records from Movie Cache in MongoDB"))
            {
                result = MovieCache.Find(movie => movie.Id == id).ToList();
            }
                
            //List is Empty retrieve from API
            if (!result.Any()) 
            {
                //Response from API
                Task<HttpResponseMessage> response;
                string responseBody;
                using(MiniProfiler.Current.Step("Time taken to retrieve response from The MovieDB API"))
                {
                    using var client = HttpClientFactory.CreateClient("MovieDbAPI");
                    //using var response = client.GetAsync($"/{id}?api_key={ApiConfig.Value.RestApi.ApiKey}")
                    response = client.GetAsync(client.BaseAddress + ApiConfig.Value.RestApi.AppendApiKey(id));  //ApiConfig.Value.RestApi.RequestUrl(id)
                    responseBody = response.Result.Content.ReadAsStringAsync().Result;
                }
                
                //Success response
                if (response.Result.StatusCode == HttpStatusCode.OK)
                {
                    if (AuditMiddleware.Logger != null)
                    {
                        var transId = Guid.NewGuid().ToString();
                        AuditLogger.RequestInfo(transId, Constants.GetMethod, Constants.GetMovie, Constants.ApiFind, id.ToString());
                        AuditLogger.ResponseInfo(transId, Constants.GetMethod, Constants.GetMovie, Constants.Insert, DBSettings.Value.DatabaseName, DBSettings.Value.CollectionName, result.ToString());
                    }

                    var jsonResult = JsonConvert.DeserializeObject<MovieInfo>(responseBody);

                    //Update Movie Cache in MongoDB
                    var filter = Builders<MovieInfo>.Filter.Eq(movie => movie.Id, id);     
                    using(MiniProfiler.Current.Step("Time taken to Upsert record into Movie Cache in MongoDB"))
                    {
                        await MovieCache.ReplaceOneAsync(
                                    filter: filter,
                                    options: new ReplaceOptions { IsUpsert = true },
                                    replacement: jsonResult);
                        //MovieCache.InsertOneAsync(jsonResult)
                    }

                    return new ContentResult
                    {
                        Content = JsonConvert.SerializeObject(jsonResult), //getting string content for responseBody
                        ContentType = Constants.Json,
                        StatusCode = 200
                    };
                }
                else
                {
                    //Response = 401(invalid api key) , 404(resouce not found)
                    //var jsonResult = JsonConvert.DeserializeObject<FailureRepsonse>(responseBody)  //mapping API response to our FailureResponse
                    var errorResponse = new ErrorResponse();
                    int statusCode;

                    if (response.Result.StatusCode == HttpStatusCode.NotFound)
                    {
                        errorResponse.ErrorMessage = "Resource Not Found.";
                        errorResponse.StatusCode = 404; //NotFound
                        statusCode = 404;
                    }
                    else
                    {
                        errorResponse.ErrorMessage = "Invalid Api Key.";
                        errorResponse.StatusCode = 401; //Unauthorized
                        statusCode = 401;
                    }
                    return new ContentResult
                    {
                        Content = JsonConvert.SerializeObject(errorResponse),
                        ContentType = Constants.Json,
                        StatusCode = statusCode
                    };
                }
            }

 
            if (AuditMiddleware.Logger != null)
            {
                var transId = Guid.NewGuid().ToString();
                AuditLogger.RequestInfo(transId, Constants.GetMethod, Constants.GetMovie, Constants.Find, id.ToString());
                AuditLogger.ResponseInfo(transId, Constants.GetMethod, Constants.GetMovie, Constants.Find, DBSettings.Value.DatabaseName, DBSettings.Value.CollectionName, result.ToString());
            }

            return new ContentResult
            {
                Content = JsonConvert.SerializeObject(result[0]),
                ContentType = Constants.Json,
                StatusCode = 200
            };
        }


        #region IsAlive
        public async Task<bool> IsAliveAsync()
        {
            try
            {
                using (MiniProfiler.Current.Step(Constants.HealthCommand))
                {
                    await Task.Delay(1);
                    return true;
                }
            }
            catch (Exception ex)
            {

                Log.Error(ex.Message);
            }
            return false;
        }
        #endregion
    }
}
