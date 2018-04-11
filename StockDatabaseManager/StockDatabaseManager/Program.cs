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
				if (Properties.Settings.Default.FirstInsert)
				{
					//データベースへテーブルを作成



					var IndexCalendar = new IndexCalendarController();
					IndexCalendar.AddFirstIndexData();

					//初回statusを更新
				}
			}
			else
			{
				if (Properties.Settings.Default.FirstInsert)
				{
					//初回の場合はDB作成から…
					return;
				}

				string firstStatus = args[0];
				string secondStatus = args[1];
				if (firstStatus == Define.StockStatus)
				{
					//株式データ
					var StockMaster = new StockMasterController();
					StockMaster.StockMasterRunner(secondStatus);
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
