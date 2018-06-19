using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
				//int nowDay = 15;
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

			var task = Logic.IndexData.GetMql5JsonAsync(nextMonthStart, nextMonthEnd);
			task.Wait();

			List<IndexCalendar> jsonData = Logic.IndexData.ResponseBodyToEntityModel(task.Result);
			List<IndexCalendar> indexData = Logic.IndexData.GetSpecifiedRangeIndex(jsonData, nextMonthStart, nextMonthEnd);

			//当月で登録されているデータを取得
			string nowMonthStart = DateTime.Now.ToString("yyyy-MM") + "-01";
			string nowMonthEnd = DateTime.Parse(DateTime.Now.AddMonths(1).ToString("yyyy-MM") + "-01 00:00:00").AddDays(-1).ToString("yyyy-MM-dd");
			List<IndexCalendar> nowMonthData = Logic.IndexData.GetRegisteredIndex(nowMonthStart, nowMonthEnd);
			


			Logic.IndexData.RegisteredIndexData(indexData,true);
		}

		/// <summary>
		/// 毎日の経済指標データ更新
		/// </summary>
		private void DailyProcessing()
		{
			string currentMonthStart = DateTime.Parse(DateTime.Now.ToString("yyyy-MM") + "-01 00:00:00").AddDays(-1).ToString("yyyy-MM-dd");
			string nowDay = DateTime.Now.ToString("yyyy-MM-dd");

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
