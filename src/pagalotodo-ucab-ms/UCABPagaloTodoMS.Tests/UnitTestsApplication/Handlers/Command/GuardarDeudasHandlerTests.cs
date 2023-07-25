using Moq;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Tests.MockData;
using Xunit;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Tests.DataSeed;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Handlers.Commands;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Requests;

namespace UCABPagaloTodoMS.Tests.UnitTestsApplication.Handlers.Command
{
    public class GuardarDeudasHandlerTests
    {
        private readonly GuardarDeudasHandler _handler;
        private readonly Mock<ILogger<GuardarDeudasHandler>> _logger;
        //Create a mock of IDbContextTransactionProxy to mock the transactions
        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        private readonly Mock<IDbContextTransactionProxy> transactionMock;

        private GuardarDeudaRequest deuda = BuildDataContextFaker.BuildGuardarDeudaRequest();

        public GuardarDeudasHandlerTests()
        {
            _dbContextMock = new();

            _dbContextMock.SetupDbContextData();

            _logger = new();
            _handler = new(_dbContextMock.Object, _logger.Object);
            transactionMock = new();
        }

        //Unit Test Success Admin
        [Fact]
        public async Task Handle_GuardarDeuda_SholudReturnGuid()
        {
            //ARRANGE

            //Create a new request
            var request = new GuardarDeudasCommand(deuda);


            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(deuda.Servicio, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ACT
            var result = await _handler.Handle(request, CancellationToken.None);

            //ASSERT
            Assert.IsType<Guid>(result);
        }

        //Unit Test Failed 
        [Fact]
        public async Task Handle_GuardarConciliacion_ShouldThrowExceptionNullrequestl()
        {
            deuda.Servicio = string.Empty;
            //Create a new request
            var request = new GuardarDeudasCommand(deuda);

            //ACT
            var result = await _handler.Handle(request, CancellationToken.None);

            //ASSERT
            Assert.IsType<Guid>(result);
        }

        //Unit Test Failed 
        [Fact]
        public async Task Handle_GuardarConciliacion_SholudThoException()
        {
            //ARRANGE

            //Create a new request
            var request = new GuardarDeudasCommand(deuda);


            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Throws(new Exception());

            //ACT
            var result = await _handler.Handle(request, CancellationToken.None);

            //ASSERT
            Assert.IsType<Guid>(result);
        }
    }
}