using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using System.Text;

namespace apiinvestimentos.Service
{
    // service que gera os eventhub no eventhub azure
    public class EventHubService
    {
        private readonly string _connectionString;
        private readonly string _eventHubName;

        public EventHubService(IConfiguration config)
        {
            _connectionString = config["EventHub:ConnectionString"];
            _eventHubName = config["EventHub:Name"];
        }

        public async Task EnviarMensagem(string mensagem)
        {
            await using var producerClient = new EventHubProducerClient(_connectionString, _eventHubName);

            using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();
            eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(mensagem)));

            await producerClient.SendAsync(eventBatch);
        }
    }

}
