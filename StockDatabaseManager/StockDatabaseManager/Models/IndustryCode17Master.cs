using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace StockDatabaseManager.Models
{
	class IndustryCode17Master
	{
		[Key]
		public int Code { get; set; }
		public string Name { get; set; }
	}
}
