
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
            public const string ConsumerKey = "0t9p71eRVmaC2LQeLvfzoirW1";
            public const string ConsumerSecret = "mKMqKpnWOaxdNmsMKpGCP3fVCAhT3IMnltgqrWs6WF5cEetrsN";
            public const string AccessToken = "897014425856557056-b9RKh8QkJMw1B66En8ojxY93jh0LlU0";
            public const string AccessSecret = "KSU3Qb2z7oF2vg9BQvbLmGGMAdMcHpgJF2zxCL4zlT1F3";
        }
    }
}
