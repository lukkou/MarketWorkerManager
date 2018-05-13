using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockDatabaseManager.Common;
using StockDatabaseManager.Context;

namespace StockDatabaseManager.Controllers
{

	class BaseController : IDisposable
	{
		public LogicContext Logic { get; private set; }

		public BaseController()
		{
			Logic = new LogicContext();
		}

		/// <summary>
		/// データベースとテーブルの新規作成
		/// </summary>
		internal void CreateDataBase()
		{
			Logic.Base.CreateDatacase();
		}

		/// <summary>
		/// 東証より当月の銘柄一覧のExcelを取得
		/// </summary>
		internal void ExcelDownload()
		{
			//ファイルの保存先はMyDocument以下のTokyoExchangeに年月フォルダを作り保存
			InitializingDirectory(GetExcelSaveDirectory());
			Task downloadTask = Logic.StockMaster.DownloadTokyoExchangeExcelAsync(GetExcelSaveDirectory() + Define.Stock.TokyoExchangeExcel);
			downloadTask.Wait();
		}

		/// <summary>
		/// Excelファイル保存先の初期化
		/// </summary>
		/// <param name="pash"></param>
		internal void InitializingDirectory(string directory)
		{
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}
			else
			{
				string[] files = Directory.GetFiles(directory);
				foreach (string file in files)
				{
					File.Delete(file);
				}
			}
		}

		/// <summary>
		/// 東証Excelの保存先ディレクトリを取得
		/// </summary>
		/// <returns></returns>
		internal string GetExcelSaveDirectory()
		{
			string result = string.Empty;

			string myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string currentMonth = DateTime.Now.ToString("yyyyMM");

			//保存先を作成
			result = myDocuments + Define.Stock.TokyoExchangeDirectory + "\\" + currentMonth;
			return result;
		}

		public void Dispose()
		{
			Logic.Dispose();
		}
	}
}
