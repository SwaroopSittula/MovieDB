using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDB.Models
{
    /// <summary>
    /// Error Response Standard that is followed for errors
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Content related to Error in ErrorMessage
        /// </summary>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// Status code corresponding to the error occurred
        /// </summary>
        public int StatusCode { get; set; }
    }
}
