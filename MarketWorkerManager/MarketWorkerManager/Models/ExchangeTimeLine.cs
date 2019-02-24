using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketWorkerManager.Models
{
	/// <summary>
	/// 為替時系列
	/// </summary>
	class ExchangeTimeLine
	{
		/// <summary>
		/// シンボル
		/// </summary>
		[Key]
		[MaxLength(6)]
		[Column("symbol", Order = 0)]
		[Display(Name = "通貨ペア")]
		public string Symbol { get; set; }

		/// <summary>
		/// 取引日
		/// </summary>
		[Key]
		[Column("tradedatetime", Order = 1)]
		[Display(Name = "取引時間")]
		public DateTime TradeDateTime { get; set; }

		/// <summary>
		/// 取引時間軸
		/// </summary>
		[Key]
		[Column("timespan", Order = 2)]
		[Display(Name = "時間軸(1 = 1分足、900 = 15分足)")]
		public int TimeSpan { get; set; }

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
	}
}
