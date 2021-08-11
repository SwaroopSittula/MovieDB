

namespace MovieDB.Helpers
{
    public class AuditLog
    {
        #region Local

        public string  Path { get; set; }

        public string  RollingInterval { get; set; }
        public bool Shared { get; set; }
        public int RetainedFileCountLimit { get; set; }

        #endregion
    }
}
