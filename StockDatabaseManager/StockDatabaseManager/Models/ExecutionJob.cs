using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockDatabaseManager.Models
{
	public class ExecutionJob
	{
		/// <summary>
		/// Guidキー
		/// </summary>
		[Key]
		[Column("guidkey")]
		[Display(Name = "Guidキー")]
		public Guid GuidKey { get; set; }

		/// <summary>
		/// 登録日
		/// </summary>
		[Required]
		[Column("registrationdate")]
		[Display(Name = "登録日")]
		public DateTime RegistrationDate { get; set; }

		/// <summary>
		/// 登録結果(Success,Errorのみ)
		/// </summary>
		[Required]
		[MaxLength(7)]
		[Column("registrationstatus")]
		[Display(Name = "登録結果")]
		public string RegistrationStatus { get; set; }

		/// <summary>
		/// 備考
		/// </summary>
		[Required]
		[Column("remarks")]
		[Display(Name = "備考")]
		public string Remarks { get; set; }
	}
}
