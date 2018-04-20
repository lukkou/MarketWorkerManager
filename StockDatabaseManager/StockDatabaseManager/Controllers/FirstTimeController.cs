using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StockDatabaseManager.Common;
using StockDatabaseManager.Models;
using StockDatabaseManager.DataModels;

namespace StockDatabaseManager.Controllers
{
	class FirstTimeController : BaseController
	{
		/// <summary>
		/// 初回実行時に当月から一年前までの指標データの作成
		/// </summary>
		public void AddFirstIndexData()
		{
			try
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

				Logic.IndexData.RegisteredIndexData(yearList);
			}
			catch (Exception e)
			{
				Log.Logger.Error(e.ToString());
				Console.WriteLine(e.Message);
				Console.ReadKey();
			}
		}

		/// <summary>
		/// 初回実行時の株マスターデータ作成
		/// </summary>
		public void AddFirstStockMasterData()
		{
			try
			{
				ExcelDownload();
				List<TokyoStockExchangeExcelModel> excelData = Logic.StockMaster.ExcelToDataModel(GetExcelSaveDirectory() + Define.Stock.TokyoExchangeExcel);

				Logic.BeginTransaction();
				var addTasks = new[]
				{
					Logic.StockMaster.AddStockMaster(excelData),
					Logic.StockMaster.AddIndustryCode17(excelData),
					Logic.StockMaster.AddIndustryCode33(excelData),
					Logic.StockMaster.AddClass(excelData)
				};

				Task.WaitAll(addTasks);
				Logic.Commit();


			}
			catch (Exception e)
			{
				Logic.Rollback();
				Log.Logger.Error(e.ToString());
				Console.WriteLine(e.Message);
				Console.ReadKey();
			}
		}


	}
}
