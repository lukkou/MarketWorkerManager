using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockDatabaseManager.Common;
using StockDatabaseManager.Utility;
using StockDatabaseManager.DataModels;

namespace StockDatabaseManager.Controllers
{
	class StockMasterController : BaseController
	{
		/// <summary>
		/// 初回実行時の株マスターデータ作成
		/// </summary>
		public void AddFirstStockMasterData()
		{
			try
			{
				ExcelDownload();
				List<TokyoStockExchangeExcelModel> excelData = Logic.StockMaster.ExcelToDataModel(GetExcelSaveDirectory() + Define.Stock.TokyoExchangeExcel);

				Logic.StockMaster.BeginTransaction();
				var addTasks = new[]
				{
					Logic.StockMaster.AddStockMaster(excelData),
					Logic.StockMaster.AddIndustryCode17(excelData),
					Logic.StockMaster.AddIndustryCode33(excelData),
					Logic.StockMaster.AddClass(excelData)
				};

				Task.WaitAll(addTasks);
				Logic.StockMaster.Commit();
			}
			catch (Exception e)
			{
				Logic.StockMaster.Rollback();
				Log.Logger.Error(e.ToString());
				Console.WriteLine(e.Message);
				Console.ReadKey();
			}
		}

		/// <summary>
		/// 毎月/毎週/毎日処理の実行
		/// </summary>
		/// <param name="status"></param>
		public void StockMasterRunner(string status)
		{
			try
			{
				ExcelDownload();
				List<TokyoStockExchangeExcelModel> excelData = Logic.StockMaster.ExcelToDataModel(GetExcelSaveDirectory() + Define.Stock.TokyoExchangeExcel);

				var deffData = Logic.StockMaster.DeffStockMaster(excelData);

				Logic.StockMaster.BeginTransaction();

				Logic.StockMaster.Commit();
			}
			catch (Exception e)
			{
				Logic.StockMaster.Rollback();
				Log.Logger.Error(e.ToString());
				Console.WriteLine(e.Message);
				Console.ReadKey();
			}
		}

		/// <summary>
		/// 東証より当月の銘柄一覧のExcelを取得
		/// </summary>
		private void ExcelDownload()
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
		private void InitializingDirectory(string directory)
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
		private string GetExcelSaveDirectory()
		{
			string result = string.Empty;

			string myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string currentMonth = DateTime.Now.ToString("yyyyMM");

			//保存先を作成
			result = myDocuments + Define.Stock.TokyoExchangeDirectory + "\\" + currentMonth;
			return result;
		}
	}
}
