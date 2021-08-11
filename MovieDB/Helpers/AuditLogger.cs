


using MovieDB.Middleware;

namespace MovieDB.Helpers
{
    public class AuditLogger
    {
        public static void RequestInfo(string transactionId, string method, string path, string queryString, string payload)
        {
            AuditMiddleware.Logger.Information(
                string.Format("Request : TransactionId - {0}, Method - {1}, Path - {2}, QueryString - {3}, Payload - {4}",
                transactionId, method, path, queryString, payload
                ));
        }

        public static void ResponseInfo(string transactionId, string method, string path, string queryString, string databaseName, string collectionName, string payload)
        {
            AuditMiddleware.Logger.Information(
                string.Format("Request : TransactionId - {0}, Method - {1}, Path - {2}, QueryString - {3}, DatabaseName - {4}, CollectionName - {5}, Payload - {6}",
                transactionId, method, path, queryString, databaseName, collectionName, payload
                ));
        }
    }
}
