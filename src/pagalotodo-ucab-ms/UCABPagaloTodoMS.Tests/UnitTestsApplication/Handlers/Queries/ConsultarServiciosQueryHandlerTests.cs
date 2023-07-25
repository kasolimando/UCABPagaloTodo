using Moq;
using UCABPagaloTodoMS.Core.Database;
using Xunit;
using UCABPagaloTodoMS.Tests.DataSeed;
using UCABPagaloTodoMS.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Handlers.Queries;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Application.Responses;

namespace UCABPagaloTodoMS.Tests.UnitTests.Handlers.Queries
{
    public class ConsultarServiciosQueryHandlerTests
    {
        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        private readonly ConsultarServiciosQueryHandler _handler;
        private readonly Mock<ILogger<ConsultarServicioQuery>> _logger;


        public ConsultarServiciosQueryHandlerTests()
        {
            _dbContextMock = new();

            _dbContextMock.SetupDbContextData();

            _logger = new();
            _handler = new(_dbContextMock.Object, _logger.Object);
        }

        //Unit Test Failed by Empty Request
        [Fact]
        public async Task Handle_ConsultarServicio_SholudThorwExceptionEmptyRequest()
        {
            //ARRANGE

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(null, CancellationToken.None));
        }

        //Unit Test Success just the Servicio
        [Fact]
        public async Task Handle_ConsultarServicio_SholudReturnTheServicio()
        {
            //ARRANGE

            //Create a new request
            var request = new ConsultarServicioQuery("servicio1","servicio");

            //ACT
            var result = await _handler.Handle(request, CancellationToken.None);

            //ASSERT
            Assert.IsType<List<ServiciosResponse>>(result);
        }

        //Unit Test Success all Servicio
        [Fact]
        public async Task Handle_ConsultarServicio_SholudReturnAllServicio()
        {
            //ARRANGE

            //Create a new request
            var request = new ConsultarServicioQuery(null,"servicio");

            //ACT
            var result = await _handler.Handle(request, CancellationToken.None);

            //ASSERT
            Assert.IsType<List<ServiciosResponse>>(result);
        }

        //Unit Test Failed Non Existent Servicio
        [Fact]
        public async Task Handle_ConsultarServicio_ShoulddThrowAExceptionNoUser()
        {
            //ARRANGE

            //Create a new request
            var request = new ConsultarServicioQuery("NoExist","servicio");

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ConsultarServicio_SholudReturnAllServicioByPrestador()
        {
            //ARRANGE

            //Create a new request
            var request = new ConsultarServicioQuery("username9", "prestador");

            //ACT
            var result = await _handler.Handle(request, CancellationToken.None);

            //ASSERT
            Assert.IsType<List<ServiciosResponse>>(result);
        }
        [Fact]
        public async Task Handle_ConsultarServicio_SholudThrowExceptionAllServicioByPrestador()
        {
            //ARRANGE

            //Create a new request
            var request = new ConsultarServicioQuery(null, "prestador");

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ConsultarServicio_SholudThrowExceptionNoPrestador()
        {
            //ARRANGE

            //Create a new request
            var request = new ConsultarServicioQuery("username7", "prestador");

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }
    }
}
