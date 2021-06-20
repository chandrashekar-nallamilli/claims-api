using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Claims_Api.Models;
using Microsoft.Extensions.Options;

namespace Claims_Api.Services.Queues
{
    public class QueueService : IQueueService
    {
        private readonly ServiceBusClient _client;
        private readonly ServiceBusSender _sender;

        public QueueService(IOptions<AppSettings> appSettings)
        {
            _client = new ServiceBusClient(appSettings.Value.queue_connectionstring);
            _sender = _client.CreateSender(appSettings.Value.queue_name);
        }
        public async Task PublishMessage(string message)
        {
            try
            {
                await _sender.SendMessageAsync(new ServiceBusMessage(message));
            }
            finally
            {
                await _sender.DisposeAsync();
                await _client.DisposeAsync();
            } 

        }
    }
}