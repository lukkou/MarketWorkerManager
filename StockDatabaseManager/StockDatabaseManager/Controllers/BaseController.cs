using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StockDatabaseManager.Context;

namespace StockDatabaseManager.Controllers
{

	class BaseController
	{
		public LogicContext Logic { get; private set; }

		public BaseController()
		{
			Logic = new LogicContext();
		}


	}
}
