using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreTweet;

using IndexNotification.Common;
using IndexNotification.Models;
using IndexNotification.Context;

namespace IndexNotification.Logics
{
    class NotificationTweetLogic
    {
        /// <summary>
        /// 指標発表一時間前情報をツイート
        /// </summary>
        /// <param name="list"></param>
        public void IndexNotificationTweet(List<IndexCalendar> list)
        {
            var tokens = Tokens.Create(Define.Tweeter.ConsumerKey, Define.Tweeter.ConsumerSecret, Define.Tweeter.AccessToken, Define.Tweeter.AccessSecret);
            for (int i = 0; i < list.Count; i++)
            {
                //ツイート分を作成
                string tweetText = "@lukkou\r\n";
                tweetText += "30分後に[" + list[i].EventName + "]の発表\r\n";
                tweetText += "前回値→[" + list[i].PreviousValue + "]\r\n";
                tweetText += "予想値→[" + list[i].ForecastValue + "]";

                tokens.Statuses.Update(status => tweetText);
            }
        }
    }
}
