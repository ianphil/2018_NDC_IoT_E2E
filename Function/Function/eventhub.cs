namespace Function
{
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Host;
    using Microsoft.Azure.WebJobs.ServiceBus;
    using System.Threading.Tasks;
    using System.Net.Http;

    public static class eventhub
    {
        public static HttpClient _client = Utils.GetClient();

        [FunctionName("eventhub")]
        public static async Task Run(
            [EventHubTrigger("ndccontosoeh", Connection = "EventHubConnection", ConsumerGroup = "function")]string myEventHubMessage,
            TraceWriter log)
        {
            log.Info($"C# Event Hub trigger function processed a message: {myEventHubMessage}");

            var url = Utils.GetUrl();
            var stringContent = Utils.GetStringContent(myEventHubMessage);

            try
            {
                var res = await _client.PostAsync(url, stringContent);

                if (!res.IsSuccessStatusCode)
                {
                    throw new System.Exception("Did not complete the call");
                }
                else
                {
                    log.Info($"Request sent to Teams Channel, Status: {res.StatusCode}");
                }

            }
            catch (System.Exception up)
            {
                throw up;
            }
        }
    }
}
