using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace StockDatabaseManager
{
	class StockDatabaseContext : DbContext
	{
		public DbSet<StockMaster> StockMasters { get; set; }
	}
}
