using Moq;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Tests.MockData;
using Xunit;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Tests.DataSeed;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Handlers.Commands;
using Microsoft.Extensions.Logging;

namespace UCABPagaloTodoMS.Tests.UnitTests.Handlers.Command
{
    public class UpdateFormatoHandlerTests
    {
        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        private readonly UpdateFormatosHandler _handler;
        private readonly Mock<ILogger<UpdateFormatosCommand>> _logger;
        //Create a mock of IDbContextTransactionProxy to mock the transactions
        private readonly Mock<IDbContextTransactionProxy> transactionMock;

        //Create a new FormatoRequest with Random Values
        private FormatosRequest formato = BuildDataContextFaker.BuildFormatosRequest();


        public UpdateFormatoHandlerTests()
        {
            _dbContextMock = new();

            _dbContextMock.SetupDbContextData();

            _logger = new();
            _handler = new(_dbContextMock.Object, _logger.Object);
            transactionMock = new();
        }

        //Unit Test Success
        [Fact]
        public async Task Handle_UpdateFormato_SholudReturnTheUsername()
        {
            //ARRANGE

            formato = BuildDataContextFaker.BuildFormatosRequestWithEspecifiServicio("servicio3");

            //Create a new request
            var request = new UpdateFormatosCommand(formato);

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(formato.Servicio, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ACT
            var result = await _handler.Handle(request, CancellationToken.None);

            //ASSERT
            Assert.IsType<string>(result);
        }

        //Unit Test Failed by Validator
        [Fact]
        public async Task Handle_Updateformato_SholudThorwValidatorException()
        {
            //ARRANGE

            formato = BuildDataContextFaker.BuildFormatosRequestWithEspecifiServicio(null);

            //Create a new request
            var request = new UpdateFormatosCommand(formato);

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(formato.Servicio, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed by non Existent Serivico
        [Fact]
        public async Task Handle_UpdateFormato_SholudThorwExceptionNoSerivico()
        {
            //ARRANGE

            formato = BuildDataContextFaker.BuildFormatosRequestWithEspecifiServicio("NoExist");

            //Create a new request
            var request = new UpdateFormatosCommand(formato);

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(formato.Servicio, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed by Empty Request
        [Fact]
        public async Task Handle_UpdateFormato_SholudThorwExceptionEmptyRequest()
        {
            //ARRANGE

            //Create a new request
            var request = new UpdateFormatosCommand(null);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed
        [Fact]
        public async Task Handle_UpdateFormato_SholudthrowAExceptionGeneral()
        {
            //ARRANGE
            formato = BuildDataContextFaker.BuildFormatosRequestWithEspecifiServicio("servicio2");

            //Create a new request
            var request = new UpdateFormatosCommand(formato);

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(formato.Servicio, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed
        [Fact]
        public async Task Handle_UpdateFormato_SholudthrowAException()
        {
            //ARRANGE
            formato = BuildDataContextFaker.BuildFormatosRequestWithEspecifiServicio("servicio2");

            //Create a new request
            var request = new UpdateFormatosCommand(formato);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Throws(new Exception());

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }
    }
}