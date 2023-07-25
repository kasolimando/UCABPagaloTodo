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
    public class UpdateConsumidorHandlerTests
    {
        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        private readonly UpdateConsumidoresHandler _handler;
        private readonly Mock<ILogger<UpdateConsumCommand>> _logger;
        //Create a mock of IDbContextTransactionProxy to mock the transactions
        private readonly Mock<IDbContextTransactionProxy> transactionMock;

        //Create a new ConsumidorRequest with Random Values
        private ConsumidorRequest consumidor = BuildDataContextFaker.BuildConsumidorRequest();


        public UpdateConsumidorHandlerTests()
        {
            _dbContextMock = new();

            _dbContextMock.SetupDbContextData();

            _logger = new();
            _handler = new(_dbContextMock.Object, _logger.Object);
            transactionMock = new();
        }

        //Unit Test Success
        [Fact]
        public async Task Handle_UpdateConsumidor_SholudReturnTheUsername()
        {
            //ARRANGE

            consumidor = BuildDataContextFaker.BuildConsumidorRequestWithEspecificEmailAndUsername("username5@gmail.com", "username5");

            //Create a new request
            var request = new UpdateConsumCommand(consumidor, "username5");

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(consumidor.Username, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ACT
            var result = await _handler.Handle(request, CancellationToken.None);

            //ASSERT
            Assert.NotNull(result);
        }

        //Unit Test Failed by Validator
        [Fact]
        public async Task Handle_UpdateConsumidor_SholudThorwException()
        {
            //ARRANGE

            consumidor = BuildDataContextFaker.BuildConsumidorRequestWithEspecificEmailAndUsername("NoCumplo", "TengoMuchoLetras");

            //Create a new request
            var request = new UpdateConsumCommand(consumidor, "TengoMuchoLetras");

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(consumidor.Username, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed by Duplicated Email or Username
        [Fact]
        public async Task Handle_UpdateConsumidor_SholudThorwExceptionFoDuplicates()
        {
            //ARRANGE

            consumidor = BuildDataContextFaker.BuildConsumidorRequestWithEspecificEmailAndUsername("username1@gmail.com", "username5");

            //Create a new request
            var request = new UpdateConsumCommand(consumidor,"username5");

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(consumidor.Username, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed by non Existent User
        [Fact]
        public async Task Handle_UpdateConsumidor_SholudThorwExceptionNoUser()
        {
            //ARRANGE

            consumidor = BuildDataContextFaker.BuildConsumidorRequestWithEspecificEmailAndUsername("username5@gmail.com", "NoExist");

            //Create a new request
            var request = new UpdateConsumCommand(consumidor, "NoExist");

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(consumidor.Username, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed by Empty Request
        [Fact]
        public async Task Handle_UpdateConsumidor_SholudThorwExceptionemptyRequest()
        {
            //ARRANGE

            //Create a new request
            var request = new UpdateConsumCommand(null,"NoExist");

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(consumidor.Username, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed by Duplicated Email or Username
        [Fact]
        public async Task Handle_UpdateConsumidor_SholudThorwExceptionGeneral()
        {
            //ARRANGE

            //Create a new request
            var request = new UpdateConsumCommand(consumidor, "username5");

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Throws(new Exception());

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }
    }
}