using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketWorkerManager.Models
{
    class History
    {
        /// <summary>
        /// 通貨のシンボル
        /// </summary>
        [Key]
        [Column("Symbol", Order = 0)]
        [Display(Name = "Symbol")]
        public string Symbol { get; set;}

        /// <summary>
        /// 
        /// </summary>
        public long OrderNo { get; set; }

    }
}
