namespace MovieDB.Models
{
    /// <summary>
    /// This response is direct mapping to Error Responses from The MovieDB API
    /// </summary>
    public class FailureRepsonse
    {
        public bool success { get; set; }
        public int status_code { get; set; }
        public string status_message { get; set; }
    }
}
