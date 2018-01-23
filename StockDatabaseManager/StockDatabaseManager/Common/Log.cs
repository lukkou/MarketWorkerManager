using NLog;

namespace StockDatabaseManager.Common
{
	public class Log
	{
		/// <summary>
		/// ロガー
		/// </summary>
		public static ILogger Logger = LogManager.GetLogger("System");

		public static ILogger SqlLogger = LogManager.GetLogger("Sql");
	}
}
