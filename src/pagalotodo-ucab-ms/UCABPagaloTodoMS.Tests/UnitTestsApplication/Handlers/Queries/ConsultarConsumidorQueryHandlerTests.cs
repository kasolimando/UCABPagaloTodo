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
    public class ConsultarConsumidorQueryHandlerTests
    {
        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        private readonly Mock<IMediator> _mediator;
        private readonly ConsultarConsumidoresQueryHandler _handler;
        private readonly Mock<ILogger<ConsultarConsumidorQuery>> _logger;
        //Create a mock of IDbContextTransactionProxy to mock the transactions
        private readonly Mock<IDbContextTransactionProxy> transactionMock;


        public ConsultarConsumidorQueryHandlerTests()
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
        public async Task Handle_ConsultarConsumidor_SholudThorwExceptionEmptyRequest()
        {
            //ARRANGE

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(null, CancellationToken.None));
        }

        //Unit Test Success just the consumidor
        [Fact]
        public async Task Handle_ConsultarConsultar_SholudReturnTheConsumidor()
        {
            //ARRANGE

            //Create a new request
            var request = new ConsultarConsumidorQuery("username5");

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ACT
            var result = await _handler.Handle(request, CancellationToken.None);

            //ASSERT
            Assert.IsType<List<ConsumidoresResponse>>(result);
        }

        //Unit Test Success all consumidores
        [Fact]
        public async Task Handle_ConsultarConsultar_SholudReturnAllConsumidores()
        {
            //ARRANGE

            //Create a new request
            var request = new ConsultarConsumidorQuery(null);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ACT
            var result = await _handler.Handle(request, CancellationToken.None);

            //ASSERT
            Assert.IsType<List<ConsumidoresResponse>>(result);
        }

        //Unit Test Failed non existent consumidor
        [Fact]
        public async Task Handle_ConsultarConsultar_ShoulddThrowAExceptionNoUser()
        {
            //ARRANGE

            //Create a new request
            var request = new ConsultarConsumidorQuery("NoExist");

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }
    }
}
