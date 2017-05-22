using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StockDatabaseManager
{
	class StockMaster: System.Data.Entity.DbContext
	{
		/// <summary>
		/// 
		/// </summary>
		[Key]
		public int StockCode { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string StockName { get; set; }
		public string MarketName { get; set; }
		public int IndustryCode33 { get;set;}
		public int IndustryCode17 { get; set; }
		public int ClassCode { get; set; }
	}
}
