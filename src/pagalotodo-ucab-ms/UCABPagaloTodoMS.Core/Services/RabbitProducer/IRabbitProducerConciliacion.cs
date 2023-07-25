
using Microsoft.AspNetCore.Http;

namespace UCABPagaloTodoMS.Infrastructure.Services.RabbitProducer
{

    public interface IRabbitProducerConciliacion
    {
        Task SendProductMessageConciliacion(IFormFile file);
    }
}
