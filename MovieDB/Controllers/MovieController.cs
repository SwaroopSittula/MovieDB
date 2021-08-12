using Microsoft.AspNetCore.Mvc;
using MovieDB.Helpers;
using MovieDB.Repository.Interfaces;
using Newtonsoft.Json;
using NSwag.Annotations;
using Serilog;
using StackExchange.Profiling;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MovieDB.Controllers
{
    [OpenApiTag("MovieDB", Description = "Get Movie | Movie Proxy")]
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieRepository MovieRepo;
        public MovieController(IMovieRepository movieRepo)
        {
            MovieRepo = movieRepo;
        }


        [HttpGet("{id}")]
        [Produces("application/json")]
        [ValidateModel]
        //1) checks if input is int or not      2) validate the annotations
        // movie/no-input   movie/.    movie/..   --> no response (they point to different end points than this may be)
        public Task<ActionResult> GetMovie([RegularExpression(@"^(\d+)$", ErrorMessage ="Bad Request!")] int id)
        {
            return  MovieRepo.GetMovie(id);
        }


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
