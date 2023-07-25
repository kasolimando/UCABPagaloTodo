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
    public class UpdateAdminHandlerTests
    {
        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        private readonly UpdateAdminsHandler _handler;
        private readonly Mock<ILogger<UpdateAdminsHandler>> _logger;
        //Create a mock of IDbContextTransactionProxy to mock the transactions
        private readonly Mock<IDbContextTransactionProxy> transactionMock;

        //Create a new AdminsRequest with Random Values
        private AdminsRequest admin = BuildDataContextFaker.BuildAdminRequest();


        public UpdateAdminHandlerTests()
        {
            _dbContextMock = new();

            _dbContextMock.SetupDbContextData();
            _logger = new();
            _handler = new(_dbContextMock.Object, _logger.Object);
            transactionMock = new();
        }

        //Unit Test Success
        [Fact]
        public async Task Handle_UpdateAdmin_SholudReturnTheUsername()
        {
            //ARRANGE

            admin = BuildDataContextFaker.BuildAdminRequestWithEspecificEmail("username1@gmail.com");

            //Create a new request
            var request = new UpdateAdminsCommand(admin, "username1");

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(request.Username, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ACT
            var result = await _handler.Handle(request, CancellationToken.None);

            //ASSERT
            Assert.NotNull(result);
        }

        //Unit Test Failed by Validator
        [Fact]
        public async Task Handle_UpdateAdmin_SholudThorwValidatorException()
        {
            //ARRANGE

            admin = BuildDataContextFaker.BuildAdminRequestWithEspecificEmail("NoCumplo");

            //Create a new request
            var request = new UpdateAdminsCommand(admin, "TengoMuchoLetras");

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed by Duplicated Email or Username
        [Fact]
        public async Task Handle_UpdateConsumidor_SholudThorwExceptionFoDuplicates()
        {
            //ARRANGE

            admin = BuildDataContextFaker.BuildAdminRequestWithEspecificEmail("username1@gmail.com");

            //Create a new request
            var request = new UpdateAdminsCommand(admin, "username2");

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(request.Username, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed by non Existent User
        [Fact]
        public async Task Handle_UpdateAdmin_SholudThorwExceptionNoUser()
        {
            //ARRANGE

            admin = BuildDataContextFaker.BuildAdminRequestWithEspecificEmail("username5@gmail.com");

            //Create a new request
            var request = new UpdateAdminsCommand(admin, "NoExist");

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(request.Username, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed by Empty Request
        [Fact]
        public async Task Handle_UpdateAdmin_SholudThorwExceptionemptyRequest()
        {
            //ARRANGE

            //Create a new request
            var request = new UpdateAdminsCommand(null,"NoExiste");

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(request.Username, default)).ReturnsAsync(true);

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
            var request = new UpdateAdminsCommand(admin, "username2");

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Throws(new Exception());

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }
    }
}