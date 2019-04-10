using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

using MarketWorkerManager.Common;
using MarketWorkerManager.Models;
using MarketWorkerManager.Context;


namespace MarketWorkerManager.Logic
{
    class IndexCalendarLogic
    {
        public DatabaseContext Db { get; set; }

        public HttpClient Client { get; set; }

        /// <summary>
        /// 指標カレンダーデータを取得
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public async Task<string> GetMql5JsonAsync(string from, string to)
        {
            var results = string.Empty;

            StringBuilder url = new StringBuilder();
            url.Append(Define.Index.Mql5ApiUrl);
            url.Append("date_mode=0");
            url.Append("&from=" + from + "T00:00:00");
            url.Append("&to=" + to + "T23:59:59");
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
        /// 指標Jsonをエンティティモデルに変換
        /// </summary>
        /// <param name="responseBody"></param>
        /// <returns></returns>
        public List<IndexCalendar> ResponseBodyToEntityModel(string responseBody)
        {
            List<IndexCalendar> results = new List<IndexCalendar>();

            //BodyよりJsonのみ取得
            Match jsonObj = Regex.Match(responseBody, Define.Index.JsonRegular);
            results = JsonConvert.DeserializeObject<List<IndexCalendar>>(jsonObj.ToString());

            TimeSpan myUtc = GetMyTimeZone();
            foreach (IndexCalendar result in results)
            {
                result.GuidKey = Guid.NewGuid();
                result.ReleaseDateGmt = new DateTime(1970, 1, 1).AddTicks(result.ReleaseDate * 10000);
                if (result.TimeMode == Define.Index.TimeModeUTC)
                {
                    result.MyReleaseDate = result.ReleaseDateGmt.Add(myUtc);
                }
                else
                {
                    result.MyReleaseDate = result.ReleaseDateGmt;
                }
            }

            return results;
        }

        /// <summary>
        /// Mysqlより指定範囲の経済指標データを取得
        /// </summary>
        /// <param name="from">範囲日From</param>
        /// <param name="to">範囲日To</param>
        /// <returns></returns>
        public List<IndexCalendar> GetRegisteredIndex(string from, string to)
        {
            var fromData = DateTime.Parse(from + " 00:00:00");
            var toData = DateTime.Parse(to + " 23:59:59");
            var utcFromTime = TimeZoneInfo.ConvertTimeToUtc(fromData);
            var utcToTime = TimeZoneInfo.ConvertTimeToUtc(toData);

            return Db.IndexCalendars.Where(x => x.ReleaseDateGmt >= utcFromTime && x.ReleaseDateGmt <= utcToTime).ToList();
        }

        /// <summary>
        /// 経済指標オブジェクトより指定範囲のデータを取得
        /// </summary>
        /// <param name="data"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public List<IndexCalendar> GetSpecifiedRangeIndex(List<IndexCalendar> data, string from, string to)
        {
            var fromData = DateTime.Parse(from + " 00:00:00");
            var toData = DateTime.Parse(to + " 23:59:59");
            var utcFromTime = TimeZoneInfo.ConvertTimeToUtc(fromData);
            var utcToTime = TimeZoneInfo.ConvertTimeToUtc(toData);

            return data.Where(x => x.ReleaseDateGmt >= utcFromTime && x.ReleaseDateGmt <= utcToTime).ToList();
        }

        /// <summary>
        /// 登録データとwebの最新データを比較し差分データを作成
        /// </summary>
        /// <param name="Registered"></param>
        /// <param name="web"></param>
        /// <returns></returns>
        public List<IndexCalendar> CompareNewnessIndexData(List<IndexCalendar> dbData, List<IndexCalendar> webData)
        {
            List<IndexCalendar> results = new List<IndexCalendar>();

            foreach (IndexCalendar webDetail in webData)
            {
                //Idと名前で一致させる（同じ月に同じイベント名の指標が2回は無いはず…）
                var dbDetail = dbData.Where(x => x.IdKey == webDetail.IdKey && x.EventName == webDetail.EventName).FirstOrDefault();

                if (dbDetail != null)
                {
                    if (dbDetail.Processed == Define.Index.ProcessedOff && webDetail.Processed == Define.Index.ProcessedOn)
                    {
                        //指標が公開された場合
                        dbDetail.ForecastValue = webDetail.ForecastValue;
                        dbDetail.ActualValue = webDetail.ActualValue;
                        dbDetail.Processed = Define.Index.ProcessedOn;

                        results.Add(dbDetail);
                        Log.Logger.Info(dbDetail.GuidKey.ToString() + "の指標公開");
                    }
                    else if (string.IsNullOrEmpty(dbDetail.OldPreviousValue) && !string.IsNullOrEmpty(webDetail.OldPreviousValue))
                    {
                        //過去データが更新された場合
                        dbDetail.OldPreviousValue = webDetail.OldPreviousValue;
                        dbDetail.PreviousValue = webDetail.PreviousValue;

                        results.Add(dbDetail);
                        Log.Logger.Info(dbDetail.GuidKey.ToString() + "の過去データ更新");
                    }
                    else if (string.IsNullOrEmpty(dbDetail.ForecastValue) && !string.IsNullOrEmpty(webDetail.ForecastValue))
                    {
                        //予想データが初期では登録されていなかった場合
                        dbDetail.ForecastValue = webDetail.ForecastValue;

                        results.Add(dbDetail);
                        Log.Logger.Info(dbDetail.GuidKey.ToString() + "の予測データ登録");
                    }
                }
                else
                {
                    //webのデータがdbに存在しない場合は新規登録を行う
                    //IndexCalendar newRow = new IndexCalendar();
                    //newRow.GuidKey = Guid.NewGuid();
                    //newRow.IdKey = webDetail.IdKey;
                    //newRow.ReleaseDate = webDetail.ReleaseDate;
                    //webDetail.ReleaseDateGmt = new DateTime(1970, 1, 1).AddTicks(webDetail.ReleaseDate * 10000);
                    //if (webDetail.TimeMode == Define.Index.TimeModeUTC)
                    //{
                    //    newRow.MyReleaseDate = webDetail.ReleaseDateGmt.Add(GetMyTimeZone());
                    //}
                    //else
                    //{
                    //    newRow.MyReleaseDate = webDetail.ReleaseDateGmt;
                    //}
                    //newRow.TimeMode = webDetail.TimeMode;
                    //newRow.CurrencyCode = webDetail.CurrencyCode;
                    //newRow.EventName = webDetail.EventName;
                    //newRow.EventType = webDetail.EventType;
                    //newRow.Importance = webDetail.Importance;
                    //newRow.Processed = webDetail.Processed;
                    //newRow.ActualValue = webDetail.ActualValue;
                    //newRow.ForecastValue = webDetail.ForecastValue;
                    //newRow.PreviousValue = webDetail.ForecastValue;
                    //newRow.OldPreviousValue = webDetail.OldPreviousValue;
                    //newRow.LinkUrl = webDetail.LinkUrl;

                    //results.Add(newRow);
                }
            }

            return results;
        }

        public Tuple<List<IndexCalendar>, List<IndexCalendar>> DuplicateDataExtraction()
        {
            List<IndexCalendar> nextIndexData = new List<IndexCalendar>();
            List<IndexCalendar> nowIndexData = new List<IndexCalendar>();

            return new Tuple<List<IndexCalendar>, List<IndexCalendar>>(nowIndexData, nextIndexData);
        }

        /// <summary>
        /// 指標データを登録
        /// </summary>
        /// <param name="data"></param>
        public void RegisteredIndexData(List<IndexCalendar> indexCalendars, bool addFlg = false)
        {
            if (addFlg)
            {
                Db.IndexCalendars.AddRange(indexCalendars);
                Db.SaveChanges();
            }
            else
            {
                foreach (IndexCalendar indexCalendar in indexCalendars)
                {
                    Db.IndexCalendars.Attach(indexCalendar);
                    Db.Entry(indexCalendar).State = EntityState.Modified;
                    Db.SaveChanges();
                }
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
    }
}
