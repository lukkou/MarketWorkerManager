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
    }
}
