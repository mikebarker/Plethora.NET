using System;
using System.Data.SqlClient;

namespace Plethora.Data.SqlClient
{
    public class SqlExceptionTester : IExceptionTester
    {
        public static Random random = new Random();

        public bool TestForRetry(Exception exception, out TimeSpan waitTime)
        {
            waitTime = TimeSpan.Zero;

            SqlException sqlException = exception as SqlException;
            if (sqlException == null)
                return false;

            if (sqlException.Number == 1205)    // Dead-lock
            {
                waitTime = new TimeSpan(0, 0, random.Next(100, 200));
                return true;
            }

            if (sqlException.Number == 10054)   // Connection re-set
            {
                waitTime = TimeSpan.Zero;
                return true;
            }

            return false;
        }
    }
}
