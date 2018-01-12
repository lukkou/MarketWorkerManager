using System.ComponentModel.DataAnnotations;

namespace StockDatabaseManager.Models
{
	class IndustryCode17Master
	{
		/// <summary>
		/// 17業種コード
		/// </summary>
		[Key]
		[Display(Name = "17業種コード")]
		public int Code { get; set; }

		/// <summary>
		/// 17業種名
		/// </summary>
		[Required]
		[MaxLength(50)]
		[Display(Name = "17業種名")]
		public string Name { get; set; }
	}
}
