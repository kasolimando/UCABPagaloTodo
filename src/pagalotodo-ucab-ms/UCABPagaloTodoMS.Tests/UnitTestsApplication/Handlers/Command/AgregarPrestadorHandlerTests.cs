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
    public class AgregarPrestadorHandlerTests
    {
        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        private readonly AgregarPrestadoresHandler _handler;
        private readonly Mock<ILogger<AgregarPrestadorCommand>> _logger;
        //Create a mock of IDbContextTransactionProxy to mock the transactions
        private readonly Mock<IDbContextTransactionProxy> transactionMock;

        //Create a new PrestadorRequest with Random Values
        private PrestadorRequest prestador = BuildDataContextFaker.BuildPrestadorRequest();


        public AgregarPrestadorHandlerTests()
        {
            _dbContextMock = new();

            _dbContextMock.SetupDbContextData();

            _logger = new();
            _handler = new(_dbContextMock.Object, _logger.Object);
            transactionMock = new();
        }

        //Unit Test Success
        [Fact]
        public async Task Handle_AgregarPrestador_SholudReturnTheUsername()
        {
            //ARRANGE

            prestador = BuildDataContextFaker.BuildPrestadorRequestWithEspecificEmailAndUsername("bueno@gmail.com", "username");

            //Create a new request
            var request = new AgregarPrestadorCommand(prestador);

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(prestador.Username, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ACT
            var result = await _handler.Handle(request, CancellationToken.None);

            //ASSERT
            Assert.NotNull(result);
        }

        //Unit Test Failed by Validator
        [Fact]
        public async Task Handle_AgregarPrestador_SholudThorwException()
        {
            //ARRANGE

            prestador = BuildDataContextFaker.BuildPrestadorRequestWithEspecificEmailAndUsername("NoCumplo", "TengoMuchoLetras");

            //Create a new request
            var request = new AgregarPrestadorCommand(prestador);

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(prestador.Username, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed by Duplicated Email or Username
        [Fact]
        public async Task Handle_AgregarPrestador_SholudThorwExceptionFoDuplicates()
        {
            //ARRANGE

            prestador = BuildDataContextFaker.BuildPrestadorRequestWithEspecificEmailAndUsername("username1@gmail.com", "username1");

            //Create a new request
            var request = new AgregarPrestadorCommand(prestador);

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(prestador.Username, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed by Empty Request
        [Fact]
        public async Task Handle_AgregarPrestador_SholudThorwExceptionForEmptyRequest()
        {
            //ARRANGE

            //Create a new request
            var request = new AgregarPrestadorCommand(null);

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(prestador.Username, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed
        [Fact]
        public async Task Handle_AgregarPrestador_SholudThrowCustomException()
        {
            //ARRANGE

            //Create a new request
            var request = new AgregarPrestadorCommand(prestador);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Throws(new CustomException(new() { ""}));

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed 
        [Fact]
        public async Task Handle_AgregarPrestador_SholudThrowExceptionGeneral()
        {
            //ARRANGE

            //Create a new request
            var request = new AgregarPrestadorCommand(prestador);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Throws(new Exception());

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }
    }
}