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

                //memo lukkou: 文字列連結内では?:演算子が使えない；。；
                string forecastValueaa = string.IsNullOrEmpty(item.ForecastValue) ? "--" : "item.ForecastValue";

                tweetText += "ーーー" + countryFlag + item.CountryName + item.EventName + "ーーー\r\n";
                tweetText += "通貨　　[" + item.CurrencyCode + "]\r\n";
                tweetText += "今回値[" + item.ActualValue + "]\r\n";
                tweetText += "予想値[" + forecastValueaa + "]\r\n";
                tweetText += "前回値[" + item.PreviousValue + "]\r\n\r\n";
                tweetText += Define.Mql5BaseUrl + item.LinkUrl;

                tokens.Statuses.Update(status => tweetText);
            }
        }
    }
}
