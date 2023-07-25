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
    public class AgregarServicioHandlerTests
    {
        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        private readonly AgregarServicioHandler _handler;
        private readonly Mock<ILogger<AgregarServicioCommand>> _logger;
        //Create a mock of IDbContextTransactionProxy to mock the transactions
        private readonly Mock<IDbContextTransactionProxy> transactionMock;

        //Create a new ServicioRequest with Random Values
        private ServicioRequest servicio = BuildDataContextFaker.BuildServicioRequest();


        public AgregarServicioHandlerTests()
        {
            _dbContextMock = new();

            _dbContextMock.SetupDbContextData();
            _logger = new();
            _handler = new(_dbContextMock.Object, _logger.Object);
            transactionMock = new();
        }

        //Unit Test Success
        [Fact]
        public async Task Handle_AgregarServicio_SholudReturnTheName()
        {
            //ARRANGE

            servicio = BuildDataContextFaker.BuildServicioRequestWithAnEspecificName("ServicioNoExist", "username7");

            //Create a new request
            var request = new AgregarServicioCommand(servicio);

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
        public async Task Handle_AgregarServicio_SholudThorwException()
        {
            //ARRANGE

            servicio = BuildDataContextFaker.BuildServicioRequestWithAnEspecificName("NoCumplo", null);

            //Create a new request
            var request = new AgregarServicioCommand(servicio);

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(servicio.Nombre, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed by Empty Request
        [Fact]
        public async Task Handle_AgregarServicio_SholudThorwExceptionForEmptyRequest()
        {
            //ARRANGE

            //Create a new request
            var request = new AgregarServicioCommand(null);

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(servicio.Nombre, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed by Already Exist servicio
        [Fact]
        public async Task Handle_AgregarServicio_ShouldThrowASqlException()
        {
            //ARRANGE

            servicio = BuildDataContextFaker.BuildServicioRequestWithAnEspecificName("servicio2", "username7");

            //Create a new request
            var request = new AgregarServicioCommand(servicio);

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(servicio.Nombre, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed 
        [Fact]
        public async Task Handle_AgregarServicio_ShouldThrowException()
        {
            //ARRANGE

            servicio = BuildDataContextFaker.BuildServicioRequestWithAnEspecificName("servicio2", "username7");

            //Create a new request
            var request = new AgregarServicioCommand(servicio);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Throws(new Exception());

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }
    }
}
