using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

using IndexNotification.Common;
using IndexNotification.Models;

namespace IndexNotification.Controller
{
    class TweetIndexNotificationController : BaseController
    {
        public void Run()
        {
            try
            {
                List<IndexCalendar> indexCalendarList = Logic.IndexCalendar.GetIndexCalendarInfo();

                if (indexCalendarList.Any())
                {
                    Logic.BeginTransaction();

                    //ツイッターに指標発表前のお知らせ
                    Logic.NotificationTweet.IndexNotificationTweet(indexCalendarList);

                    //お知らせレコード登録
                    Logic.IndexCalendar.AddTweetFlg(indexCalendarList);
                    Logic.Commit();
                }else
                {
                    //データが無い場合は30秒止める
                    Thread.Sleep(30000);
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
