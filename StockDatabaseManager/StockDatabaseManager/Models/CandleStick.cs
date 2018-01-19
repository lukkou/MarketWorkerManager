using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockDatabaseManager.Models
{
	/// <summary>
	/// ローソク足
	/// </summary>
	class CandleStick
	{
		/// <summary>
		/// 銘柄コード
		/// </summary>
		[Key]
		[MaxLength(4)]
		[Column("stockcode", Order = 0)]
		[Display(Name = "銘柄コード")]
		public string StockCode { get; set; }

		/// <summary>
		/// 取引日
		/// </summary>
		[Key]
		[Column("businessday", Order = 1)]
		[Display(Name = "取引日")]
		public DateTime BusinessDay { get; set; }

		/// <summary>
		/// 寄値
		/// </summary>
		[Column("open")]
		[Display(Name = "寄値")]
		public double Open { get; set; }

		/// <summary>
		/// 高値
		/// </summary>
		[Column("high")]
		[Display(Name = "高値")]
		public double High { get; set; }

		/// <summary>
		/// 安値
		/// </summary>
		[Column("low")]
		[Display(Name = "安値")]
		public double Low { get; set; }

		/// <summary>
		/// 終値
		/// </summary>
		[Column("close")]
		[Display(Name = "終値")]
		public double Close { get; set; }

		/// <summary>
		/// 出来高
		/// </summary>
		[Column("volume")]
		[Display(Name = "出来高")]
		public double Volume { get; set; }
	}
}
