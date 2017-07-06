using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace StockDatabaseManager.Models
{
	class MarketMaster
	{
		/// <summary>
		/// 市場・商品区分コード
		/// </summary>
		[Key]
		[MaxLength(2)]
		[Display(Name = "市場・商品区分コード")]
		public int Code { get; set; }

		/// <summary>
		/// 市場・商品区分名
		/// </summary>
		[Required]
		[MaxLength(2)]
		[Display(Name = "市場・商品区分名")]
		public string Name { get; set; }
	}
}
