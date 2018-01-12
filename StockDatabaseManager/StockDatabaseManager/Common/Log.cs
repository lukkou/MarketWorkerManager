using NLog;

namespace StockDatabaseManager.Common
{
	public class Log
	{
		/// <summary>
		/// ロガー
		/// </summary>
		public static ILogger Logger = LogManager.GetCurrentClassLogger();
	}
}
