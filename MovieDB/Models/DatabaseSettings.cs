
namespace MovieDB.Models
{
    /// <summary>
    /// Class used to store Database settings
    /// </summary>
    public class DatabaseSettings
    {
        /// <summary>
        /// Collection Name that is used by this API
        /// </summary>
        public string CollectionName { get; set; }

        /// <summary>
        /// Database name that is connected is stored in this property
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// Connection string used to connect to the Database
        /// </summary>
        public string ConnectionString { get; set; }
    }
}
