using Moq;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Tests.MockData;
using Xunit;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Tests.DataSeed;
using UCABPagaloTodoMS.Application.Exceptions;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Handlers.Commands;
using UCABPagaloTodoMS.Application.Requests;

namespace UCABPagaloTodoMS.Tests.UnitTests.Handlers.Command
{
    public class StatusServicioHandlerTests
    {
        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        private readonly StatusServiciosHandler _handler;
        private readonly Mock<ILogger<StatusServiciosCommand>> _logger;
        //Create a mock of IDbContextTransactionProxy to mock the transactions
        private readonly Mock<IDbContextTransactionProxy> transactionMock;

        private StatusServicioRequest servicioStatus = BuildDataContextFaker.BuildStatusServicioRequest();


        public StatusServicioHandlerTests()
        {
            _dbContextMock = new();

            _dbContextMock.SetupDbContextData();
            _logger = new();
            _handler = new(_dbContextMock.Object, _logger.Object);
            transactionMock = new();
        }

        //Unit Test Success
        [Fact]
        public async Task Handle_StatusServicios_SholudReturnTheName()
        {
            //ARRANGE

            //Create a new request
            var request = new StatusServiciosCommand(servicioStatus,"servicio1");

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges("servicio1", default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ACT
            var result = await _handler.Handle(request, CancellationToken.None);

            //ASSERT
            Assert.IsType<string>(result);
        }

        //Unit Test Failed by Validator
        [Fact]
        public async Task Handle_StatusServicios_SholudThorwException()
        {
            //ARRANGE
            //Create a new request
            var request = new StatusServiciosCommand(servicioStatus, null);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed by Empty Request
        [Fact]
        public async Task Handle_StatusServicios_SholudThorwExceptionForEmptyRequest()
        {
            //ARRANGE

            //Create a new request
            var request = new StatusServiciosCommand(null,"servicio1");
            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed by Already Exist servicio
        [Fact]
        public async Task Handle_StatusServicios_ShouldThrowASqlException()
        {
            //ARRANGE

            //Create a new request
            var request = new StatusServiciosCommand(servicioStatus,"NoExisto");

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed 
        [Fact]
        public async Task Handle_StatusServicios_ShouldThrowException()
        {
            //ARRANGE

            //Create a new request
            var request = new StatusServiciosCommand(servicioStatus,"servicio");

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Throws(new Exception());

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed 
        [Fact]
        public async Task Handle_StatusServicios_ShouldThrowCustomException()
        {
            //ARRANGE

            //Create a new request
            var request = new StatusServiciosCommand(servicioStatus, "servicio");

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Throws(new CustomException(new() { ""}));

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }
    }
}
