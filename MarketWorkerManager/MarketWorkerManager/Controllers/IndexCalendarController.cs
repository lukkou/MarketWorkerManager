using System;
using System.Collections.Generic;
using System.Linq;

using MarketWorkerManager.Common;
using MarketWorkerManager.Models;


namespace MarketWorkerManager.Controllers
{
	class IndexCalendarController : BaseController
	{
		/// <summary>
		/// エントリーポイント
		/// </summary>
		public void Run()
		{
			try
			{
				int nowDay = DateTime.Now.Day;
				if (nowDay == 15)
				{
					//毎月15日に翌月データを取得する
					NextMonthlyProcessing();
				}

				DailyProcessing();
			}
			catch (Exception e)
			{
				Log.Logger.Error(e.ToString());
				Console.WriteLine(e.Message);
				Console.ReadKey();
			}
		}

		/// <summary>
		/// 翌月の経済指標データの取得と登録
		/// </summary>
		private void NextMonthlyProcessing()
		{
			string nextMonthStart = DateTime.Now.AddMonths(1).ToString("yyyy-MM") + "-01";
			string nextMonthEnd = DateTime.Parse(DateTime.Now.AddMonths(2).ToString("yyyy-MM") + "-01 00:00:00").AddDays(-1).ToString("yyyy-MM-dd");

			Log.Logger.Info(nextMonthStart + "-" + nextMonthEnd + "のデータ取得");

			var task = Logic.IndexData.GetMql5JsonAsync(nextMonthStart, nextMonthEnd);
			task.Wait();

			List<IndexCalendar> jsonData = Logic.IndexData.ResponseBodyToEntityModel(task.Result);
			List<IndexCalendar> indexData = Logic.IndexData.GetSpecifiedRangeIndex(jsonData, nextMonthStart, nextMonthEnd);

			Logic.IndexData.RegisteredIndexData(indexData);
		}

		/// <summary>
		/// 毎日の経済指標データ更新
		/// </summary>
		private void DailyProcessing()
		{
			string currentMonthStart = DateTime.Now.ToString("yyyy-MM") + "-01";
			string nowDay = DateTime.Now.ToString("yyyy-MM-dd");

			Log.Logger.Info(currentMonthStart + "-" + nowDay + "のデータ取得");

			var task = Logic.IndexData.GetMql5JsonAsync(currentMonthStart, nowDay);
			task.Wait();

			List<IndexCalendar> myData = Logic.IndexData.GetRegisteredIndex(currentMonthStart, nowDay);
			List<IndexCalendar> jsonData = Logic.IndexData.ResponseBodyToEntityModel(task.Result);
			List<IndexCalendar> webData = Logic.IndexData.GetSpecifiedRangeIndex(jsonData, currentMonthStart, nowDay);

			List<IndexCalendar> importData = Logic.IndexData.CompareNewnessIndexData(myData, webData);

			if (importData.Any())
			{
				Logic.IndexData.RegisteredIndexData(importData);
			}
		}
	}
}
