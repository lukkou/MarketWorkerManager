using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockDatabaseManager.Common;
using StockDatabaseManager.Models;


namespace StockDatabaseManager.Controllers
{
	class IndexCalendarController: BaseController
	{
		/// <summary>
		/// 指標データ取得処理
		/// </summary>
		/// <param name="status"></param>
		public void IndexCalendarRunner(string status)
		{
			try
			{
				if (status == Define.Fx.Monthly)
				{
					MonthlyProcessing();
				}
				else if (status == Define.Fx.Daily)
				{
					DailyProcessing();
				}
			}
			catch (Exception e)
			{
				Log.Logger.Error(e.ToString());
			}
		}

		/// <summary>
		/// 翌月データの取得と登録
		/// </summary>
		private async void MonthlyProcessing()
		{
			string nextMonthStart = DateTime.Now.AddMonths(1).ToString("yyyy-MM") + "-01";
			string nextMonthEnd = DateTime.Parse(DateTime.Now.AddMonths(2).ToString("yyyy-MM") + "-01 00:00:00").AddDays(-1).ToString("yyyy-MM-dd");

			var responseBody = Logic.IndexData.GetMql5JsonAsync(nextMonthStart, nextMonthEnd);

			List<IndexCalendar> indexData = Logic.IndexData.ResponseBodyToEntityModel(responseBody.Result);

			Logic.IndexData.RegisteredIndexData(indexData);
		}

		/// <summary>
		/// 毎日のデータ更新
		/// </summary>
		private async void DailyProcessing()
		{
			string currentMonthStart = DateTime.Now.ToString("yyyy-MM") + "-01";
			string nowDay = DateTime.Now.ToString("yyyy-MM-dd");

			var responseBody = await Logic.IndexData.GetMql5JsonAsync(currentMonthStart, nowDay);

			List<IndexCalendar> indexData = Logic.IndexData.ResponseBodyToEntityModel(responseBody);
			List<IndexCalendar> registeredData = Logic.IndexData.GetRegisteredIndexData(currentMonthStart, nowDay);

			List<IndexCalendar> importData = Logic.IndexData.CompareNewnessIndexData(indexData, registeredData);

			if (importData.Any())
			{
				Logic.IndexData.RegisteredIndexData(importData);
			}
		}
	}
}
