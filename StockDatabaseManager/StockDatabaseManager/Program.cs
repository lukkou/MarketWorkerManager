using System;
using StockDatabaseManager.Common;
using StockDatabaseManager.Controllers;


namespace StockDatabaseManager
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length == 0)
			{

				string deleteExecuteStatus = Console.ReadLine();
				if(deleteExecuteStatus.ToUpper() == Define.RunStatus)
				{
					var firstTime = new FirstTimeController();
					firstTime.EnvironmentBuilding();
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
					//FX用データ
					var IndexCalendar = new IndexCalendarController();
					IndexCalendar.IndexCalendarRunner();
				}
			}
		}
	}
}
