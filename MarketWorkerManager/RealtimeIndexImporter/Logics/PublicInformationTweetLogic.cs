using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreTweet;

using RealtimeIndexImporter.Common;
using RealtimeIndexImporter.Models;

namespace RealtimeIndexImporter.Logic
{
    class PublicInformationTweetLogic
    {
        /// <summary>
        /// 公開された指標をツイート
        /// </summary>
        /// <param name="list"></param>
        public void PublicInformationTweet(List<IndexCalendar> list)
        {
            var tokens = Tokens.Create(Define.Tweeter.ConsumerKey, Define.Tweeter.ConsumerSecret, Define.Tweeter.AccessToken, Define.Tweeter.AccessSecret);
            foreach(IndexCalendar item in list)
            {
                string tweetText = string.Empty;
                tweetText += "ーーー" + item.EventName + "ーーー\r\n";
                tweetText += "通貨　[" + item.CurrencyCode + "]\r\n";
                tweetText += "今回値[" + item.ActualValue + "]\r\n";
                tweetText += "予想値[" + item.ForecastValue + "]\r\n";
                tweetText += "前回値[" + item.PreviousValue + "]\r\n";

                tokens.Statuses.Update(status => tweetText);
            }
        }
    }
}
