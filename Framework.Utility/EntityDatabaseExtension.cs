using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;


namespace Framework.Utility
{
    public static class EntityDatabaseExtension
    {
        public static List<T> QueryStore<T>(this Database entitydb, string query, ref SqlParameter[] outValues, CommandType commandType = CommandType.Text, params SqlParameter[] sqlParameters) where T : new()
        {
            return QueryStoreBase<T>(entitydb, query, ref outValues, commandType, sqlParameters);
        }

        public static List<T> QueryStore<T>(this Database entitydb, string query, ref int totalRow, CommandType commandType = CommandType.Text, params SqlParameter[] sqlParameters) where T : new()
        {
            var outValues = new[] { new SqlParameter { ParameterName = "@TotalRow", Direction = ParameterDirection.ReturnValue, DbType = DbType.Int32 } };
            var data = QueryStoreBase<T>(entitydb, query, ref outValues, commandType, sqlParameters);
            totalRow = (int)outValues[0].Value;
            return data;
        }

        private static List<T> QueryStoreBase<T>(this Database entitydb, string query, ref SqlParameter[] outValues, CommandType commandType = CommandType.Text, params SqlParameter[] sqlParameters) where T : new()
        {
            List<T> result;
            var connection = entitydb.Connection;
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = query;
            command.CommandType = commandType;
            command.CommandTimeout = 300;
            if (sqlParameters != null && sqlParameters.Length > 0)
            {
                command.Parameters.AddRange(sqlParameters);
            }
            if (outValues != null && outValues.Length > 0)
            {
                command.Parameters.AddRange(outValues);
            }

            using (var reader = command.ExecuteReader())
            {
                result = reader.MapToList<T>();
            }
            command.Dispose();
            connection.Close();
            return result;
        }

        public static T QueryStoreReturnObject<T>(this Database entitydb, string query, CommandType commandType = CommandType.Text, params SqlParameter[] sqlParameters) where T : new()
        {
            T result;
            var connection = entitydb.Connection;
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = query;
            command.CommandType = commandType;
            if (sqlParameters != null && sqlParameters.Length > 0)
            {
                command.Parameters.AddRange(sqlParameters);
            }
            using (var reader = command.ExecuteReader())
            {
                result = reader.MapToObject<T>();
            }
            connection.Close();
            return result;
        }

        public static object QueryStoreWithScalar(this Database entitydb, string query, CommandType commandType = CommandType.Text, params SqlParameter[] sqlParameters)
        {
            var connection = entitydb.Connection;
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = query;
            command.CommandType = commandType;
            if (sqlParameters != null && sqlParameters.Length > 0)
            {
                command.Parameters.AddRange(sqlParameters);
            }
            var result = command.ExecuteScalar().ToString();
            connection.Close();
            return result;
        }

        public static object QueryStoreWithNonQuery(this Database entitydb, string query, CommandType commandType = CommandType.Text, params SqlParameter[] sqlParameters)
        {
            var connection = entitydb.Connection;
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = query;
            command.CommandType = commandType;
            if (sqlParameters != null && sqlParameters.Length > 0)
            {
                command.Parameters.AddRange(sqlParameters);
            }
            var result = command.ExecuteNonQuery();
            connection.Close();
            return result;
        }
    }
}
