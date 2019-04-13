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
                Console.WriteLine("Eをクリックすると止まります");
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
