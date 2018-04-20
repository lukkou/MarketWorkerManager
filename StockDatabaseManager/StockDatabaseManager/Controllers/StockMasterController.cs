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

			}
			catch (Exception e)
			{
				Log.Logger.Error(e.ToString());
				Console.WriteLine(e.Message);
				Console.ReadKey();
			}
		}

	}
}
