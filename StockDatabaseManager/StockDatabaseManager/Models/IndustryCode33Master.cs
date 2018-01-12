using System.ComponentModel.DataAnnotations;

namespace StockDatabaseManager.Models
{
	class IndustryCode33Master
	{
		/// <summary>
		/// 33業種コード
		/// </summary>
		[Key]
		[Display(Name = "33業種コード")]
		public int Code { get; set; }

		/// <summary>
		/// 33業種名
		/// </summary>
		[Required]
		[MaxLength(50)]
		[Display(Name = "33業種名")]
		public string Name { get; set; }
	}
}
