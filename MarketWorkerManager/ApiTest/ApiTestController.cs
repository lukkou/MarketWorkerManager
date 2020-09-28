﻿using System;
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
        private CookieContainer cookieContainer = null;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public ApiTestController()
        {
            var baseAddress = new Uri("https://www.mql5.com");
            cookieContainer = new CookieContainer();

            client = new HttpClient() { BaseAddress = baseAddress };
        }

        public void Run()
        {
            try
            {
                GetPcInfo();

                //var task = GetPostMQL5();
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
            string url = "https://www.mql5.com/ja/economic-calendar";

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
            //HttpWebResponse res = (HttpWebResponse)request.GetResponse();
            //request.Headers.Add("Cookie", "lang=ja; uniq=5046563697952439240; _fz_fvdt=1568328136; _fz_uniq=5046563697952439240; sid=yhvbyfaqu10uunf20x42kfhh; cookie_accept=1; utm_campaign=ja.news.calendar.10.reasons; utm_source=www.metatrader4.com; _fz_ssn=1569738437144968219");

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
            string url = "https://www.mql5.com/ja/economic-calendar";
            //string url = "https://www.mql5.com";
            //url = "https://www.mql5.com/ja/economic-calendar/content?date_mode=1&from=2019-08-05T00%3A00%3A00&to=2019-08-11T23%3A59%3A59&importance=8&currencies=127";


            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            request.Headers.Add("accept-encoding", "gzip, deflate, br");
            request.Headers.Add("accept-language", "ja,en;q=0.9,en-GB;q=0.8,en-US;q=0.7");
            request.Headers.Add("sec-fetch-dest", "document");
            request.Headers.Add("sec-fetch-mode", "navigate");
            request.Headers.Add("sec-fetch-site", "none");
            request.Headers.Add("upgrade-insecure-requests", "1");
            request.Headers.Add("user-agent", "wwwMozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36 Edg/85.0.564.63");

            using (HttpResponseMessage response = await client.SendAsync(request))
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
