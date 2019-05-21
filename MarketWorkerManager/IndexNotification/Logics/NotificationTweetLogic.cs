using System.Collections.Generic;
using CoreTweet;

using IndexNotification.Utility;
using IndexNotification.Common;
using IndexNotification.Models;

namespace IndexNotification.Logic
{
    class NotificationTweetLogic
    {
        /// <summary>
        /// 金利文字定数
        /// </summary>
        private const string InterestRate = "金利";

        /// <summary>
        /// 英国の固定文字
        /// </summary>
        private const string GreatBritain = "英国";

        /// <summary>
        /// 指標発表30分前情報をツイート
        /// </summary>
        /// <param name="list"></param>
        public void IndexNotificationTweet(List<IndexCalendar> list)
        {
            var tokens = Tokens.Create(Define.Tweeter.ConsumerKey, Define.Tweeter.ConsumerSecret, Define.Tweeter.AccessToken, Define.Tweeter.AccessSecret);
            for (int i = 0; i < list.Count; i++)
            {
                //ツイート文を作成
                string tweetText = string.Empty;
                string publicTime = list[i].MyReleaseDate.ToString("HH:mm");

                string countryFlag = Tools.CountryNameToCountryFlag(list[i].CountryName);
                string title = string.Empty;
                title += countryFlag;

                //英国の文字は固定で削除する
                title = title.Replace(GreatBritain, string.Empty);

                //タイトルに金利の文字が無い場合国名をつける
                if (list[i].EventName.IndexOf(InterestRate) == -1)
                {
                    title += list[i].CountryName;
                }
                title += " " + list[i].EventName;

                tweetText += "@lukkou\r\n";
                tweetText += publicTime + "に" + title + "の発表\r\n";
                tweetText += "通貨　[" + list[i].CurrencyCode + "]\r\n";
                if (list[i].EventType != Define.EventTypeAnnouncement)
                {
                    if (!string.IsNullOrEmpty(list[i].ForecastValue))
                    {
                        tweetText += "予想値[" + list[i].ForecastValue + "]\r\n";
                    }
                    tweetText += "前回値[" + list[i].PreviousValue + "]\r\n";
                }
                tweetText += "\r\n" + Define.Mql5BaseUrl + list[i].LinkUrl;

                tokens.Statuses.Update(status => tweetText);
            }
        }
    }
}
