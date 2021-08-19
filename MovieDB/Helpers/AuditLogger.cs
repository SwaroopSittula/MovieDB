


using MovieDB.Middleware;

namespace MovieDB.Helpers
{
    public static class AuditLogger
    {
        /// <summary>
        /// This Method is used to "log the request info" whose respose typically have PII (Personally Identifiable Information)
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="method"></param>
        /// <param name="path"></param>
        /// <param name="queryString"></param>
        /// <param name="payload"></param>
        public static void RequestInfo(string transactionId, string method, string path, string queryString, string payload)
        {
            AuditMiddleware.Logger.Information(
                string.Format("Request : TransactionId - {0}, Method - {1}, Path - {2}, QueryString - {3}, Payload - {4}",
                transactionId, method, path, queryString, payload
                ));
        }

        /// <summary>
        /// To log the PII(Personally Identifiable Information) resposne info
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="method"></param>
        /// <param name="path"></param>
        /// <param name="queryString"></param>
        /// <param name="databaseName"></param>
        /// <param name="collectionName"></param>
        /// <param name="payload"></param>
        public static void ResponseInfo(string transactionId, string method, string path, string queryString, string databaseName, string collectionName, string payload)
        {
            AuditMiddleware.Logger.Information(
                string.Format("Request : TransactionId - {0}, Method - {1}, Path - {2}, QueryString - {3}, DatabaseName - {4}, CollectionName - {5}, Payload - {6}",
                transactionId, method, path, queryString, databaseName, collectionName, payload
                ));
        }
    }
}
