using System;
using StockDatabaseManager.Models;
using StockDatabaseManager.Logic;

namespace StockDatabaseManager.Context
{
	class LogicContext : IDisposable
	{
		/// <summary>
		/// データベースコンテキスト
		/// </summary>
		private DatabaseContext db { get; set; }

		/// <summary>
		/// 株マスターロジック
		/// </summary>
		public StockMasterLogic StockMaster { get; private set; }

		/// <summary>
		/// 経済指標ロジック
		/// </summary>
		public IndexCalendarLogic IndexData { get; private set; }



		public LogicContext()
		{
			db = new DatabaseContext();

			//各ロジックを生成
			StockMaster = new StockMasterLogic (db);
			IndexData = new IndexCalendarLogic(db);
		}

		public void Dispose()
		{
			if(db != null)
			{
				db.Dispose();
			}
		}
	}
}
