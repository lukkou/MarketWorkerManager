using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace StockDatabaseManager.Models
{
	class IndustryCode33Master
	{
		/// <summary>
		/// 33業種コード
		/// </summary>
		[Key]
		public int Code { get; set; }

		/// <summary>
		/// 33業種名
		/// </summary>
		public string Name { get; set; }
	}
}
