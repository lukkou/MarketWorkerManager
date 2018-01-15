using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockDatabaseManager.Models
{
	/// <summary>
	/// 
	/// </summary>
	class MarketMaster
	{
		/// <summary>
		/// 市場・商品区分コード
		/// </summary>
		[Key]
		[MaxLength(2)]
		[Column("code")]
		[Display(Name = "市場・商品区分コード")]
		public string Code { get; set; }

		/// <summary>
		/// 市場・商品区分名
		/// </summary>
		[Required]
		[MaxLength(50)]
		[Column("name",TypeName = "nvarchar")]
		[Display(Name = "市場・商品区分名")]
		public string Name { get; set; }
	}
}
