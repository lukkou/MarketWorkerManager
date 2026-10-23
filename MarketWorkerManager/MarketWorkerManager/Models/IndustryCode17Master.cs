using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketWorkerManager.Models
{
    class IndustryCode17Master
    {
        /// <summary>
        /// 17業種コード
        /// </summary>
        [Key]
        [MaxLength(2)]
        [Column("code")]
        [Display(Name = "17業種コード")]
        public string Code { get; set; }

        /// <summary>
        /// 17業種名
        /// </summary>
        [Required]
        [MaxLength(50)]
        [Column("name", TypeName = "nvarchar")]
        [Display(Name = "17業種名")]
        public string Name { get; set; }
    }
}