using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDB.Repository.Interfaces
{
    public interface IMovieRepository
    {
        /// <summary>
        /// This method is to retrieve Movie Info based on Id.
        /// This method is to retrieve Movie Info based on Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ActionResult> GetMovie(int id);

        /// <summary>
        /// Health check endpoint abstraction
        /// </summary>
        /// <returns></returns>
        public Task<bool> IsAliveAsync();
    }
}
