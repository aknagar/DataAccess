using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.SqlServer
{
    public static class SqlHelper
    {
        public static DbConnection GetOpenConnection(string connectionString)
        {
            DbConnection cn = new SqlConnection(connectionString);
            
            var success = false;
            try
            {
                cn.Open();
                success = true;
                return cn;
            }
            finally
            {
                if (!success)
                {
                    cn.Dispose();
                }
            }
        }

        public static async Task<DbConnection> GetOpenConnectionAsync(string connectionString)
        {
            DbConnection cn = new SqlConnection(connectionString);
            
            var success = false;
            try
            {
                await cn.OpenAsync();
                success = true;
                return cn;
            }
            finally
            {
                if (!success)
                {
                    cn.Dispose();
                }
            }
        }
        
        public static async Task<int> ExecuteNonQuery(string connectionString, SqlQuery query)
        {
            int result = 0;
            using (var cn = SqlHelper.GetOpenConnection(connectionString))
            using (var cmd = query.CreateCommand(cn))
            {              
                result = await cmd.ExecuteNonQueryAsync();                
            }

            return result;
        }

        public static async Task<T> ExecuteScalar<T>(string connectionString, SqlQuery query) 
        {
            T result = default(T);
            using (var cn = SqlHelper.GetOpenConnection(connectionString))
            using (var cmd = query.CreateCommand(cn))
            {
                result = (T)await cmd.ExecuteScalarAsync();                
            }

            return result;
        }
        
        public static async Task ExecuteQuery<T>(string connnectionString, SqlQuery query, Action<IDataReader> map)
        {
            using (var cn = SqlHelper.GetOpenConnection(connnectionString))
            using (var cmd = query.CreateCommand(cn))
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (reader.Read())
                {
                    map(reader);
                }
            }
        }

        public static int GetOrdinalOf(this IDataReader reader, string columnName)
        {
            int columNumber = reader.GetOrdinal(columnName);
            if (reader.IsDBNull(columNumber))
            {
                throw new ApplicationException(string.Format($"No column exists with name {0}", columnName));
            }

            return columNumber;
        }       
        
    }
}
