using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Oracle.ManagedDataAccess.Client;

namespace Plethora.Data
{
    /// <summary>
    /// Extension class to add functionality to the <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> class.
    /// </summary>
    public static class DatabaseHelper
    {
        /// <summary>
        /// Adds a new In / Out <see cref="DbParameter"/> object to the given command.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="command">The command to add the parameter.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="dbType">One of the <see cref="DbType"/> values.</param>
        /// <param name="value">The value of the parameter.</param>
        public static void AddInOutParameter(this Database database, DbCommand command, string name, DbType dbType, object value)
        {
            database.AddInParameter(command, name, dbType, value);
            DbParameter parameter = command.Parameters["@" + name];
            parameter.Direction = ParameterDirection.InputOutput;
        }

        /// <summary>
        /// Adds a Table-Value <see cref="DbParameter"/> to the command.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="command">The command to add the parameter.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="dataTable">The value of the parameter.</param>
        public static void AddInTableVaueParameter(this Database database, DbCommand command, string name, DataTable dataTable)
        {
            if (command is SqlCommand)
            {
                var sqlCommand = (SqlCommand)command;

                AddInTableVaueParameter(sqlCommand, name, dataTable);
            }
            else if (command is OracleCommand)
            {
                var oracleCommand = (OracleCommand)command;

                AddInTableVaueParameter(oracleCommand, name, dataTable);
            }
        }

        private static void AddInTableVaueParameter(SqlCommand command, string name, DataTable dataTable)
        {
            var parameter = command.Parameters.AddWithValue(name, dataTable);
            parameter.Direction = ParameterDirection.Input;
            parameter.SqlDbType = SqlDbType.Structured;
        }

        public static void AddInTableVaueParameter(OracleCommand command, string name, DataTable dataTable)
        {
            //TODO:
            throw new NotImplementedException();
        }
    }
}
