
namespace StockDatabaseManager.Common
{
	public class Define
	{
		public const string StockStatus = "0";

		public const string FXStatus = "1";

		public class Stock
		{
			public const string TokyoExchangeUrl = "http://www.jpx.co.jp/markets/statistics-equities/misc/tvdivq0000001vg2-att/data_j.xls";

			public const string TokyoExchangeDirectory = "\\TokyoExchange";

			public const string TokyoExchangeExcel = "\\data.xls";
			public const string ExcelSheetName = "Sheet1";
		}

		/// <summary>
		/// Fx関連
		/// </summary>
		public class Fx
		{
			public const string Monthly = "0";

			public const string Daily = "1";
		}

		/// <summary>
		/// 経済指標関連
		/// </summary>
		public class Index
		{
			/// <summary>
			/// 指標カレンダーAPIURL
			/// </summary>
			public const string Mql5ApiUrl = "https://www.mql5.com/ja/economic-calendar/content?";

			/// <summary>
			/// UTCタイムゾーン摘要
			/// </summary>
			public const string TimeModeUTC = "0";

			/// <summary>
			/// グリニッジ標準時
			/// </summary>
			public const string TimeModeGMT = "1";

			/// <summary>
			/// 指標未公開フラグ
			/// </summary>
			public const int ProcessedOff = 0;

			/// <summary>
			/// 指標公開フラグ
			/// </summary>
			public const int ProcessedOn = 1;

			/// <summary>
			/// Json取得用正規パターン
			/// </summary>
			public const string JsonRegular = @"\[.+?\]";
		}
	}
}
