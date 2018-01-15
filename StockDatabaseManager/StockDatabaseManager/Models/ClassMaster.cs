using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockDatabaseManager.Models
{
	/// <summary>
	/// 
	/// </summary>
	class ClassMaster
	{
		/// <summary>
		/// 上場市場コード
		/// </summary>
		[Key]
		[MaxLength(2)]
		[Column("code")]
		[Display(Name = "上場市場コード")]
		public string Code { get; set; }

		/// <summary>
		/// 上場市場名
		/// </summary>
		[Required]
		[MaxLength(20)]
		[Column("name", TypeName = "nvarchar")]
		[Display(Name = "上場市場名")]
		public string Name { get; set; }
	}
}
