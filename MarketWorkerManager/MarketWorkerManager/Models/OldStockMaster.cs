using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketWorkerManager.Models
{
    /// <summary>
    /// 上場廃止銘柄マスター
    /// </summary>
    class OldStockMaster
    {
        /// <summary>
        /// Guidキー
        /// </summary>
        [Key]
        [Column("guidkey", Order = 0)]
        [Display(Name = "Guidキー")]
        public Guid GuidKey { get; set; }

        /// <summary>
        /// 銘柄コード
        /// </summary>
        [Key]
        [MaxLength(4)]
        [Column("stockcode", Order = 1)]
        [Display(Name = "銘柄コード")]
        public string StockCode { get; set; }

        /// <summary>
        /// 上場廃止日（yyyyMMでしか取得出来ないので現状はyyyyMM）
        /// </summary>
        [MaxLength(6)]
        [Column("deletedate")]
        [Display(Name = "上場廃止日")]
        public string DeleteDate { get; set; }

        /// <summary>
        /// 銘柄名
        /// </summary>
        [Required]
        [MaxLength(100)]
        [Column("stockname", TypeName = "nvarchar")]
        [Display(Name = "銘柄名")]
        public string StockName { get; set; }

        /// <summary>
        /// 市場コード
        /// </summary>
        [MaxLength(2)]
        [Column("marketcode")]
        [Display(Name = "市場コード")]
        public string MarketCode { get; set; }

        /// <summary>
        /// 33業種コード
        /// </summary>
        [MaxLength(4)]
        [Column("industrycode33")]
        [Display(Name = "33業種コード")]
        public string IndustryCode33 { get; set; }

        /// <summary>
        /// 17業種コード
        /// </summary>
        [MaxLength(2)]
        [Column("industrycode17")]
        [Display(Name = "17業種コード")]
        public string IndustryCode17 { get; set; }

        /// <summary>
        /// 規模コード
        /// </summary>
        [MaxLength(2)]
        [Column("classcode")]
        [Display(Name = "規模コード")]
        public string ClassCode { get; set; }
    }
}
