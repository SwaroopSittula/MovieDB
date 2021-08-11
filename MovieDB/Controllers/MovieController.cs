using Microsoft.AspNetCore.Mvc;
using MovieDB.Helpers;
using MovieDB.Repository.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MovieDB.Controllers
{
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
    public IActionResult GetMovie([RegularExpression(@"^(\d+)$", ErrorMessage ="Bad Request!")] int id)
    {
        return MovieRepo.GetMovie(id);
    }

}
}
