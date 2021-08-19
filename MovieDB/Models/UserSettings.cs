
using MovieDB.Helpers;

namespace MovieDB.Models
{
    /// <summary>
    /// To store UserSettings
    /// </summary>
    public class UserSettings
    {
        /// <summary>
        /// Audig log settings are stored in this property
        /// </summary>
        public AuditLog AuditLog { get; set; }

        /// <summary>
        /// Stores base path for Miniprofiler
        /// </summary>
        public string  BasePath { get; set; }

        /// <summary>
        /// To switch Miniprofiler to on and off
        /// </summary>
        public bool EnableMiniProfiler { get; set; }
    }
}
