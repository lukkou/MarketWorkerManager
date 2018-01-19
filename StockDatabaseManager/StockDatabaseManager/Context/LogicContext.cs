using System;
using System.Net;
using System.Net.Http;
using StockDatabaseManager.Logic;

namespace StockDatabaseManager.Context
{
	class LogicContext : IDisposable
	{
		/// <summary>
		/// データベースコンテキスト
		/// </summary>
		private DatabaseContext db { get; set; }

		private HttpClient client { get; set; }

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
			client = new HttpClient();

			//各ロジックを生成
			StockMaster = new StockMasterLogic { Db = this.db, Client = this.client};
			IndexData = new IndexCalendarLogic { Db = this.db, Client = this.client };
		}

		public void Dispose()
		{
			if(db != null)
			{
				db.Dispose();
			}

			if(client != null)
			{
				client.Dispose();
			}
		}
	}
}
