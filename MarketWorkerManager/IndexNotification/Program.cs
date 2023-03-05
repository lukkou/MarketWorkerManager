using System;
using IndexNotification.Controller;

namespace IndexNotification
{
    class Program
    {
        static void Main(string[] args)
        {
            using(TweetIndexNotificationController tweetIndexNotification = new TweetIndexNotificationController())
            {
                Console.WriteLine("指標発表30分前通知exe\r\nEをクリックすると止まります");
                while (true)
                {
                    tweetIndexNotification.Run();

                    if (Console.KeyAvailable)
                    {
                        string inputKey = Console.ReadKey().Key.ToString();
                        if (inputKey == "E")
                        {
                            return;
                        }
                    }
                }
            }
        }
    }
}
