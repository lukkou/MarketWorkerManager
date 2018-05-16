using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MarketWorkerManager.Common;
using MarketWorkerManager.Models;
using MarketWorkerManager.DataModels;

namespace MarketWorkerManager.Controllers
{
	class FirstTimeController : BaseController
	{
		/// <summary>
		/// エントリーポイント
		/// </summary>
		public void Run()
		{
			try
			{
				CreateDataBase();
				AddFirstIndexData();
				AddFirstStockMasterData();
			}
			catch (Exception e)
			{
				Logic.Rollback();
				Log.Logger.Error(e.Message);
				Log.Logger.Error(e.StackTrace);
				if (e.InnerException != null)
				{
					Log.Logger.Error(e.InnerException.Message);
					Log.Logger.Error(e.InnerException.StackTrace);
				}
				Console.WriteLine(e.Message);
				Console.ReadKey();
			}
		}

		/// <summary>
		/// 初回実行時に当月から一年前までの指標データの作成
		/// </summary>
		private void AddFirstIndexData()
		{
			List<IndexCalendar> yearList = new List<IndexCalendar>();
			//一年前から当月までの指標データを取得
			for (int i = 0; i <= 12; i++)
			{
				string monthStart = DateTime.Now.AddMonths(i * -1).ToString("yyyy-MM") + "-01";
				string monthEnd = DateTime.Parse(DateTime.Now.AddMonths(i * -1).AddMonths(1).ToString("yyyy-MM") + "-01 00:00:00").AddDays(-1).ToString("yyyy-MM-dd");

				var task = Logic.IndexData.GetMql5JsonAsync(monthStart, monthEnd);
				task.Wait();

				List<IndexCalendar> jsonData = Logic.IndexData.ResponseBodyToEntityModel(task.Result);
				List<IndexCalendar> indexData = Logic.IndexData.GetSpecifiedRangeIndex(jsonData, monthStart, monthEnd);
				yearList.AddRange(indexData);
			}

			Logic.IndexData.RegisteredIndexData(yearList, true);
		}

		/// <summary>
		/// 初回実行時の株マスターデータ作成
		/// </summary>
		private void AddFirstStockMasterData()
		{
			ExcelDownload();
			List<TokyoStockExchangeExcelModel> excelData = Logic.StockMaster.ExcelToDataModel(GetExcelSaveDirectory() + Define.Stock.TokyoExchangeExcel);
			List<MarketMaster> marketList = Logic.StockMaster.ExcelModelToMarketModel(excelData);

			Logic.BeginTransaction();

			var addTasks = new[]
			{
					Logic.StockMaster.AddMarketMaster(marketList),
					Logic.StockMaster.AddStockMaster(excelData,marketList),
					Logic.StockMaster.AddIndustryCode17(excelData),
					Logic.StockMaster.AddIndustryCode33(excelData),
					Logic.StockMaster.AddClass(excelData)
				};

			Task.WaitAll(addTasks);
			Logic.Commit();
		}
	}
}