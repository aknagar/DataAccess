using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.SqlServer
{
    class ItemDao
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }

        public string PartNumber { get; set; }

        public string Description { get; set; }


        public void MapAll(IDataReader r)
        {
            this.ItemId = r.GetInt32(r.GetOrdinalOf("ItemID"));
            this.PartNumber = r.GetString(r.GetOrdinalOf("PartNumber"));
            this.ItemName = r.GetString(r.GetOrdinalOf("ItemName"));
            this.Description = r.GetString(r.GetOrdinalOf("Description"));            
        }
    }
}
