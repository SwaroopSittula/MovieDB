
using MovieDB.Helpers;

namespace MovieDB.Models
{
    public class UserSettings
    {
        public AuditLog AuditLog { get; set; }
        public string  BasePath { get; set; }

        public bool EnableMiniProfiler { get; set; }
    }
}
