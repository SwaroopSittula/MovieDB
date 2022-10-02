using MovieDB.Models;
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
        Task<ResponseDto> GetMovie(int id);

        /// <summary>
        /// Health check endpoint abstraction
        /// </summary>
        /// <returns></returns>
        public Task<bool> IsAliveAsync();


        /// <summary>
        /// Loads all the data available in TMDB to the Cache
        /// </summary>
        public Task Cache();
    }
}
