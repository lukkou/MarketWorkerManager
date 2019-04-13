using System;
using System.Diagnostics;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

using RealtimeIndexImporter.Common;
using RealtimeIndexImporter.Models;
using RealtimeIndexImporter.Context;

using System.Web.Http;

namespace RealtimeIndexImporter.Logic
{
    class IndexCalendarLogic
    {
        public DatabaseContext Db { get; set; }

        public HttpClient Client { get; set; }

        /// <summary>
        /// 現在の時刻から前後１分の経済指標（重要度HIGH）を取得
        /// </summary>
        /// <returns></returns>
        public List<IndexCalendar> GetIndexInfo()
        {
            List<IndexCalendar> result = new List<IndexCalendar>();

            DateTime before = DateTime.Now.AddMinutes(-1);
            DateTime after = DateTime.Now.AddMinutes(1);

            result = Db.IndexCalendars.Where(
                                        x => x.MyReleaseDate >= before &&
                                        x.MyReleaseDate <= after &&
                                        x.Importance == Define.ImportanceHigh).ToList();

            return result;
        }


        /// <summary>
        /// 指標データの内すでにつぶやき済みのデータを削除
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<IndexCalendar> RemoveAlreadyInfo(List<IndexCalendar> list)
        {
            List<IndexCalendar> result = list;

            foreach (IndexCalendar item in list)
            {
                NotificationFlg flgData = Db.NotificationFlgs.Where(x => x.GuidKey == item.GuidKey && x.TweetFlg == true).FirstOrDefault();
                if(flgData != null)
                {
                    result.Remove(item);
                }
            }

            return result;
        }

        /// <summary>
        /// 現在時刻の前後1時間の指標データを取得
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetMql5JsonAsync()
        {
            var results = string.Empty;

            DateTime weekMonday = GetFirstDayOfThisWeek(DateTime.Now);
            string fromDay = weekMonday.ToString("yyyy-MM-dd");
            string fromTime = "00:00:00";

            string toDay = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            string toTime = "23:59:59";

            StringBuilder url = new StringBuilder();
            url.Append(Define.Mql5ApiUrl);
            url.Append("date_mode=1");
            url.Append("&from=" + fromDay + "T" + fromTime);
            url.Append("&to=" + toDay + "T" + toTime);
            url.Append("&importance=15&currencies=127");

            using (HttpResponseMessage response = await Client.GetAsync(url.ToString()))
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    results = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    //THHPステータス 200以外
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
            }

            return results;
        }

        /// <summary>
        /// 指定のIdKeyの情報を取得
        /// </summary>
        /// <param name="responseBody"></param>
        /// <param name="idKey"></param>
        /// <returns></returns>
        public IndexCalendar ResponseBodyToEntityModel(string responseBody, string idKey)
        {
            List<IndexCalendar> results = new List<IndexCalendar>();

            Match jsonObj = Regex.Match(responseBody, Define.JsonRegular);
            if (jsonObj.Success)
            {
                results = JsonConvert.DeserializeObject<List<IndexCalendar>>(jsonObj.ToString());

                TimeSpan myUtc = GetMyTimeZone();
                foreach (IndexCalendar result in results)
                {
                    result.GuidKey = Guid.NewGuid();
                    result.ReleaseDateGmt = new DateTime(1970, 1, 1).AddTicks(result.ReleaseDate * 10000);
                    if (result.TimeMode == Define.TimeModeUTC)
                    {
                        result.MyReleaseDate = result.ReleaseDateGmt.Add(myUtc);
                    }
                    else
                    {
                        result.MyReleaseDate = result.ReleaseDateGmt;
                    }
                }
            }

            return results.Where(x => x.IdKey == idKey).FirstOrDefault();
        }

        /// <summary>
        /// 登録されているデータの公開フラグと実績値を更新
        /// </summary>
        /// <param name="myData"></param>
        /// <param name="webData"></param>
        /// <returns></returns>
        public IndexCalendar MergeMyDataToNowData(IndexCalendar myData, IndexCalendar webData)
        {
            IndexCalendar result = myData;
            result.Processed = webData.Processed;
            result.ActualValue = webData.ActualValue;

            return result;
        }

        /// <summary>
        /// Guidより登録されている指標データを取得
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public IndexCalendar GetMyIndexData(Guid guid)
        {
            return Db.IndexCalendars.Where(x => x.GuidKey == guid).FirstOrDefault();
        }

        /// <summary>
        /// 指標データを上書き
        /// </summary>
        /// <param name="data"></param>
        public void RegisteredIndexData(List<IndexCalendar> list)
        {
            foreach (IndexCalendar item in list)
            {
                Db.IndexCalendars.Attach(item);
                Db.Entry(item).State = EntityState.Modified;
                Db.SaveChanges();
            }
        }

        /// <summary>
        /// 指標公開ツイートフラグ更新
        /// </summary>
        /// <param name="list"></param>
        public void RegisteredUpdateFlg(List<IndexCalendar> list)
        {
            foreach (IndexCalendar item in list)
            {
                NotificationFlg updateInfo = new NotificationFlg
                {
                    GuidKey = item.GuidKey,
                    IdKey = item.IdKey,
                    TweetFlg = true
                };

                Db.NotificationFlgs.Attach(updateInfo);
                Db.Entry(updateInfo).State = EntityState.Modified;
                Db.SaveChanges();
            }
        }

        /// <summary>
        /// ローカルマシンのタイムゾーンを取得
        /// </summary>
        /// <returns></returns>
        private TimeSpan GetMyTimeZone()
        {
            return TimeZoneInfo.Local.BaseUtcOffset;
        }

        /// <summary>
        /// 週初め月曜の日付を取得
        /// </summary>
        /// <param name="nowDate"></param>
        /// <returns></returns>
        private DateTime GetFirstDayOfThisWeek(DateTime nowDate)
        {
            int deff = DayOfWeek.Monday - nowDate.DayOfWeek;
            if (deff > 0)
            {
                deff -= 7;
            }

            return nowDate.AddDays(deff);
        }
    }
}
