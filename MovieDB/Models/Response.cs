
namespace MovieDB.Models
{
    /// <summary>
    /// Reponse from Repository
    /// </summary>
    public class ResponseDto
    {
        /// <summary>
        /// Reponse Message
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        /// Status code
        /// </summary>
        public int StatusCode { get; set; }

    }
}
