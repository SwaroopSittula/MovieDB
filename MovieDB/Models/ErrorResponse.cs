using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDB.Models
{
    public class ErrorResponse
    {
        public string ErrorMessage { get; set; }
        public int StatusCode { get; set; }
    }
}
