using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IndexNotification.Common;
using IndexNotification.Models;

namespace IndexNotification.Controllers
{
    class TweetIndexNotificationController : BaseController
    {
        public void Run()
        {
            try
            {
                Logic.BeginTransaction();

                List<IndexCalendar> indexCalendarList = Logic.IndexCalendar.GetIndexCalendarInfo();
                Logic.NotificationTweet.IndexNotificationTweet(indexCalendarList);

                Logic.IndexCalendarLogic.AddTweetFlg(indexCalendarList);
                Logic.Commit();
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
