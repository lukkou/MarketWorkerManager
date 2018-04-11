using System;
using System.Diagnostics;
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
					Debug.WriteLine(monthStart);
					Debug.WriteLine(monthEnd);

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
		/// 毎月/毎日の実行処理
		/// </summary>
		/// <param name="status"></param>
		public void IndexCalendarRunner()
		{
			try
			{
				int nowDay = DateTime.Now.Day;

				if (nowDay == 15)
				{
					//毎月15日に翌月データを取得する
					MonthlyProcessing();
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
		private void MonthlyProcessing()
		{
			string nextMonthStart = DateTime.Now.AddMonths(1).ToString("yyyy-MM") + "-01";
			string nextMonthEnd = DateTime.Parse(DateTime.Now.AddMonths(2).ToString("yyyy-MM") + "-01 00:00:00").AddDays(-1).ToString("yyyy-MM-dd");

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

			var task = Logic.IndexData.GetMql5JsonAsync(currentMonthStart, nowDay);
			task.Wait();

			List<IndexCalendar> regData = Logic.IndexData.GetRegisteredIndex(currentMonthStart, nowDay);
			List<IndexCalendar> jsonData = Logic.IndexData.ResponseBodyToEntityModel(task.Result);
			List<IndexCalendar> webData = Logic.IndexData.GetSpecifiedRangeIndex(jsonData, currentMonthStart, nowDay);

			List<IndexCalendar> importData = Logic.IndexData.CompareNewnessIndexData(regData, webData);

			if (importData.Any())
			{
				Logic.IndexData.RegisteredIndexData(importData);
			}
		}
	}
}
