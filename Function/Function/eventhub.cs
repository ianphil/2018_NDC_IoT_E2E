using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Function
{
    public static class eventhub
    {
        public static HttpClient _client = new HttpClient();

        [FunctionName("eventhub")]
        public static async Task Run(
            [EventHubTrigger("ndccontosoeh", Connection = "EventHubConnection")]string myEventHubMessage,
            TraceWriter log)
        {
            log.Info($"C# Event Hub trigger function processed a message: {myEventHubMessage}");

            var ehMessage = JsonConvert.DeserializeObject<Accelerometer>(myEventHubMessage);

            var reqData = new TeamHook { title = "Message from Function", text = "Grandma Fell Down : Ping the bot : Device ID: 04030201" };
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(reqData), Encoding.UTF8, "application/json");
            string url = "https://outlook.office.com/webhook/e05e982d-7173-431a-b48d-97026b681d8a@72f988bf-86f1-41af-91ab-2d7cd011db47/IncomingWebhook/5b3ca82bc03c4c32b39afbe8fb4258a4/2c8df394-55f5-4dae-b2d3-27b3617aa6c3";

            try
            {
                // leave until eh retention clears test data
                if (ehMessage.Y == 0.1)
                {
                    var data = await _client.PostAsync(url, stringContent);

                    if (!data.IsSuccessStatusCode)
                    {
                        throw new System.Exception("Did not complete the call");
                    }
                }

            }
            catch (System.Exception up)
            {
                throw up;
            }
        }
    }

    public class TeamHook
    {
        public string title { get; set; }
        public string text { get; set; }
    }

    public class Accelerometer
    {
        public string Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
