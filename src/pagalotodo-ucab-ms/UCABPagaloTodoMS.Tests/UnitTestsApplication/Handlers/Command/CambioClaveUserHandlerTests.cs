using Moq;
using UCABPagaloTodoMS.Tests.MockData;
using Xunit;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Exceptions;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Handlers.Commands;
using UCABPagaloTodoMS.Infrastructure.Services.RabbitProducer;
using Microsoft.AspNetCore.Http;
using UCABPagaloTodoMS.Application.BusinessValidation.Implementation;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Tests.DataSeed;

namespace UCABPagaloTodoMS.Tests.UnitTestsApplication.Handlers.Command
{
    public class CambioClaveUserHandlerTests
    {
        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        private readonly CambioClaveUserHandler _handler;
        private readonly Mock<ILogger<CambioClaveUserCommand>> _logger;
        //Create a mock of IDbContextTransactionProxy to mock the transactions
        private readonly Mock<IDbContextTransactionProxy> transactionMock;

        public CambioClaveUserHandlerTests()
        {
            _dbContextMock = new();

            _dbContextMock.SetupDbContextData();

            _logger = new();
            _handler = new(_dbContextMock.Object, _logger.Object);
            transactionMock = new();
        }

        //Unit Test Success Admin
        [Fact]
        public async Task Handle_CambioClaveUserAdmin_SholudReturnTheUsername()
        {
            //ARRANGE

            //Create a new CambioClaveUserRequest with Random Values
            var cambio = BuildDataContextFaker.BuildCambioClaveUserRequestWithSpecificInfo("username1");

            //Create a new request
            var request = new CambioClaveUserCommand(cambio, new AdminCambiarClaveValidation(), "username2");

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(request.Username, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ACT
            var result = await _handler.Handle(request, CancellationToken.None);

            //ASSERT
            Assert.IsType<string>(result);
        }

        //Unit Test Failed by Non existent Username
        [Fact]
        public async Task Handle_CambioClaveUserAdmin_ShouldThrowExceptionByNoUsername()
        {
            //ARRANGE

            //Create a new CambioClaveUserRequest with Random Values
            var cambio = BuildDataContextFaker.BuildCambioClaveUserRequestWithSpecificInfo("usernameE");

            //Create a new request
            var request = new CambioClaveUserCommand(cambio, new AdminCambiarClaveValidation(), "NoExist");

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(request.Username, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed Admin
        [Fact]
        public async Task Handle_CambioClaveUserAdmin_SholudThowSQLException()
        {
            //ARRANGE

            //Create a new CambioClaveUserRequest with Random Values
            var cambio = BuildDataContextFaker.BuildCambioClaveUserRequestWithSpecificInfo("username1");

            //Create a new request
            var request = new CambioClaveUserCommand(cambio, new AdminCambiarClaveValidation(), "username1");
            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Throws(new SQLException(new() { "" }));

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }


        //Unit Test Failed Admin
        [Fact]
        public async Task Handle_CambioClaveUserAdmin_SholudThowException()
        {
            //ARRANGE

            //Create a new CambioClaveUserRequest with Random Values
            var cambio = BuildDataContextFaker.BuildCambioClaveUserRequestWithSpecificInfo("username1");

            //Create a new request
            var request = new CambioClaveUserCommand(cambio, new AdminCambiarClaveValidation(), "username1");
            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Throws(new Exception());

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed Admin
        [Fact]
        public async Task Handle_CambioClaveUserAdmin_SholudThowValidatorException()
        {
            //ARRANGE

            //Create a new CambioClaveUserRequest with Random Values
            var cambio = BuildDataContextFaker.BuildCambioClaveUserRequestWithSpecificInfo("");

            //Create a new request
            var request = new CambioClaveUserCommand(cambio, new AdminCambiarClaveValidation(), "username1");

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }
        //Unit Test Failed Admin
        [Fact]
        public async Task Handle_CambioClaveUserAdmin_SholudThowExceptionByNullRequest()
        {
            //ARRANGE

            //Create a new request
            var request = new CambioClaveUserCommand(null, new AdminCambiarClaveValidation(), "username1");

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }
    }
}