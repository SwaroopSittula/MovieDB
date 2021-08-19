namespace MovieDB.Helpers
{
    /// <summary>
    /// Settings related to Audit Log are injected to the class properties
    /// </summary>
    public class AuditLog
    { 
        /// <summary>
        /// Path Where audit logs are stored
        /// </summary>
        public string  Path { get; set; }

        /// <summary>
        /// Time to roll the logs in log file
        /// </summary>
        public string  RollingInterval { get; set; }
        /// <summary>
        /// true by default
        /// </summary>
        public bool Shared { get; set; }
        /// <summary>
        /// No of files to retain in the logs
        /// </summary>
        public int RetainedFileCountLimit { get; set; }

    }
}
