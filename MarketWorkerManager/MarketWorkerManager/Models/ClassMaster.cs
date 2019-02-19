using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketWorkerManager.Models
{
	/// <summary>
	/// 上場規模
	/// </summary>
	class ClassMaster
	{
		/// <summary>
		/// 上場規模
		/// </summary>
		[Key]
		[MaxLength(2)]
		[Column("code")]
		[Display(Name = "上場規模コード")]
		public string Code { get; set; }

		/// <summary>
		/// 上場市場名
		/// </summary>
		[Required]
		[MaxLength(20)]
		[Column("name", TypeName = "nvarchar")]
		[Display(Name = "上場規模名")]
		public string Name { get; set; }
	}
}
