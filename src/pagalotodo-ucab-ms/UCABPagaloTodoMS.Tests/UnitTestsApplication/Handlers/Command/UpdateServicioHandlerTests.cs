using Moq;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Tests.MockData;
using Xunit;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Tests.DataSeed;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Requests;
using MediatR;
using UCABPagaloTodoMS.Application.Handlers.Commands;
using Microsoft.Extensions.Logging;

namespace UCABPagaloTodoMS.Tests.UnitTests.Handlers.Command
{
    public class UpdateServicioHandlerTests
    {
        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        private readonly Mock<IMediator> _mediator;
        private readonly UpdateServiciosHandler _handler;
        private readonly Mock<ILogger<UpdateServiciosCommand>> _logger;
        //Create a mock of IDbContextTransactionProxy to mock the transactions
        private readonly Mock<IDbContextTransactionProxy> transactionMock;

        //Create a new ServicioRequest with Random Values
        private ServicioRequest servicio = BuildDataContextFaker.BuildServicioRequest();


        public UpdateServicioHandlerTests()
        {
            _dbContextMock = new();

            _dbContextMock.SetupDbContextData();

            _mediator = new();
            _logger = new();
            _handler = new(_dbContextMock.Object, _logger.Object);
            transactionMock = new();
        }

        //Unit Test Success
        [Fact]
        public async Task Handle_UpdatePrestador_SholudReturnTheUsername()
        {
            //ARRANGE

            servicio = BuildDataContextFaker.BuildServicioRequestWithAnEspecificName("servicio1", "username7");

            //Create a new request
            var request = new UpdateServiciosCommand(servicio);

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(servicio.Nombre, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ACT
            var result = await _handler.Handle(request, CancellationToken.None);

            //ASSERT
            Assert.NotNull(result);
        }

        //Unit Test Failed by Validator
        [Fact]
        public async Task Handle_UpdateServicio_SholudThorwException()
        {
            //ARRANGE

            servicio = BuildDataContextFaker.BuildServicioRequestWithAnEspecificName("servicio1", null);

            //Create a new request
            var request = new UpdateServiciosCommand(servicio);

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(servicio.Nombre, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed by Empty Request
        [Fact]
        public async Task Handle_UpdateServicio_SholudThorwExceptionEmptyRequest()
        {
            //ARRANGE

            //Create a new request
            var request = new UpdateServiciosCommand(null);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed by non Existent Serivico
        [Fact]
        public async Task Handle_UpdateServicio_SholudThorwExceptionNoSerivico()
        {
            //ARRANGE

            servicio = BuildDataContextFaker.BuildServicioRequestWithAnEspecificName("NoExist", "username7");

            //Create a new request
            var request = new UpdateServiciosCommand(servicio);

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(servicio.Nombre, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed
        [Fact]
        public async Task Handle_UpdateServicio_SholudThorwExceptionGeneral()
        {
            //ARRANGE

            //Create a new request
            var request = new UpdateServiciosCommand(servicio);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Throws(new Exception());

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }
    }
}