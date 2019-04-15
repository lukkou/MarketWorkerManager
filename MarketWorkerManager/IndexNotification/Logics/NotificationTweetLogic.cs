using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreTweet;

using IndexNotification.Common;
using IndexNotification.Models;
using IndexNotification.Context;

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
                tweetText += "@lukkou";
                tweetText += "ーーー30分後に[" + list[i].EventName + "]の発表ーーー\r\n";
                tweetText += "通貨　[" + list[i].CurrencyCode + "]\r\n";
                tweetText += "予想値[" + list[i].ForecastValue + "]\r\n";
                tweetText += "前回値[" + list[i].PreviousValue + "]";

                tokens.Statuses.Update(status => tweetText);
            }
        }
    }
}
