using Moq;
using UCABPagaloTodoMS.Core.Database;
using Xunit;
using UCABPagaloTodoMS.Tests.DataSeed;
using UCABPagaloTodoMS.Application.Exceptions;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Handlers.Queries;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Application.Responses;

namespace UCABPagaloTodoMS.Tests.UnitTests.Handlers.Queries
{
    public class ConsultarDeudasQueryHandlerTests
    {
        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        private readonly ConsultarDeudasQueryHandler _handler;
        private readonly Mock<ILogger<ConsultarDeudasQuery>> _logger;


        public ConsultarDeudasQueryHandlerTests()
        {
            _dbContextMock = new();

            _dbContextMock.SetupDbContextData();

            _logger = new();
            _handler = new(_dbContextMock.Object, _logger.Object);
        }

        //Unit Test Failed by Empty Request
        [Fact]
        public async Task Handle_ConsultarDeudas_SholudThorwExceptionEmptyRequest()
        {
            //ARRANGE

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(null, CancellationToken.None));
        }

        //Unit Test Success
        [Fact]
        public async Task Handle_ConsultarDeudas_SholudReturnTheConsumidor()
        {
            //ARRANGE

            //Create a new request
            var request = new ConsultarDeudasQuery("servicio3", "username4");

            //ACT
            var result = await _handler.Handle(request, CancellationToken.None);

            //ASSERT
            Assert.IsType<List<DeudaResponse>>(result);
        }

        //Unit Test Success all deudas
        [Fact]
        public async Task Handle_ConsultarDeudas_SholudReturnAllConsumidores()
        {
            //ARRANGE

            //Create a new request
            var request = new ConsultarDeudasQuery("servicio3",null);


            //ACT
            var result = await _handler.Handle(request, CancellationToken.None);

            //ASSERT
            Assert.IsType<List<DeudaResponse>>(result);
        }

        //Unit Test Failed
        [Fact]
        public async Task Handle_ConsultarDeudas_ShouldThrowAExceptionNoDeudas()
        {
            //ARRANGE

            //Create a new request
            var request = new ConsultarDeudasQuery("servicio1",null);


            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed
        [Fact]
        public async Task Handle_ConsultarDeudas_ShouldThrowASqlException()
        {
            //ARRANGE

            //Create a new request
            var request = new ConsultarDeudasQuery("NoExiste", null);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed
        [Fact]
        public async Task Handle_ConsultarDeudas_ShouldThrowACustomExceptionNoUser()
        {
            //ARRANGE

            //Create a new request
            var request = new ConsultarDeudasQuery("servicio3", "username6");

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed
        [Fact]
        public async Task Handle_ConsultarDeudas_ShouldThrowACustomExceptionGeneral()
        {
            //ARRANGE

            //Create a new request
            var request = new ConsultarDeudasQuery("servicio3", "Noexiste");

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }
    }
}
