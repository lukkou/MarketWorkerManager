using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockDatabaseManager.Models
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
		[Column("stockcode")]
		[Display(Name = "銘柄コード")]
		public string StockCode { get; set; }

		/// <summary>
		/// 銘柄名
		/// </summary>
		[Required]
		[MaxLength(100)]
		[Column("stockname", TypeName = "nvarchar")]
		[Display(Name = "銘柄名")]
		public string StockName { get; set; }

		/// <summary>
		/// 市場コード
		/// </summary>
		[MaxLength(2)]
		[Column("marketcode")]
		[Display(Name = "市場コード")]
		public string MarketCode { get; set; }

		/// <summary>
		/// 33業種コード
		/// </summary>
		[MaxLength(4)]
		[Column("industrycode33")]
		[Display(Name = "33業種コード")]
		public string IndustryCode33 { get;set;}

		/// <summary>
		/// 17業種コード
		/// </summary>
		[MaxLength(2)]
		[Column("industrycode17")]
		[Display(Name = "17業種コード")]
		public string IndustryCode17 { get; set; }

		/// <summary>
		/// 規模コード
		/// </summary>
		[MaxLength(2)]
		[Column("classcode")]
		[Display(Name = "規模コード")]
		public string ClassCode { get; set; }
	}
}
