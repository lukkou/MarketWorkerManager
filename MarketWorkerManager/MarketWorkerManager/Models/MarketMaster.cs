using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketWorkerManager.Models
{
    class MarketMaster
    {
        /// <summary>
        /// 市場ID
        /// </summary>
        [Key]
        [Column("marketid")]
        [MaxLength(2)]
        [Display(Name = "市場ID")]
        public string MarketId { get; set; }

        /// <summary>
        /// 市場名
        /// </summary>
        [Column("marketname")]
        [MaxLength(100)]
        [Display(Name = "市場名")]
        public string MarketName { get; set; }
    }
}
