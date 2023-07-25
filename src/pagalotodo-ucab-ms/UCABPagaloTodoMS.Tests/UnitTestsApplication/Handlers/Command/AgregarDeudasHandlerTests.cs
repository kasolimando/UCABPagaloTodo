using Moq;
using UCABPagaloTodoMS.Tests.MockData;
using Xunit;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Handlers.Commands;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Infrastructure.Services.RabbitProducer;
using RabbitMQ.Client;
using UCABPagaloTodoMS.Core.Services.Firebase;
using Microsoft.AspNetCore.Http;

namespace UCABPagaloTodoMS.Tests.UnitTestsApplication.Handlers.Command
{
    public class AgregarDeudasHandlerTests
    {
        private readonly AgregarDeudaHandler _handler;
        private readonly Mock<ILogger<AgregarDeudaCommand>> _logger;
        private readonly Mock<IRabbitProducer> _producer;
        private readonly Mock<IConnectionFactory> _factory;
        private readonly Mock<IFirebase> _firebase;
        private readonly Mock<IConnection> _connection;


        private IFormFile deuda = BuildDataContextFaker.BuildArchivo();

        public AgregarDeudasHandlerTests()
        {
            _logger = new();
            _factory = new();
            _producer = new();
            _firebase = new();
            _connection = new();
            _handler = new(_logger.Object, _producer.Object);
        }

        //Unit Test Success
        [Fact]
        public async Task Handle_AgregarDeuda_SholudReturnTheServicio()
        {
            //ARRANGE
            _firebase.Setup(fr => fr.ReadFileContentsAsync(It.IsAny<string>()))
            .ReturnsAsync("Servicio1 Username1 100.00\r\nServicio2 Username2 200.00\r\n");

            _factory.Setup(f => f.CreateConnection()).Returns(_connection.Object);

            //Create a new request
            var request = new AgregarDeudaCommand(deuda);

            //ACT
            var result = await _handler.Handle(request, CancellationToken.None);

            //ASSERT
            Assert.IsType<string>(result);
        }

        //Unit Test Failed by Validator
        [Fact]
        public async Task Handle_AgregarDeuda_SholudThrowValidationException()
        {
            //ARRANGE
            deuda = BuildDataContextFaker.BuildArchivoWithEmptyName();

            //Create a new request
            var request = new AgregarDeudaCommand(deuda);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed by Empty Request
        [Fact]
        public async Task Handle_AgregarDeuda_SholudThorwExceptionForEmptyRequest()
        {
            //ARRANGE
            _producer.Setup(x => x.SendProductMessage(deuda)).ThrowsAsync(new Exception());

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(null, CancellationToken.None));
        }

        //Unit Test Failed by Empty Request
        [Fact]
        public async Task Handle_AgregarDeuda_SholudThorwExceptionForProducer()
        {
            //ARRANGE
            _producer.Setup(x => x.SendProductMessage(deuda)).ThrowsAsync(new Exception());

            //Create a new request
            var request = new AgregarDeudaCommand(deuda);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }
    }
}