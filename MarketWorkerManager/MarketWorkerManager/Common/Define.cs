
namespace MarketWorkerManager.Common
{
    public class Define
    {
        /// <summary>
        /// 株用ステータス
        /// </summary>
        public const string StockStatus = "0";

        /// <summary>
        /// Fx用ステータス
        /// </summary>
        public const string FXStatus = "1";

        /// <summary>
        /// コンソールからの実行の場合のステータス
        /// </summary>
        public const string RunStatus = "Y";

        /// <summary>
        /// コンソールからの未実行の場合のステータス
        /// </summary>
        public const string NotRunStatus = "N";

        public class Stock
        {
            /// <summary>
            /// 東証銘柄ExcelDL用URL
            /// </summary>
            public const string TokyoExchangeUrl = "http://www.jpx.co.jp/markets/statistics-equities/misc/tvdivq0000001vg2-att/data_j.xls";

            /// <summary>
            /// 東証銘柄Excelファイル名
            /// </summary>
            public const string TokyoExchangeExcel = "\\data.xls";

            /// <summary>
            /// 東証銘柄Excelシート名
            /// </summary>
            public const string ExcelSheetName = "Sheet1";

            /// <summary>
            /// パス
            /// </summary>
            public const string TokyoExchangeDirectory = "\\TokyoExchange";

            /// <summary>
            /// QuandlApiキー
            /// </summary>
            public const string QuandlApiKey = "Y5ysVpvTnde_ppzSbpEG";
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

            /// <summary>
            /// 各国の祝日のイベントタイプ
            /// </summary>
            public const int EventType_PublicHoliday = 2;
        }

        /// <summary>
        /// Tweeter関連
        /// </summary>
        public class Tweeter
        {
            public const string ConsumerKey = "SCI6zXvgfqnaf2YqPkApUkwHv";
            public const string ConsumerSecret = "LiR7cbVRC3TT1U8mvRDs2rYbGtNizkVNAwWQmEfwKlCG0MobdZ";
            public const string AccessToken = "897014425856557056-P8kdYE8TstqUqD82DoJ0oInic62BjLn";
            public const string AccessSecret = "A7kB8z0fEkszrtqig9x0BJS9NOx8CHCWlPTYxGEJWDJQb";
        }

        /// <summary>
        /// 通貨コード
        /// </summary>
        public class CurrencyCode
        {
            public const string US = "USD";
            public const string JP = "JPY";
            public const string EU = "EUR";
            public const string GB = "GBP";
            public const string CA = "CAD";
            public const string AU = "AUD";
            public const string CH = "CHF";
        }

        /// <summary>
        /// 国名
        /// </summary>
        public class CountryName
        {
            public const string US = "アメリカ合衆国";
            public const string JP = "日本";
            public const string EU = "ユーロ圏";
            public const string GB = "イギリス";
            public const string CA = "カナダ";
            public const string AU = "オーストラリア";
            public const string CH = "スイス";

            //ユーロ圏
            public const string DE = "ドイツ";
            public const string IT = "イタリア";
            public const string FR = "フランス";
            public const string ES = "スペイン";
        }

        /// <summary>
        /// 国旗コード
        /// </summary>
        public class CompanyFlag
        {
            public const string US = "🇺🇸";
            public const string JP = "🇯🇵";
            public const string EU = "🇪🇺";
            public const string GB = "🇬🇧";
            public const string CA = "🇨🇦";
            public const string AU = "🇦🇺";
            public const string CH = "🇨🇭";

            //ユーロ圏国旗
            public const string DE = "🇩🇪";
            public const string IT = "🇮🇹";
            public const string FR = "🇫🇷";
            public const string ES = "🇪🇸";
        }
    }
}
