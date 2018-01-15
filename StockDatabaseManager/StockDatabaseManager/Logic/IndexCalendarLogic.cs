using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StockDatabaseManager.Common;
using StockDatabaseManager.Models;
using StockDatabaseManager.Context;

namespace StockDatabaseManager.Logic
{
	class IndexCalendarLogic
	{
		private DatabaseContext db { get; set; }

		public IndexCalendarLogic(DatabaseContext context)
		{
			db = context;
		}

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
			url.Append(Define.Index.Mql5_ApiUrl);
			url.Append("date_mode=0");
			url.Append("&from=" + from + "T00:00:00");
			url.Append("&to=" + to + "T23:59:59");
			url.Append("&importance=15&currencies=127");

			using (HttpClient client = new HttpClient())
			using (HttpResponseMessage response = await client.GetAsync(url.ToString()).ConfigureAwait(false))
			{
				if (response.StatusCode == HttpStatusCode.OK)
				{
					results = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
				}
				else
				{
					//THHPステータス 200以外
				}
			}

			return results;
		}

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
		/// 指定範囲の経済指標データを取得
		/// </summary>
		/// <param name="from">範囲日From</param>
		/// <param name="to">範囲日To</param>
		/// <returns></returns>
		public List<IndexCalendar> GetRegisteredIndexData(string from, string to)
		{
			return db.IndexCalendar.Where(x => x.MyReleaseDate >= DateTime.Parse(from) && x.MyReleaseDate <= DateTime.Parse(to)).ToList();
		}

		/// <summary>
		/// 登録データとwebの最新データを比較し差分データを作成
		/// </summary>
		/// <param name="Registered"></param>
		/// <param name="web"></param>
		/// <returns></returns>
		public List<IndexCalendar> CompareNewnessIndexData(List<IndexCalendar> dbIndexes, List<IndexCalendar> webIndexes)
		{
			List<IndexCalendar> results = new List<IndexCalendar>();

			foreach (IndexCalendar webIndex in webIndexes)
			{
				//Idと名前で一致させる（同じ月に同じイベント名の指標が2回は無いはず…）
				var dbIndex = dbIndexes.Where(x => x.IdKey == webIndex.IdKey && x.EventName == webIndex.EventName).FirstOrDefault();

				if (dbIndex != null)
				{
					if (dbIndex.Processed == Define.Index.ProcessedOff && dbIndex.Processed == Define.Index.ProcessedOn)
					{
						//指標が公開された場合
						dbIndex.ForecastValue = webIndex.ForecastValue;
						dbIndex.ActualValue = webIndex.ActualValue;

						results.Add(dbIndex);
					}
					else if (string.IsNullOrEmpty(dbIndex.OldPreviousValue) && !string.IsNullOrEmpty(webIndex.OldPreviousValue))
					{
						//過去データが更新された場合
						dbIndex.OldPreviousValue = webIndex.OldPreviousValue;
						dbIndex.PreviousValue = webIndex.PreviousValue;

						results.Add(dbIndex);
					}
					else if (string.IsNullOrEmpty(dbIndex.ForecastValue) && !string.IsNullOrEmpty(webIndex.ForecastValue))
					{
						//予想データが初期では登録されていなかった場合
						dbIndex.ForecastValue = webIndex.ForecastValue;

						results.Add(dbIndex);
					}
				}
				else
				{
					//webのデータがdbに存在しない場合は新規登録を行う
					IndexCalendar newRow = new IndexCalendar();
					newRow.GuidKey = Guid.NewGuid();
					newRow.IdKey = webIndex.IdKey;
					newRow.ReleaseDate = webIndex.ReleaseDate;
					webIndex.ReleaseDateGmt = new DateTime(1970, 1, 1).AddTicks(webIndex.ReleaseDate * 10000);
					if (webIndex.TimeMode == Define.Index.TimeModeUTC)
					{
						newRow.MyReleaseDate = webIndex.ReleaseDateGmt.Add(GetMyTimeZone());
					}
					else
					{
						newRow.MyReleaseDate = webIndex.ReleaseDateGmt;
					}
					newRow.TimeMode = webIndex.TimeMode;
					newRow.CurrencyCode = webIndex.CurrencyCode;
					newRow.EventName = webIndex.EventName;
					newRow.EventType = webIndex.EventType;
					newRow.Importance = webIndex.Importance;
					newRow.Processed = webIndex.Processed;
					newRow.ActualValue = webIndex.ActualValue;
					newRow.ForecastValue = webIndex.ForecastValue;
					newRow.PreviousValue = webIndex.ForecastValue;
					newRow.OldPreviousValue = webIndex.OldPreviousValue;
					newRow.LinkUrl = webIndex.LinkUrl;

					results.Add(newRow);
				}
			}

			return results;
		}

		/// <summary>
		/// 指標データを登録
		/// </summary>
		/// <param name="data"></param>
		public void RegisteredIndexData(List<IndexCalendar> data)
		{
			db.IndexCalendar.AddRange(data);
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
