using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockDatabaseManager.Common
{
	public class Define
	{
		public const string StockStatus = "0";

		public const string FXStatus = "2";

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
			public const string Mql5_ApiUrl = "https://www.mql5.com/ja/economic-calendar/content?";

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
