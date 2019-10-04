using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
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
            var baseAddress = new Uri("https://www.mql5.com/ja/economic-calendar");
            var cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler() { CookieContainer = cookieContainer };

            client = new HttpClient(handler) { BaseAddress = baseAddress };
        }

        public void Run()
        {
            try
            {
                GetPcInfo();

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

            if (client != null)
            {
                client.Dispose();
            }
        }

        private void GetPcInfo()
        {
            // アダプタリストを取得する
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface adapter in adapters)
            {

                // ネットワーク接続状態が UP のアダプタのみ表示 
                if (adapter.OperationalStatus == OperationalStatus.Up)
                {
                    Console.WriteLine("//-----------------------------------------------------------------------------");
                    Console.WriteLine(adapter.Name);         // アダプタ名
                    Console.WriteLine(adapter.Description);  // アダプタの説明

                    IPInterfaceProperties ip_prop = adapter.GetIPProperties();

                    // ユニキャスト IP アドレスの取得
                    UnicastIPAddressInformationCollection addrs = ip_prop.UnicastAddresses;
                    foreach (UnicastIPAddressInformation addr in addrs)
                    {
                        Console.WriteLine(addr.Address.ToString());

                        // メモ
                        //-----------------------------------------------------------------------------
                        // IPv4 : addr.Address.AddressFamily = Net.Sockets.AddressFamily.InterNetwork 
                        // IPv6 : addr.Address.AddressFamily = Net.Sockets.AddressFamily.InterNetwork6 
                    }

                    // ゲートウェイ IP アドレスの取得
                    GatewayIPAddressInformationCollection gates = ip_prop.GatewayAddresses;
                    foreach (GatewayIPAddressInformation gate in gates)
                    {
                        Console.WriteLine("ゲートウェイ情報：" + gate.Address.ToString());
                    }

                    // 物理（MAC）アドレスの取得
                    PhysicalAddress phy = adapter.GetPhysicalAddress();
                    Console.WriteLine("MACアドレス：" + phy.ToString());
                }
            }
        }


        private async Task<string> GetPostMQL5()
        {
            


            var results = string.Empty;
            string url = string.Empty;

            //POSTパラメーター作成
            var parameters = new Dictionary<string, string>()
            {
                { "date_mode", "4" },
                { "from", "2019-09-01T00:00:00" },
                { "to", "2019-09-30T23:59:59" },
                { "importance", "15" },
                { "currencies", "127" },
            };
            var content = new FormUrlEncodedContent(parameters);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Cookie", "lang=ja; uniq=5046563697952439240; _fz_fvdt=1568328136; _fz_uniq=5046563697952439240; sid=yhvbyfaqu10uunf20x42kfhh; cookie_accept=1; utm_campaign=ja.news.calendar.10.reasons; utm_source=www.metatrader4.com; _fz_ssn=1569738437144968219");

            using (var response = await client.PostAsync("/ja/economic-calendar/content", content))
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
