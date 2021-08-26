using Microsoft.AspNetCore.Mvc;
using MovieDB.Helpers;
using MovieDB.Models;
using MovieDB.Repository.Interfaces;
using Newtonsoft.Json;
using NSwag.Annotations;
using Serilog;
using StackExchange.Profiling;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MovieDB.Controllers
{
    /// <summary>
    /// Movie Controller class to route the http endpoint request to the Controller class Methods
    /// </summary>
    [OpenApiTag("MovieDB", Description = "Get Movie | Movie Proxy")]
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        /// <summary>
        /// Class property to store the instance of MovieRepository using Constructor Dependency Injection
        /// </summary>
        private readonly IMovieRepository MovieRepo;
        public MovieController(IMovieRepository movieRepo)
        {
            MovieRepo = movieRepo;
        }


        /// <summary>
        /// This is Get Method to retrieve Movie Info given a MovieId.
        /// 
        /// Here ValidateModel Filter does the following
        /// 1) checks if input is int or not
        /// 2) validate the annotations
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ValidateModel]
        public async Task<IActionResult> GetMovie([RegularExpression(@"^(\d+)$", ErrorMessage ="Bad Request!")] int id)
        {
            ResponseDto reponse = await MovieRepo.GetMovie(id);
            return new ContentResult()
            {
                Content = reponse.Response,
                ContentType = Constants.Json,
                StatusCode = reponse.StatusCode
            };
        }


        /// <summary>
        /// Health Check Endpoint
        /// </summary>
        /// <returns></returns>
        #region IsAlive
        [ApiVersion("1.0")]
        [HttpGet("health", Name = "IsAlive")]
        public async Task<IActionResult> IsAliveAsync()
        {
            var health = await MovieRepo.IsAliveAsync();

            using (MiniProfiler.Current.Step(Constants.Health))
            {
                Log.Information(Constants.Health);
                return Content(JsonConvert.SerializeObject(health));
            }
        }
        #endregion

    }
}
