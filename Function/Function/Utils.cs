using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;

namespace Function
{
    internal class Utils
    {
        private static HttpClient httpClient;

        internal static HttpClient GetClient()
        {
            return httpClient ?? GetNewClient();
        }

        private static HttpClient GetNewClient()
        {
            var sp = ServicePointManager.FindServicePoint(new Uri(GetUrl()));
            sp.ConnectionLeaseTimeout = 60 * 1000;

            httpClient = new HttpClient();

            return httpClient;
        }

        internal static String GetUrl()
        {
            string team = Environment.GetEnvironmentVariable("team");
            string channel = Environment.GetEnvironmentVariable("channel");
            return $"https://outlook.office.com/webhook/{team}/IncomingWebhook/{channel}";
        }

        internal static StringContent GetStringContent(string myEventHubMessage)
        {
            var ehMessage = JsonConvert.DeserializeObject<Accelerometer>(myEventHubMessage);
            var reqData = new TeamHook { title = "Message from Function", text = "Grandma Fell Down : Ping the bot : Device ID: 04030201" };
            return new StringContent(JsonConvert.SerializeObject(reqData), Encoding.UTF8, "application/json");
        }
    }
}
