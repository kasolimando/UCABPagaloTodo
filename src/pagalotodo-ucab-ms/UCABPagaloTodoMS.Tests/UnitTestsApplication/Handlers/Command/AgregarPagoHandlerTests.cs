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

namespace UCABPagaloTodoMS.Tests.UnitTestsApplication.Handlers.Command
{
    public class AgregarPagoHandlerTests
    {
        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        private readonly AgregarPagoHandler _handler;
        private readonly Mock<ILogger<AgregarPagoCommand>> _logger;
        //Create a mock of IDbContextTransactionProxy to mock the transactions
        private readonly Mock<IDbContextTransactionProxy> transactionMock;

        //Create a new PagoRequest with Random Values
        private PagoRequest pago = BuildDataContextFaker.BuildPagoRequest();


        public AgregarPagoHandlerTests()
        {
            _dbContextMock = new();

            _dbContextMock.SetupDbContextData();

            _logger = new();
            _handler = new(_dbContextMock.Object, _logger.Object);
            transactionMock = new();
        }

        //Unit Test Success
        [Fact]
        public async Task Handle_AgregarPago_SholudReturnTheServicio()
        {
            //ARRANGE

            pago = BuildDataContextFaker.BuildPagoRequestWithEspecificData("servicio4", "username5");

            //Create a new request
            var request = new AgregarPagoCommand(pago);

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(pago.Consumidor, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ACT
            var result = await _handler.Handle(request, CancellationToken.None);

            //ASSERT
            Assert.NotNull(result);
        }

        //Unit Test Failed by Validator
        [Fact]
        public async Task Handle_AgregarPago_SholudThorwException()
        {
            //ARRANGE

            pago = BuildDataContextFaker.BuildPagoRequestWithEspecificData(string.Empty, "username5");

            //Create a new request
            var request = new AgregarPagoCommand(pago);

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(pago.Consumidor, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed for non existent username
        [Fact]
        public async Task Handle_AgregarPago_SholudThorwExceptionForNonExistent()
        {
            //ARRANGE

            pago = BuildDataContextFaker.BuildPagoRequestWithEspecificData("servicio4", "NoExist");

            //Create a new request
            var request = new AgregarPagoCommand(pago);

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(pago.Consumidor, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed by Empty Request
        [Fact]
        public async Task Handle_AgregarPago_SholudThorwExceptionForEmptyRequest()
        {
            //ARRANGE

            //Create a new request
            var request = new AgregarPagoCommand(null);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed by Exception
        [Fact]
        public async Task Handle_AgregarPago_SholudThorwExceptionGeneral()
        {
            //ARRANGE
            //Create a new request
            var request = new AgregarPagoCommand(pago);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Throws(new Exception());

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }
    }
}