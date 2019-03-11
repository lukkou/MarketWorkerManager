using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketWorkerManager.Models
{
	class NotificationFlg
	{
		/// <summary>
		/// GUIDキー
		/// </summary>
		[Key]
		[Column("guidkey", Order = 0)]
		[Display(Name = "Guidキー")]
		public Guid GuidKey { get; set; }

		/// <summary>
		/// Idキー
		/// </summary>
		[Key]
		[Column("idkey", Order = 1)]
		[MaxLength(10)]
		[Display(Name = "IDキー")]
		public string IdKey { get; set; }

		/// <summary>
		/// ツイッター通知フラグ
		/// </summary>
		[Column("tweetflg")]
		[Display(Name = "ツイート済みフラグ")]
		public bool TweetFlg { get; set; }
	}
}
