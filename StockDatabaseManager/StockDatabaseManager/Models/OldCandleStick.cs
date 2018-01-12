using System;
using System.ComponentModel.DataAnnotations;
namespace StockDatabaseManager.Models
{
	class OldCandleStick
	{
		/// <summary>
		/// Guidキー
		/// </summary>
		[Key]
		[Display(Name = "Guidキー")]
		public Guid GuidKey { get; set; }

		/// <summary>
		/// 銘柄コード
		/// </summary>
		[Key]
		[MaxLength(4)]
		[Display(Name = "銘柄コード")]
		public string StockCode { get; set; }

		/// <summary>
		/// 取引日
		/// </summary>
		[Key]
		[Display(Name = "取引日")]
		public DateTime BusinessDay { get; set; }

		/// <summary>
		/// 寄値
		/// </summary>
		[Display(Name = "寄値")]
		public double Open { get; set; }

		/// <summary>
		/// 高値
		/// </summary>
		[Display(Name = "高値")]
		public double High { get; set; }

		/// <summary>
		/// 安値
		/// </summary>
		[Display(Name = "安値")]
		public double Low { get; set; }

		/// <summary>
		/// 終値
		/// </summary>
		[Display(Name = "終値")]
		public double Close { get; set; }

		/// <summary>
		/// 出来高
		/// </summary>
		[Display(Name = "出来高")]
		public double Volume { get; set; }
	}
}
