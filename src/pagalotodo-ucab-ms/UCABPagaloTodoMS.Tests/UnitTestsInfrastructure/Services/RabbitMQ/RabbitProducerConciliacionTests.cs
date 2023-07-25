using Microsoft.AspNetCore.Http;
using Moq;
using RabbitMQ.Client;
using System.Text;
using UCABPagaloTodoMS.Application.BusinessValidation;
using UCABPagaloTodoMS.Core.Services.Firebase;
using UCABPagaloTodoMS.Infrastructure.Services.RabbitProducer;
using UCABPagaloTodoMS.Tests.MockData;
using Xunit;

namespace UCABPagaloTodoMS.Tests.UnitTestsInfrastructure.Services
{
    public class RabbitProducerConciliacionTests
    {
        private readonly RabbitProducerConciliacion rabbitProducer;
        private IFormFile file = BuildDataContextFaker.BuildArchivo();

        public RabbitProducerConciliacionTests()
        {
            rabbitProducer = new();
        }

        [Fact]
        public async Task TestSendProductMessage_ShouldCreateChannelAndPublishMessage()
        {
            // Arrange
            try
            {
                await rabbitProducer.SendProductMessageConciliacion(file);
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }
        }

    }
}
