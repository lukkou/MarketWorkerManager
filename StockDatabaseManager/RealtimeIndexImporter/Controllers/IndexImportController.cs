using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CoreTweet;

using RealtimeIndexImporter.Common;
using RealtimeIndexImporter.Models;

namespace RealtimeIndexImporter.Controllers
{
	class IndexImportController:BaseController
	{
		public void Run(string[] guidList)
		{
			try
			{
				List<IndexCalendar> indexCalendarList = new List<IndexCalendar>();

				DateTime afterFiveMinutes = DateTime.Now.AddMinutes(5);

				//必要な指標が全て揃うか5分経つまでデータを取得し続ける
				while (true)
				{
					for (int i = 0; i <= guidList.Length - 1; i++)
					{
						if (!indexCalendarList.Any(x => x.GuidKey == Guid.Parse(guidList[i])))
						{
							var task = Logic.IndexCalendar.GetMql5JsonAsync();
							task.Wait();

							IndexCalendar myData = Logic.IndexCalendar.GetMyIndexData(guidList[i]);
							IndexCalendar webData = Logic.IndexCalendar.ResponseBodyToEntityModel(task.Result, myData.IdKey);
							if(webData == null)
							{
								continue;
							}

							IndexCalendar indexData = Logic.IndexCalendar.MergeMyDataToNowData(myData, webData);

							indexCalendarList.Add(indexData);
						}
					}

					if (indexCalendarList.Count == guidList.Length || afterFiveMinutes <= DateTime.Now)
					{
						break;
					}

					//リクエストを投げ続けないために30秒待機
					Thread.Sleep(300000);
				}

				if (indexCalendarList.Count > 0)
				{
					Logic.BeginTransaction();
					Logic.IndexCalendar.RegisteredIndexData(indexCalendarList);

					//Twitterに投稿
					//var tokens = Tokens.Create(Define.Tweeter.ConsumerKey,Define.Tweeter.ConsumerSecret,Define.Tweeter.AccessToken,Define.Tweeter.AccessSecret);
					//for(int i = 0;i < indexCalendarList.Count; i++)
					//{
					//	string tweetText = string.Empty;
					//	tweetText = tweetText + indexCalendarList[i].EventName + " 今回値→[" + indexCalendarList[i].ActualValue + "]  予想→[" + indexCalendarList[i].ForecastValue + "]  前回値→[" + indexCalendarList[i].PreviousValue + "]";
					//	tokens.Statuses.Update(status => tweetText);
					//}
					Logic.Commit();
				}
			}
			catch (Exception e)
			{
				Logic.Rollback();
				Log.Logger.Error(e.Message);
				Log.Logger.Error(e.StackTrace);
				if (e.InnerException != null)
				{
					Log.Logger.Error(e.InnerException.Message);
					Log.Logger.Error(e.InnerException.StackTrace);
				}
			}
		}
	}
}
