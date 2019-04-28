namespace IndexNotification.Common
{
    class Define
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
        /// MQL5のベースURL
        /// </summary>
        public const string Mql5BaseUrl = "https://www.mql5.com";

        /// <summary>
        /// Json取得用正規パターン
        /// </summary>
        public const string JsonRegular = @"\[.+?\]";

        /// <summary>
        /// UTCタイムゾーン摘要
        /// </summary>
        public const string TimeModeUTC = "0";

        /// <summary>
        /// 指標発表のタイムモード
        /// </summary>
        public const string TimeModeIndexInfo = "0";

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
