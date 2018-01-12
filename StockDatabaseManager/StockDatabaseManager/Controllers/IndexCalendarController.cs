using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockDatabaseManager.Context;
using StockDatabaseManager.Models;

namespace StockDatabaseManager.Controllers
{
	class IndexCalendarController: BaseController
	{
		public void IndexCalendarRunner(string s)
		{
			try
			{

			}catch(Exception e)
			{

			}
		}

		private async void MonthlyProcessing()
		{
			string nextMonthStart = DateTime.Now.AddMonths(1).ToString("yyyy-MM") + "-01";
			string nextMonthEnd = DateTime.Parse(DateTime.Now.AddMonths(2).ToString("yyyy-MM") + "01 00:00:00").AddDays(-1).ToString("yyyy-MM-dd");

			List<IndexCalendar> indexData = await Logic.IndexData.GetIndexJsonToModelAsync(nextMonthStart, nextMonthEnd);

			Logic.IndexData.RegisteredIndexData(indexData);
		}

		private async void DailyProcessing()
		{
			string currentMonthStart = DateTime.Now.ToString("yyyy-MM") + "-01";
			string nowDay = DateTime.Now.ToString("yyyy-MM-dd");

			List<IndexCalendar> indexData = await Logic.IndexData.GetIndexJsonToModelAsync(currentMonthStart, nowDay);

			List<IndexCalendar> registeredData = Logic.IndexData.GetRegisteredIndexData(currentMonthStart, nowDay);
		}
	}
}
