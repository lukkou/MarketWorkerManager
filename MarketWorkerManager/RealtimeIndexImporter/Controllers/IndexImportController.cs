using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using RealtimeIndexImporter.Common;
using RealtimeIndexImporter.Models;

namespace RealtimeIndexImporter.Controller
{
    class IndexImportController : BaseController
    {
        public void Run()
        {
            try
            {
                List<IndexCalendar> indexCalendarList = Logic.IndexCalendar.GetIndexInfo();
                indexCalendarList = Logic.IndexCalendar.RemoveAlreadyInfo(indexCalendarList);
                List<IndexCalendar> tweetData = new List<IndexCalendar>();

                if (indexCalendarList.Any())
                {
                    //５分先の時刻を取得
                    DateTime afterFiveMinutes = DateTime.Now.AddMinutes(5);

                    foreach (IndexCalendar item in indexCalendarList)
                    {
                        while (true)
                        {
                            var task = Logic.IndexCalendar.GetMql5JsonAsync();
                            task.Wait();

                            IndexCalendar webData = Logic.IndexCalendar.ResponseBodyToEntityModel(task.Result, item.IdKey);
                            if (webData == null)
                            {
                                //リクエストを投げ続けないために30秒待機
                                Thread.Sleep(300000);
                                continue;
                            }

                            IndexCalendar margeData = Logic.IndexCalendar.MergeMyDataToNowData(item, webData);
                            tweetData.Add(margeData);
                            break;
                        }
                    }

                    Logic.BeginTransaction();

                    //Tweet
                    Logic.PublicInformationTweet.PublicInformationTweet(tweetData);

                    //指標データを更新
                    Logic.IndexCalendar.RegisteredIndexData(tweetData);
                    Logic.IndexCalendar.RegisteredUpdateFlg(tweetData);

                    Logic.Commit();

                }else
                {
                    //データが無い場合は１分止める
                    Thread.Sleep(600000);
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
