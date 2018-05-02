using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtimeIndexImporter.Common
{
	public class Define
	{
		/// <summary>
		/// 指標重要度（高）
		/// </summary>
		public const string ImportanceHigh = "high";

		/// <summary>
		/// 指標重要度（中）
		/// </summary>
		public const string ImportanceMedium = "medium";

		/// <summary>
		/// 指標重要度（低）
		/// </summary>
		public const string ImportanceLow = "low";

		/// <summary>
		/// 指標カレンダーAPIURL
		/// </summary>
		public const string Mql5ApiUrl = "https://www.mql5.com/ja/economic-calendar/content?";

		/// <summary>
		/// Json取得用正規パターン
		/// </summary>
		public const string JsonRegular = @"\[.+?\]";

		/// <summary>
		/// UTCタイムゾーン摘要
		/// </summary>
		public const string TimeModeUTC = "0";

		/// <summary>
		/// Tweeter関連
		/// </summary>
		public class Tweeter
		{
			public const string ConsumerKey = "";
			public const string ConsumerSecret = "";
			public const string AccessToken = "";
			public const string AccessSecret = "";
		}
	}
}
