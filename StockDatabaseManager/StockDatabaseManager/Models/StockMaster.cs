using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace StockDatabaseManager
{
	class StockMaster
	{
		[Key]
		public int StockCode { get; set; }
		public string StockName { get; set; }
		public string MarketName { get; set; }
		public int IndustryCode33 { get;set;}
		public int IndustryCode17 { get; set; }
		public int ClassCode { get; set; }
	}
}
