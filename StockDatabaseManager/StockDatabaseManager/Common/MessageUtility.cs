using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockDatabaseManager.Common
{
	class MessageUtility
	{
		public static string CreateConsoleMessage(string msg)
		{
			return DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + "：" + msg;
		}
	}
}
