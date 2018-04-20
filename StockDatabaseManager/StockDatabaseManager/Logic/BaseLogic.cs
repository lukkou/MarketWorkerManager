using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockDatabaseManager.Models;
using StockDatabaseManager.Context;

namespace StockDatabaseManager.Logic
{
	class BaseLogic
	{
		public DatabaseContext Db { get; set; }

		/// <summary>
		/// DatabaseとTableが存在しない場合新規に作成
		/// </summary>
		public void CreateDatacase()
		{
			bool createFlg = Db.Database.CreateIfNotExists();
			if (!createFlg)
			{
				Db.Database.Delete();

				Db.Database.Create();
			}
		}
	}
}
