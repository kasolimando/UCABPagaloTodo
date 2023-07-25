
using Microsoft.AspNetCore.Http;

namespace UCABPagaloTodoMS.Infrastructure.Services.RabbitProducer
{
    public interface IRabbitProducer
    {
        public Task SendProductMessage(IFormFile file);
    }
}
