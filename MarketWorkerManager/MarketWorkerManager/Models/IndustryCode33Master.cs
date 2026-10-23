using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketWorkerManager.Models
{
    class IndustryCode33Master
    {
        /// <summary>
        /// 33業種コード
        /// </summary>
        [Key]
        [MaxLength(4)]
        [Column("code")]
        [Display(Name = "33業種コード")]
        public string Code { get; set; }

        /// <summary>
        /// 33業種名
        /// </summary>
        [Required]
        [MaxLength(50)]
        [Column("name", TypeName = "nvarchar")]
        [Display(Name = "33業種名")]
        public string Name { get; set; }
    }
}
