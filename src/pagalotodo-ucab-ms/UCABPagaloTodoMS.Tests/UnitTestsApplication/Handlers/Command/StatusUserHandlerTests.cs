using Microsoft.Extensions.Logging;
using Moq;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Handlers.Commands;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Tests.MockData;
using Xunit;
using UCABPagaloTodoMS.Tests.DataSeed;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.BusinessValidation.Implementation;

namespace UCABPagaloTodoMS.Tests.UnitTests.Handlers.Command
{
    public class StatusUserHandlerTests
    {

        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        private readonly StatusUserHandler _handler;
        private readonly Mock<ILogger<StatusUserCommand>> _logger;
        //Create a mock of IDbContextTransactionProxy to mock the transactions
        private readonly Mock<IDbContextTransactionProxy> transactionMock;

        //Create a new StatusUserRequest with Random Values
        private StatusUserRequest status = BuildDataContextFaker.BuildStatusUserRequest();


        public StatusUserHandlerTests()
        {
            _dbContextMock = new();

            _dbContextMock.SetupDbContextData();
            _logger = new();
            _handler = new(_dbContextMock.Object, _logger.Object);
            transactionMock = new();
        }

        //Unit Test Success Admin
        [Fact]
        public async Task Handle_StatusAdmin_SholudReturnTheUsername()
        {
            //ARRANGE

            //Create a new request
            var request = new StatusUserCommand(status, new AdminStatusUserValidation(),"username1");

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(request.Username, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ACT
            var result = await _handler.Handle(request, CancellationToken.None);

            //ASSERT
            Assert.IsType<string>(result);
        }

        //Unit Test Failed by Empty Request
        [Fact]
        public async Task Handle_Status_SholudThorwExceptionEmptyRequest()
        {
            //ARRANGE

            //Create a new request
            var request = new StatusUserCommand(null, new AdminStatusUserValidation(), "username1");

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed by non Existent User
        [Fact]
        public async Task Handle_Status_SholudThorwExceptionNoUser()
        {
            //Create a new request
            var request = new StatusUserCommand(status, new AdminStatusUserValidation(), "NoExist");


            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed 
        [Fact]
        public async Task Handle_Status_SholudThorwException()
        {
            //ARRANGE

            //Create a new request
            var request = new StatusUserCommand(status, new AdminStatusUserValidation(), "username1");

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Throws(new Exception());

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed 
        [Fact]
        public async Task Handle_Status_SholudThorwValidatorException()
        {
            //ARRANGE

            //Create a new request
            var request = new StatusUserCommand(status, new AdminStatusUserValidation(), null);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed 
        [Fact]
        public async Task Handle_Status_SholudThorwCustomException()
        {
            //ARRANGE

            //Create a new request
            var request = new StatusUserCommand(status, new AdminStatusUserValidation(), "username1");

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Throws(new CustomException(new() { ""}));

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }
    }
}
