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
    public class ConsultarPagoQueryHandlerTests
    {
        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        private readonly ConsultarPagosQueryHandler _handler;
        private readonly Mock<ILogger<ConsultarPagoQuery>> _logger;


        public ConsultarPagoQueryHandlerTests()
        {
            _dbContextMock = new();

            _dbContextMock.SetupDbContextData();

            _logger = new();
            _handler = new(_dbContextMock.Object, _logger.Object);
        }

        //Unit Test Failed by Empty Request
        [Fact]
        public async Task Handle_ConsultarPago_SholudThorwExceptionEmptyRequest()
        {
            //ARRANGE

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(null, CancellationToken.None));
        }

        //Unit Test Failed
        [Fact]
        public async Task Handle_ConsultarPago_SholudThorwExceptionValidator()
        {
            //ARRANGE
            var request = new ConsultarPagoQuery("", "", "19/06/2023", "25/05/2023");

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Success
        [Fact]
        public async Task Handle_ConsultarPago_SholudReturnAllPagos()
        {
            //ARRANGE

            //Create a new request
            var request = new ConsultarPagoQuery("","","19/06/2023", DateTime.Now.ToString("dd/MM/yyyy"));

            //ACT
            var result = await _handler.Handle(request, CancellationToken.None);

            //ASSERT
            Assert.IsType<List<PagoResponse>>(result);
        }

        //Unit Test Failed
        [Fact]
        public async Task Handle_ConsultarPago_SholudThrowExceptionNoPagos()
        {
            //ARRANGE

            //Create a new request
            var request = new ConsultarPagoQuery("", "", "19/06/2023", "25/06/2023");

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }


        //Unit Test Success
        [Fact]
        public async Task Handle_ConsultarPago_SholudReturnAllConsumidoresPagos()
        {
            //ARRANGE

            //Create a new request
            var request = new ConsultarPagoQuery("", "username4", "19/06/2023", DateTime.Now.ToString("dd/MM/yyyy"));

            //ACT
            var result = await _handler.Handle(request, CancellationToken.None);

            //ASSERT
            Assert.IsType<List<PagoResponse>>(result);
        }

        //Unit Test Failed
        [Fact]
        public async Task Handle_ConsultarPago_SholudthrowNoPagoConsumidor()
        {
            //ARRANGE

            //Create a new request
            var request = new ConsultarPagoQuery("", "username4", "19/06/2023","25/06/2023");

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Success
        [Fact]
        public async Task Handle_ConsultarPago_SholudReturnAllServiciosPagos()
        {
            //ARRANGE

            //Create a new request
            var request = new ConsultarPagoQuery("servicio3", "", "19/06/2023", DateTime.Now.ToString("dd/MM/yyyy"));

            //ACT
            var result = await _handler.Handle(request, CancellationToken.None);

            //ASSERT
            Assert.IsType<List<PagoResponse>>(result);
        }

        //Unit Test Failed
        [Fact]
        public async Task Handle_ConsultarPago_SholudthrowExceptionNoServiciosPagos()
        {
            //ARRANGE

            //Create a new request
            var request = new ConsultarPagoQuery("servicio1", "", "19/06/2023", "25/06/2023");

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed
        [Fact]
        public async Task Handle_ConsultarPago_SholudthrowExceptionCustom1()
        {
            //ARRANGE

            //Create a new request
            var request = new ConsultarPagoQuery("servicio1", "NoExist", "19/06/2023", DateTime.Now.ToString("dd/MM/yyyy"));

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }
    }
}
