using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace StockDatabaseManager
{
	/// <summary>
	/// 銘柄マスター
	/// </summary>
	class StockMaster
	{
		/// <summary>
		/// 銘柄コード
		/// </summary>
		[Key]
		[MaxLength(4)]
		[Display(Name = "銘柄コード")]
		public int StockCode { get; set; }

		/// <summary>
		/// 銘柄名
		/// </summary>
		[Required]
		[MaxLength(100)]
		[Display(Name = "銘柄名")]
		public string StockName { get; set; }

		/// <summary>
		/// 市場コード
		/// </summary>
		[MaxLength(1)]
		[Display(Name = "市場コード")]
		public int MarketCode { get; set; }

		/// <summary>
		/// 33業種コード
		/// </summary>
		[MaxLength(4)]
		[Display(Name = "33業種コード")]
		public int IndustryCode33 { get;set;}

		/// <summary>
		/// 17業種コード
		/// </summary>
		[MaxLength(2)]
		[Display(Name = "17業種コード")]
		public int IndustryCode17 { get; set; }

		/// <summary>
		/// 規模コード
		/// </summary>
		[MaxLength(1)]
		[Display(Name = "規模コード")]
		public int ClassCode { get; set; }
	}
}
