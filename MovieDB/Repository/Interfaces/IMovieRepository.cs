using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDB.Repository.Interfaces
{
    public interface IMovieRepository
    {
        ActionResult GetMovie(int id);
    }
}
