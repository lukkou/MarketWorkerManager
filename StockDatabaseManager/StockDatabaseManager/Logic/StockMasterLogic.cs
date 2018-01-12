using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockDatabaseManager.Context;

namespace StockDatabaseManager.Logic
{
	class StockMasterLogic
	{
		private DatabaseContext db { get; set; }

		public StockMasterLogic(DatabaseContext context)
		{
			db = context;
		}
	}
}
