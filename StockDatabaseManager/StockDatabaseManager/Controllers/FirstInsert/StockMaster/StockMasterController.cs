using System;
using Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StockDatabaseManager.Common;

namespace StockDatabaseManager.Controllers
{
	class StockMasterController: BaseController
	{

		public void StockMasterLoad()
		{

		}

		public void ExcelDownload()
		{
			//保存先を作成
			string saveDirectory = ConfigurationManager.AppSettings["key1"];
		}
	}
}
