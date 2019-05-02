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
                string publicTime = list[i].MyReleaseDate.ToString("hh:mm");

                string countryFlag = Tools.CountryNameToCountryFlag(list[i].CountryName);

                tweetText += "@lukkou\r\n";
                tweetText += publicTime + "に["+ countryFlag + list[i].CountryName + list[i].EventName + "]の発表\r\n";
                tweetText += "通貨　[" + list[i].CurrencyCode + "]\r\n";
                if(list[i].EventType != Define.EventTypeAnnouncement)
                {
                    tweetText += "予想値[" + list[i].ForecastValue + "]\r\n";
                    tweetText += "前回値[" + list[i].PreviousValue + "]\r\n\r\n";
                }
                tweetText += Define.Mql5BaseUrl + list[i].LinkUrl;

                tokens.Statuses.Update(status => tweetText);
            }
        }
    }
}
