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
    public class SqlQuery
    {
        public SqlQuery(string query): this()
        {
            this.Query = query;
        }
        public SqlQuery()
        {
            Type = CommandType.Text;
            Parameters = new List<SqlParameter>();
        }

        public string Query { get; set; }

        public CommandType Type { get; set; }

        public List<SqlParameter> Parameters { get; set; }

        public int CommandTimeOut { get; set; }
        
        public DbCommand CreateCommand(DbConnection connection)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandTimeout = CommandTimeOut;
            cmd.CommandText = Query;
            cmd.CommandType = Type;
            foreach (var param in Parameters)
            {
                cmd.Parameters.Add(param);
            }
            return cmd;
        }

        public static string GetSqlScript(string scriptName)
        {
            using (var str = GetManifestResourceStream(scriptName))
            {
                using (var r = new StreamReader(str))
                {
                    return r.ReadToEnd();
                }
            }
        }

        private static Stream GetManifestResourceStream(string embeddedResource)
        {
            var str = Assembly.GetExecutingAssembly().GetManifestResourceStream(embeddedResource);

            if (str == null)
            {
                throw new InvalidOperationException(
                    string.Format("The embedded resource {0} was not found.", embeddedResource));
            }

            return str;
        }
    }
}
