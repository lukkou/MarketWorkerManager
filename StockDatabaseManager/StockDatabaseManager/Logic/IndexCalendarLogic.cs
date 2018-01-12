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
		public async Task<List<IndexCalendar>> GetIndexJsonToModelAsync(string from, string to)
		{
			List<IndexCalendar> results = new List<IndexCalendar>();

			StringBuilder url = new StringBuilder();
			url.Append(Define.Index.Mql5_ApiUrl);
			url.Append("date_mode=0");
			url.Append("&from=" + from + "T00:00:00");
			url.Append("&to=" + to + "T23:59:59");
			url.Append("&importance=15&currencies=127");

			using (HttpClient client = new HttpClient())
			using (HttpResponseMessage response = await client.GetAsync(url.ToString()))
			{
				if (response.StatusCode == HttpStatusCode.OK)
				{
					var responseBody = await response.Content.ReadAsStringAsync();

					//BodyよりJsonのみ取得
					Match jsonObj = Regex.Match(responseBody, Define.Index.JsonRegular);

					results = JsonConvert.DeserializeObject<List<IndexCalendar>>(jsonObj.ToString());

					//ローカルマシンのタイムゾーン取得
					TimeSpan myUtc = TimeZoneInfo.Local.BaseUtcOffset;
					foreach (IndexCalendar result in results)
					{
						result.GuidKey = Guid.NewGuid();
						result.ReleaseDateGmt = new DateTime(1970, 1, 1).AddTicks(long.Parse(result.ReleaseDate) * 10000);
						if (result.TimeMode == Define.Index.TimeModeUTC)
						{
							result.MyReleaseDate = result.ReleaseDateGmt.Add(myUtc);
						}
						else
						{
							result.MyReleaseDate = result.ReleaseDateGmt;
						}
					}
				}
				else
				{
					//THHPステータス 200以外
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
		public List<IndexCalendar> GetRegisteredIndexData(string from,string to)
		{
			return db.IndexCalendar.Where(x => x.MyReleaseDate >= DateTime.Parse(from) && x.MyReleaseDate <= DateTime.Parse(to)).ToList();
		}

		/// <summary>
		/// 登録データとwebの最新データを比較
		/// </summary>
		/// <param name="Registered"></param>
		/// <param name="web"></param>
		/// <returns></returns>
		public List<IndexCalendar> CompareNewnessIndexData(List<IndexCalendar> dbIndexes, List<IndexCalendar> webIndexes)
		{
			List<IndexCalendar> results = new List<IndexCalendar>();

			foreach(IndexCalendar webIndex in webIndexes)
			{
				var dbIndex = dbIndexes.Where(x => x.IdKey == webIndex.IdKey).FirstOrDefault();

				if (dbIndex != null)
				{
					if(dbIndex.Processed == Define.Index.ProcessedOff && dbIndex.Processed == Define.Index.ProcessedOn)
					{
						//指標が公開された場合
						dbIndex.ForecastValue = webIndex.ForecastValue;
						dbIndex.ActualValue = webIndex.ActualValue;
					}else if ()
					{
						//過去
					}
				}
				else
				{
					//webのデータがdbに存在しない場合は新規登録を行う
				}
			}
		}
	
		/// <summary>
		/// 指標データを登録
		/// </summary>
		/// <param name="data"></param>
		public void RegisteredIndexData(List<IndexCalendar> data)
		{
			db.IndexCalendar.AddRange(data);
		}
	}
}
