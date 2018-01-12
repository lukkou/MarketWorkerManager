using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace StockDatabaseManager.Models
{
	/// <summary>
	/// 指標カレンダー
	/// </summary>
	[JsonObject]
	class IndexCalendar
	{
		/// <summary>
		/// GUIDキー
		/// </summary>
		[Key]
		[Display(Name = "Guidキー")]
		public Guid GuidKey { get; set; }

		/// <summary>
		/// Idキー
		/// </summary>
		[Key]
		[MaxLength(10)]
		[Display(Name = "IDキー")]
		[JsonProperty("Id")]
		public string IdKey { get; set; }

		/// <summary>
		/// データ公開日
		/// </summary>
		[Required]
		[Display(Name = "データ公開日")]
		[JsonProperty("ReleaseDate")]
		public string ReleaseDate { get; set; }

		/// <summary>
		/// データ公開日（GMT）
		/// </summary>
		[Required]
		[Display(Name = "データ公開日GMT")]
		public DateTime ReleaseDateGmt { get; set; }

		/// <summary>
		/// データ公開日（ローカルUTCゾーン）
		/// </summary>
		[Index]
		[Required]
		[Display(Name = "データ公開日ローカルUTCゾーン")]
		public DateTime MyReleaseDate { get; set; }

		/// <summary>
		/// UTC時刻の摘要フラグ
		/// </summary>
		[Display(Name = "UTC時刻の摘要フラグ")]
		[JsonProperty("TimeMode")]
		public string TimeMode { get; set; }

		/// <summary>
		/// 国
		/// </summary>
		[Required]
		[MaxLength(3)]
		[Display(Name = "国")]
		[JsonProperty("CurrencyCode")]
		public string CurrencyCode { get; set; }

		/// <summary>
		/// タイトル
		/// </summary>
		[Index]
		[Required]
		[MaxLength(100)]
		[Display(Name = "タイトル")]
		[JsonProperty("EventName")]
		public string EventName { get; set; }

		/// <summary>
		/// イベントタイプ
		/// </summary>
		[Display(Name = "イベントタイプ")]
		[JsonProperty("EventType")]
		public int EventType { get; set; }

		/// <summary>
		/// 重要度（none、low、medium、high）
		/// </summary>
		[Required]
		[MaxLength(6)]
		[Display(Name = "重要度")]
		[JsonProperty("Importance")]
		public string Importance { get; set; }

		/// <summary>
		/// 情報公開済フラグ
		/// </summary>
		[Display(Name = "情報公開フラグ")]
		[JsonProperty("Processed")]
		public int Processed { get; set; }

		/// <summary>
		/// 実績（今回）値
		/// </summary
		[MaxLength(20)]
		[Display(Name = "実績（今回）値")]
		[JsonProperty("ActualValue")]
		public string ActualValue { get; set; }

		/// <summary>
		/// 予想値
		/// </summary>
		[MaxLength(20)]
		[Display(Name = "予想値")]
		[JsonProperty("ForecastValue")]
		public string ForecastValue { get; set; }

		/// <summary>
		/// 前回値
		/// </summary>
		[MaxLength(20)]
		[Display(Name = "前回値")]
		[JsonProperty("PreviousValue")]
		public string PreviousValue { get; set; }

		/// <summary>
		/// /前回値（修正前）
		/// </summary>
		[MaxLength(20)]
		[Display(Name = "前回値（修正前）")]
		[JsonProperty("OldPreviousValue")]
		public string OldPreviousValue { get; set; }

		/// <summary>
		/// MQL5コミュニティURL
		/// </summary>
		[MaxLength(2048)]
		[Display(Name = "MQL5コミュニティURL")]
		[JsonProperty("Url")]
		public string LinkUrl { get; set; }
	}
}
