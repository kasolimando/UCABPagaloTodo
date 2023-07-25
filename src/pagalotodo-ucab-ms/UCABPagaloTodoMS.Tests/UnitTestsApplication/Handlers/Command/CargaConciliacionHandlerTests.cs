using Moq;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Tests.MockData;
using Xunit;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Tests.DataSeed;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Handlers.Commands;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.BusinessValidation.Implementation;
using Microsoft.AspNetCore.Http;
using UCABPagaloTodoMS.Infrastructure.Services.RabbitProducer;

namespace UCABPagaloTodoMS.Tests.UnitTestsApplication.Handlers.Command
{
    public class CargaConciliacionHandlerTests
    {
        private readonly CargarConciliacionHandler _handler;
        private readonly Mock<ILogger<CargarConciliacionCommand>> _logger;
        //Create a mock of IDbContextTransactionProxy to mock the transactions
        private readonly Mock<IRabbitProducerConciliacion> _producer;

        private IFormFile conciliacion = BuildDataContextFaker.BuildArchivo();

        public CargaConciliacionHandlerTests()
        {
            _producer = new();

            _logger = new();
            _handler = new(_logger.Object, _producer.Object);
        }

        //Unit Test Success Admin
        [Fact]
        public async Task Handle_CargarConciliacion_SholudReturnTheFileName()
        {
            //ARRANGE

            //Create a new request
            var request = new CargarConciliacionCommand(conciliacion);

            _producer.Setup(x => x.SendProductMessageConciliacion(request.Archivo)).Returns(Task.CompletedTask);

            //ACT
            var result = await _handler.Handle(request, CancellationToken.None);

            //ASSERT
            Assert.IsType<string>(result);
        }

        //Unit Test Failed 
        [Fact]
        public async Task Handle_CargarConciliacion_ShouldThrowExceptionNullrequestl()
        {

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(null, CancellationToken.None));
        }

        //Unit Test Failed Admin
        [Fact]
        public async Task Handle_CargarConciliacion_SholudThowSQLException()
        {
            //ARRANGE

            var request = new CargarConciliacionCommand(conciliacion);

            //Configure the _producer object for the transactions (BeginTransaction, commit and rollback)
            _producer.Setup(x => x.SendProductMessageConciliacion(request.Archivo)).Throws(new Exception());

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }
    }
}