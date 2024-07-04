using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace Library
{
    internal static class DbConnection
    {
        private static SqlConnection dbConnection;
        public static SqlConnection get() {
            if (dbConnection != null) {
                return dbConnection;
            }

            string conn = "Data Source=localhost;Integrated Security=SSPI;Initial Catalog=;";
            dbConnection = new SqlConnection(conn);

            return dbConnection;
        }
    }
}
