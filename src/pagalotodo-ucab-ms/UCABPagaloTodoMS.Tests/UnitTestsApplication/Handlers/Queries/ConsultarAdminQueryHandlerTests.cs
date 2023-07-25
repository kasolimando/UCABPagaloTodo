using Moq;
using UCABPagaloTodoMS.Core.Database;
using Xunit;
using UCABPagaloTodoMS.Tests.DataSeed;
using UCABPagaloTodoMS.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Handlers.Queries;
using UCABPagaloTodoMS.Application.Queries;

namespace UCABPagaloTodoMS.Tests.UnitTests.Handlers.Queries
{
    public class ConsultarAdminQueryHandlerTests
    {
        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        private readonly Mock<IMediator> _mediator;
        private readonly ConsultarAdminsQueryHandler _handler;
        private readonly Mock<ILogger<ConsultarAdminsQuery>> _logger;
        //Create a mock of IDbContextTransactionProxy to mock the transactions
        private readonly Mock<IDbContextTransactionProxy> transactionMock;


        public ConsultarAdminQueryHandlerTests()
        {
            _dbContextMock = new();

            _dbContextMock.SetupDbContextData();

            _mediator = new();
            _logger = new();
            _handler = new(_dbContextMock.Object, _logger.Object);
            transactionMock = new();
        }

        //Unit Test Failed by Empty Request
        [Fact]
        public async Task Handle_ConsultarAdmin_SholudThorwExceptionEmptyRequest()
        {
            //ARRANGE

            //Create a new request
            var request = new ConsultarAdminsQuery(null);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Success
        [Fact]
        public async Task Handle_ConsultarAdmin_SholudReturnTheUsername()
        {
            //ARRANGE

            //Create a new request
            var request = new ConsultarAdminsQuery("username1");

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ACT
            var result = await _handler.Handle(request, CancellationToken.None);

            //ASSERT
            Assert.NotNull(result);
        }

        //Unit Test Failed by a Non existent Username
        [Fact]
        public async Task Handle_ConsultarAdmin_ShouldThrowAExceptionNoUser()
        {
            //ARRANGE

            //Create a new request
            var request = new ConsultarAdminsQuery("");

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }
    }
}
