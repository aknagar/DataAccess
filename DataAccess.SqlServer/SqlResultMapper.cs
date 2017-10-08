using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.SqlServer
{
    class SqlResultMapper
    {
        public Dictionary<int, string> ColumnNames;
        public readonly string ConnectionString;
        
        public SqlResultMapper(string connectionString)
        {
            this.ConnectionString = connectionString;
            this.ColumnNames = new Dictionary<int, string>();
        }

        public int MapColumn(string columnName)
        {
            var index = this.ColumnNames.Count;
            this.ColumnNames.Add(index,columnName);
            return index;
        }

        public Task<T> MapQueryResultsAsync<T>(SqlQuery query, Action<IDataReader> map, Func<T> reduce)
        {
            return Task.Factory.StartNew(() => this.MapQueryResults(query, map, reduce));
        }

        public T MapQueryResults<T>(SqlQuery query, Action<IDataReader> map, Func<T> reduce)
        {
            this.ExecuteQuery(query, map);
            return reduce();
        }

        public Task MapQueryResultsAsync(SqlQuery query, Action<IDataReader> map)
        {
            return Task.Factory.StartNew(() => this.ExecuteQuery(query, map));
        }

        public void ExecuteQuery(SqlQuery query, Action<IDataReader> map)
        {
            using (var cn = SqlHelper.GetOpenConnection(this.ConnectionString))
            using (var cmd = query.CreateCommand(cn))
            using (var r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    map(r);
                }
            }
        }
    }
}
