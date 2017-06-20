using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockDatabaseManager.DataModels
{
	/// <summary>
	/// 東証よりダウンロードした銘柄Excelモデル
	/// </summary>
	public class TokyoStockExchangeExcelModel
	{
		/// <summary>
		/// 日付
		/// </summary>
		public int UpdatedDate { get; set; }

		/// <summary>
		/// コード
		/// </summary>
		public int Code { get; set; }

		/// <summary>
		/// 銘柄名
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 市場・商品区分
		/// </summary>
		public string MarketName { get; set; }

		/// <summary>
		/// 33業種コード
		/// </summary>
		public int Category33Code { get; set; }

		/// <summary>
		/// 33業種区分
		/// </summary>
		public string Category33Name { get; set; }

		/// <summary>
		/// 17業種コード
		/// </summary>
		public int Category17Code { get; set; }

		/// <summary>
		/// 17業種区分
		/// </summary>
		public string Category17Name { get; set; }

		/// <summary>
		/// 規模コード
		/// </summary>
		public int ClassCode { get; set; }

		/// <summary>
		/// 規模区分
		/// </summary>
		public string ClassName { get; set; }
	}
}
