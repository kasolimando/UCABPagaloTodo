using Moq;
using UCABPagaloTodoMS.Core.Database;
using Xunit;
using UCABPagaloTodoMS.Tests.DataSeed;
using UCABPagaloTodoMS.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Handlers.Queries;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Application.Responses;

namespace UCABPagaloTodoMS.Tests.UnitTests.Handlers.Queries
{
    public class ConsultarPrestadoresQueryHandlerTests
    {
        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        private readonly Mock<IMediator> _mediator;
        private readonly ConsultarPrestadoresQueryHandler _handler;
        private readonly Mock<ILogger<ConsultarPrestadorQuery>> _logger;
        //Create a mock of IDbContextTransactionProxy to mock the transactions
        private readonly Mock<IDbContextTransactionProxy> transactionMock;


        public ConsultarPrestadoresQueryHandlerTests()
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
        public async Task Handle_ConsultarPrestador_SholudThorwExceptionEmptyRequest()
        {
            //ARRANGE

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(null, CancellationToken.None));
        }

        //Unit Test Success just the prestador
        [Fact]
        public async Task Handle_ConsultarPrestador_SholudReturnThePrestador()
        {
            //ARRANGE

            //Create a new request
            var request = new ConsultarPrestadorQuery("username8");

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ACT
            var result = await _handler.Handle(request, CancellationToken.None);

            //ASSERT
            Assert.IsType<List<PrestadoresResponse>>(result);
        }

        //Unit Test Success all Prestadores
        [Fact]
        public async Task Handle_ConsultarPrestadores_SholudReturnAllPrestadores()
        {
            //ARRANGE

            //Create a new request
            var request = new ConsultarPrestadorQuery(null);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ACT
            var result = await _handler.Handle(request, CancellationToken.None);

            //ASSERT
            Assert.IsType<List<PrestadoresResponse>>(result);
        }

        //Unit Test Failed Non Existent Prestadores
        [Fact]
        public async Task Handle_ConsultarPrestadores_ShoulddThrowAExceptionNoUser()
        {
            //ARRANGE

            //Create a new request
            var request = new ConsultarPrestadorQuery("NoExist");

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }
    }
}
