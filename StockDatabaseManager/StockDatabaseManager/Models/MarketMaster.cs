using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace StockDatabaseManager.Models
{
	class MarketMaster
	{
		[Key]
		public int Code { get; set; }
		public string Name { get; set; }
	}
}
