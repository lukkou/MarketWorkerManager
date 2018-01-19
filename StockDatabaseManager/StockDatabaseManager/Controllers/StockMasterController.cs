using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NPOI.SS.UserModel;

using StockDatabaseManager.Common;
using StockDatabaseManager.Utility;
using StockDatabaseManager.DataModels;

namespace StockDatabaseManager.Controllers
{
	class StockMasterController: BaseController
	{
		public void StockMasterRunner(string status)
		{
			try
			{
				ExcelDownload();

				List<TokyoStockExchangeExcelModel> lists = Logic.StockMaster.StockExcelToDataModel(GetExcelSaveDirectory() + Define.Stock.TokyoExchangeExcel);


			}
			catch(Exception e)
			{
				Log.Logger.Error(e.ToString());
				Console.WriteLine(e.Message);
				Console.ReadKey();
			}
		}

		public void ExcelDownload()
		{
			//ファイルの保存先はMyDocument以下のTokyoExchangeに年月フォルダを作り保存
			string saveDirectory = GetExcelSaveDirectory();

			if (!Directory.Exists(saveDirectory))
			{
				Directory.CreateDirectory(saveDirectory);
			}
			else
			{
				string[] files = Directory.GetFiles(saveDirectory);
				foreach (string file in files)
				{
					File.Delete(file);
				}
			}

			Task task =  Logic.StockMaster.DownloadTokyoExchangeExcelAsync(saveDirectory + Define.Stock.TokyoExchangeExcel);
			task.Wait();
		}

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
