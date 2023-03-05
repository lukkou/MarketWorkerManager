using System;

namespace MarketWorkerManager.DataModels
{
    /// <summary>
    /// 上場廃止銘柄保持モデル
    /// </summary>
    class DeleteStockModel
    {
        /// <summary>
        /// Guidキー
        /// </summary>
        public Guid GuidKey { get; set; }

        /// <summary>
        /// 銘柄コード
        /// </summary>
        public string StockCode { get; set; }
    }
}
