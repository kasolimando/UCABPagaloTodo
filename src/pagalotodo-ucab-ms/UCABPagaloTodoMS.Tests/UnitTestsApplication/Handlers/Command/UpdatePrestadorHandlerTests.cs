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
    public class UpdatePrestadorHandlerTests
    {
        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        private readonly Mock<IMediator> _mediator;
        private readonly UpdatePrestadoresHandler _handler;
        private readonly Mock<ILogger<UpdatePrestadoresCommand>> _logger;
        //Create a mock of IDbContextTransactionProxy to mock the transactions
        private readonly Mock<IDbContextTransactionProxy> transactionMock;

        //Create a new PrestadorRequest with Random Values
        private PrestadorRequest prestador = BuildDataContextFaker.BuildPrestadorRequest();


        public UpdatePrestadorHandlerTests()
        {
            _dbContextMock = new();

            _dbContextMock.SetupDbContextData();

            _mediator = new();
            _logger = new();
            _handler = new(_dbContextMock.Object, _logger.Object);
            transactionMock = new();
        }

        //Unit Test Failed by Validator
        [Fact]
        public async Task Handle_UpdatePrestador_SholudThorwException()
        {
            //ARRANGE

            prestador = BuildDataContextFaker.BuildPrestadorRequestWithEspecificEmailAndUsername("NoCumplo", "TengoMuchoLetras");

            //Create a new request
            var request = new UpdatePrestadoresCommand(prestador, "TengoMuchoLetras");

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(prestador.Username, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed by Duplicated Email or Username
        [Fact]
        public async Task Handle_UpdatePrestador_SholudThorwExceptionFoDuplicates()
        {
            //ARRANGE

            prestador = BuildDataContextFaker.BuildPrestadorRequestWithEspecificEmailAndUsername("username80@gmail.com", "username9");

            //Create a new request
            var request = new UpdatePrestadoresCommand(prestador, "username9");

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(prestador.Username, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed by non Existent User
        [Fact]
        public async Task Handle_UpdatePrestador_SholudThorwExceptionNoUser()
        {
            //ARRANGE

            prestador = BuildDataContextFaker.BuildPrestadorRequestWithEspecificEmailAndUsername("username5@gmail.com", "NoExist");

            //Create a new request
            var request = new UpdatePrestadoresCommand(prestador, "NoExist");

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(prestador.Username, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed by Empty Request
        [Fact]
        public async Task Handle_UpdatePrestador_SholudThorwExceptionEmptyRequest()
        {
            //ARRANGE

            //Create a new request
            var request = new UpdatePrestadoresCommand(null, "NoExist");

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(prestador.Username, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed by Empty Request
        [Fact]
        public async Task Handle_UpdatePrestador_SholudThorwExceptionGeneral()
        {
            //ARRANGE

            //Create a new request
            var request = new UpdatePrestadoresCommand(prestador, "NoExist");

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Throws(new Exception());

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }
    }
}