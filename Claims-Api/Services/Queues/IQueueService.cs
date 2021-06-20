using System.Threading.Tasks;

namespace Claims_Api.Services.Queues
{
    public interface IQueueService
    {
        Task PublishMessage(string message);
    }
}