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
    public class AgregarConsumidorHandlerTests
    {
        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        private readonly AgregarConsumidoresHandler _handler;
        private readonly Mock<ILogger<AgregarConsumidorCommand>> _logger;
        //Create a mock of IDbContextTransactionProxy to mock the transactions
        private readonly Mock<IDbContextTransactionProxy> transactionMock;

        //Create a new ConsumidorRequest with Random Values
        private ConsumidorRequest consumidor = BuildDataContextFaker.BuildConsumidorRequest();


        public AgregarConsumidorHandlerTests()
        {
            _dbContextMock = new();

            _dbContextMock.SetupDbContextData();

            _logger = new();
            _handler = new(_dbContextMock.Object, _logger.Object);
            transactionMock = new();
        }

        //Unit Test Success
        [Fact]
        public async Task Handle_AgregarConsumidor_SholudReturnTheUsername()
        {
            //ARRANGE

            consumidor = BuildDataContextFaker.BuildConsumidorRequestWithEspecificEmailAndUsername("bueno@gmail.com", "username");

            //Create a new request
            var request = new AgregarConsumidorCommand(consumidor);

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(consumidor.Username, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ACT
            var result = await _handler.Handle(request, CancellationToken.None);

            //ASSERT
            Assert.IsType<string>(result);
        }

        //Unit Test Failed by Validator
        [Fact]
        public async Task Handle_AgregarConsumidor_SholudThorwException()
        {
            //ARRANGE

            consumidor = BuildDataContextFaker.BuildConsumidorRequestWithEspecificEmailAndUsername("NoCumplo", "TengoMuchoLetras");

            //Create a new request
            var request = new AgregarConsumidorCommand(consumidor);

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(consumidor.Username, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed by Duplicated Email or Username
        [Fact]
        public async Task Handle_AgregarConsumidor_SholudThorwExceptionFoDuplicates()
        {
            //ARRANGE

            consumidor = BuildDataContextFaker.BuildConsumidorRequestWithEspecificEmailAndUsername("username1@gmail.com", "username1");

            //Create a new request
            var request = new AgregarConsumidorCommand(consumidor);

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(consumidor.Username, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed by Empty Request
        [Fact]
        public async Task Handle_AgregarConsumidor_SholudThorwExceptionForEmptyRequest()
        {
            //ARRANGE

            //Create a new request
            var request = new AgregarConsumidorCommand(null);

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(consumidor.Username, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }
        //Unit Test Failed by Validator
        [Fact]
        public async Task Handle_AgregarConsumidor_SholudThrowExceptionGeneral()
        {
            //ARRANGE

            consumidor = BuildDataContextFaker.BuildConsumidorRequestWithEspecificEmailAndUsername("NoExiste@gmail.com", "NoExiste");

            //Create a new request
            var request = new AgregarConsumidorCommand(consumidor);

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(consumidor.Username, default)).ThrowsAsync(new Exception());

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }
    }
}