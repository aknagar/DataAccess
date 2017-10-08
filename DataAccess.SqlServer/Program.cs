using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.SqlServer
{
    class Program
    {
        private static string connectionString = "Data Source=.;Initial Catalog=TestDb;Integrated Security=True";

        static void Main(string[] args)
        {
            // GetTop100Items();
            //GetTopItemId();
            // Update(60);
            ExecuteSqlScript();
            //ExecuteStoredProc();
        }

        public static List<ItemDao> GetTop100Items()
        {
            List<ItemDao> items = new List<ItemDao>(100);
            SqlResultMapper mapper = new SqlResultMapper(connectionString);
            Action<System.Data.IDataReader> map = r =>
            {
                var item = new ItemDao();

                item.ItemId = r.GetInt32(r.GetOrdinalOf("ItemID"));
                item.PartNumber = r.GetString(r.GetOrdinalOf("PartNumber"));
                item.ItemName = r.GetString(r.GetOrdinalOf("ItemName"));
                item.Description = r.GetString(r.GetOrdinalOf("Description"));

                items.Add(item);
            };

            mapper.MapQueryResultsAsync(new SqlQuery("Select top 100 * from dbo.Item"), map, () => items).Wait();
            return items;
        }

        public static int GetTopItemId()
        {
            var query = new SqlQuery("Select top 1 ItemId from Item");
            
            var result = SqlHelper.ExecuteScalar<int>(connectionString, query).Result;
            return result;
        }

        public static int Update(int itemId)
        {            
            string description = "Description";
            var query = new SqlQuery($"Update Item set Description = {description} where ItemId = {itemId}");

            var result = SqlHelper.ExecuteNonQuery(connectionString, query).Result;
            return result;
        }

        public static void ExecuteSqlScript()
        {
            var sqlQuery = new SqlQuery();
            sqlQuery.Query = SqlQuery.GetSqlScript("DataAccess.SqlServer.Scripts.MySqlScript.sql");
            var sqlParameter = new SqlParameter("@PartNumber", "FQC-10068");
            sqlQuery.Parameters.Add(sqlParameter);
            var result = SqlHelper.ExecuteScalar<string>(connectionString, sqlQuery).Result;
        }

        public static void ExecuteStoredProc()
        {
            List<ItemDao> items = new List<ItemDao>(100);
            var sqlQuery = new SqlQuery();
            sqlQuery.Query = "GetItem";
            sqlQuery.Type = CommandType.StoredProcedure;
            sqlQuery.Parameters.Add(new SqlParameter("@PartNumber", "FQC-10070"));
            SqlResultMapper mapper = new SqlResultMapper(connectionString);
            Action<System.Data.IDataReader> map = r =>
            {
                var item = new ItemDao();

                item.ItemId = r.GetInt32(r.GetOrdinalOf("ItemID"));
                item.PartNumber = r.GetString(r.GetOrdinalOf("PartNumber"));
                item.ItemName = r.GetString(r.GetOrdinalOf("ItemName"));
                item.Description = r.GetString(r.GetOrdinalOf("Description"));

                items.Add(item);
            };

            mapper.MapQueryResultsAsync(sqlQuery, map, () => items).Wait();
        }

    }
}
