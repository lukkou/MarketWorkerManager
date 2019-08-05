using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;

namespace ApiTest
{
    class ApiTestController
    {

        /// <summary>
        /// HTTPクライアントオブジェクト
        /// ※usingを使用した場合、using毎回ソケットがオープンされ
        /// クローズしてもTIME_WAIT後ちょっとの間解放されないので
        /// 一つのオブジェクトを使いまわしLogicContextが解放された
        /// タイミングでクローズする。
        /// </summary>
        private HttpClient client { get; set; }


        /// <summary>
        /// コンストラクター
        /// </summary>
        public ApiTestController()
        {
            client = new HttpClient();
        }

        public void Run()
        {
            try
            {
                var task = GetMql5JsonAsync();
                task.Wait();

                Console.WriteLine(task.Result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                if (e.InnerException != null)
                {
                    Console.WriteLine(e.InnerException.Message);
                    Console.WriteLine(e.InnerException.StackTrace);
                }
            }

            if(client != null)
            {
                client.Dispose();
            }
        }

        private async Task<string> GetMql5JsonAsync()
        {
            var results = string.Empty;
            string url = string.Empty;
            url = "https://www.mql5.com/ja/economic-calendar/content?date_mode=1&from=2019-08-05T00%3A00%3A00&to=2019-08-11T23%3A59%3A59&importance=8&currencies=127";

            using (HttpResponseMessage response = await client.GetAsync(url))
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    results = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    //THHPステータス 200以外
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
            }

            return results;
        }
    }
}
