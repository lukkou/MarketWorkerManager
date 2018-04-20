using System;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using StockDatabaseManager.Common;
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
		/// データベースコンテキストのトランザクション
		/// </summary>
		private DbContextTransaction tran { get; set; }

		/// <summary>
		/// HTTPクライアントオブジェクト
		/// ※usingを使用した場合、using毎回ソケットがオープンされ
		/// クローズしてもTIME_WAIT後ちょっとの間解放されないので
		/// 一つのオブジェクトを使いまわしLogicContextが解放された
		/// タイミングでクローズする。
		/// </summary>
		private HttpClient client { get; set; }
		
		/// <summary>
		/// ベースロジック
		/// </summary>
		public BaseLogic Base { get; private set; }

		/// <summary>
		/// 株マスターロジック
		/// </summary>
		public StockMasterLogic StockMaster { get; private set; }

		/// <summary>
		/// 経済指標ロジック
		/// </summary>
		public IndexCalendarLogic IndexData { get; private set; }

		/// <summary>
		/// LogicContext
		/// </summary>
		public LogicContext()
		{
			db = new DatabaseContext();
			db.Database.Log = AppendLog;
			client = new HttpClient();

			//各ロジックを生成
			Base = new BaseLogic { Db = db };
			StockMaster = new StockMasterLogic { Db = this.db, Client = this.client};
			IndexData = new IndexCalendarLogic { Db = this.db, Client = this.client };
		}

		/// <summary>
		/// EntityFrameworkのトランザクション開始
		/// </summary>
		public void BeginTransaction()
		{
			tran = db.Database.BeginTransaction();
		}

		/// <summary>
		/// EntityFrameworkのコミット
		/// </summary>
		public void Commit()
		{
			if (tran != null)
			{
				tran.Commit();
				tran.Dispose();
			}
		}

		/// <summary>
		/// EntityFrameworkのロールバック
		/// </summary>
		public void Rollback()
		{
			if (tran != null)
			{
				tran.Rollback();
				tran.Dispose();
			}
		}

		/// <summary>
		/// Dispose
		/// </summary>
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

		/// <summary>
		/// EntityFrameworkログ出力オブジェクト
		/// </summary>
		/// <param name="msg"></param>
		private void AppendLog(string msg)
		{
			if (string.IsNullOrWhiteSpace(msg))
			{
				return;
			}

			string logMsg = msg.TrimEnd(new char[]{'\r','\n' });
			Log.SqlLogger.Info("\n" + logMsg + "\n");
		}
	}
}
