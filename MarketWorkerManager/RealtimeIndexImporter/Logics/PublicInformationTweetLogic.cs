using System.Collections.Generic;
using CoreTweet;

using RealtimeIndexImporter.Utility;
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
                string countryFlag = Tools.CountryNameToCountryFlag(item.CountryName);

                tweetText += "ーーー" + item.EventName + "ーーー\r\n";
                tweetText += "国　　[" + countryFlag + item.CountryName + "]\r\n";
                tweetText += "今回値[" + item.ActualValue + "]\r\n";
                tweetText += "予想値[" + item.ForecastValue + "]\r\n";
                tweetText += "前回値[" + item.PreviousValue + "]\r\n\r\n";
                tweetText += Define.Mql5BaseUrl + item.LinkUrl;

                tokens.Statuses.Update(status => tweetText);
            }
        }
    }
}
