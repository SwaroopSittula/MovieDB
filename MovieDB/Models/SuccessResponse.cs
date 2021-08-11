using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDB.Models
{
    public class SuccessResponse
    {
        public string Source { get; set; }
        public object Content { get; set; }
        public string ContentType { get; set; }
        public int StatusCode { get; set; }
    }
}
