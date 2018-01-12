using System.ComponentModel.DataAnnotations;

namespace StockDatabaseManager.Models
{
	class ClassMaster
	{
		/// <summary>
		/// 上場市場コード
		/// </summary>
		[Key]
		[Display(Name = "上場市場コード")]
		public int Code { get; set; }

		/// <summary>
		/// 上場市場名
		/// </summary>
		[Required]
		[MaxLength(20)]
		[Display(Name = "上場市場名")]
		public string Name { get; set; }
	}
}
