using System;
using System.Diagnostics;
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
		/// 現在時刻の前後1時間の指標データを取得
		/// </summary>
		/// <returns></returns>
		public async Task<string> GetMql5JsonAsync()
		{
			var results = string.Empty;

			string fromDay = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd");
			string fromTime = DateTime.Now.AddHours(-1).ToString("hh:mm:ss");

			string toDay = DateTime.Now.AddHours(1).ToString("yyyy-MM-dd");
			string toTime = DateTime.Now.AddHours(1).ToString("hh:mm:ss");

			StringBuilder url = new StringBuilder();
			url.Append(Define.Mql5ApiUrl);
			url.Append("date_mode=0");
			url.Append("&from=" + fromDay + " T" + fromTime);
			url.Append("&to=" + toDay + " T" + toTime);
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
		public IndexCalendar ResponseBodyToEntityModel(string responseBody,string idKey)
		{
			List<IndexCalendar> results = new List<IndexCalendar>();

			Match jsonObj = Regex.Match(responseBody, Define.JsonRegular);
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
		public IndexCalendar GetMyIndexData(string guid)
		{
			var myGuid = Guid.Parse(guid);
			return Db.IndexCalendars.Where(x => x.GuidKey == myGuid).FirstOrDefault();
		}

		/// <summary>
		/// 指標データを上書き
		/// </summary>
		/// <param name="data"></param>
		public void RegisteredIndexData(List<IndexCalendar> indexCalendars)
		{
			foreach (IndexCalendar indexCalendar in indexCalendars)
			{
				Db.IndexCalendars.Attach(indexCalendar);
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
	}
}
