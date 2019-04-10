using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace IndexNotification.Controllers
{
    class TweetIndexNotificationController : BaseController
    {
        public void Run()
        {
            try
            {

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
