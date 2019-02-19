using System;
using MarketWorkerManager.Common;
using MarketWorkerManager.Controllers;

namespace MarketWorkerManager
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				Console.Write("初期データを作成しますか？ Y/N：");
				string deleteExecuteStatus = Console.ReadLine();
				if (deleteExecuteStatus.ToUpper() == Define.RunStatus)
				{
					using (FirstTimeController firstTime = new FirstTimeController())
					{
						firstTime.Run();
					}
				}
			}
			else
			{
				string firstStatus = args[0];
				if (firstStatus == Define.StockStatus)
				{
					//株式データ
					var StockMaster = new StockMasterController();
				}
				else if (firstStatus == Define.FXStatus)
				{
					using (IndexCalendarController IndexCalendar = new IndexCalendarController())
					{
						//FX用データ
						IndexCalendar.Run();
					}
				}
			}
		}
	}
}
