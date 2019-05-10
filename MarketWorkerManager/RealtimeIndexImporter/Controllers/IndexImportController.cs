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
                            Log.Logger.Info("データの取得開始 IdKey:" + item.IdKey);
                            var task = Logic.IndexCalendar.GetMql5JsonAsync();
                            task.Wait();

                            IndexCalendar webData = Logic.IndexCalendar.ResponseBodyToEntityModel(task.Result, item.IdKey);

                            //5分データが取れなかったらツイートをあきらめる
                            if(DateTime.Compare(DateTime.Now,afterFiveMinutes) == 1)
                            {
                                Log.Logger.Info("5分データを取得できませんでした IdKey:" + item.IdKey);
                                break;
                            }

                            if (webData == null || string.IsNullOrEmpty(webData.ActualValue))
                            {
                                //リクエストを投げ続けないために10秒待機
                                Thread.Sleep(10000);
                                continue;
                            }

                            IndexCalendar margeData = Logic.IndexCalendar.MergeMyDataToNowData(item, webData);
                            tweetData.Add(margeData);
                            Log.Logger.Info("データの取得完了 IdKey:" + item.IdKey);
                            break;
                        }
                    }
                    //Tweet
                    Logic.PublicInformationTweet.PublicInformationTweet(tweetData);

                    Logic.BeginTransaction();

                    //指標データを更新
                    Logic.IndexCalendar.RegisteredIndexData(tweetData);
                    Logic.IndexCalendar.RegisteredUpdateFlg(tweetData);

                    Logic.Commit();

                }else
                {
                    //データが無い場合は60秒止める
                    Thread.Sleep(60000);
                }
            }
            catch (Exception e)
            {
                Logic.Rollback();
                Log.Logger.Error(e.Message);
                Log.Logger.Error(e.StackTrace);
                Console.WriteLine(e.Message);
                if (e.InnerException != null)
                {
                    Log.Logger.Error(e.InnerException.Message);
                    Log.Logger.Error(e.InnerException.StackTrace);
                }
            }
        }
    }
}
