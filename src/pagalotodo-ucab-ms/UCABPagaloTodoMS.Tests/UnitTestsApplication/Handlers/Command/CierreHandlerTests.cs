using Moq;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Tests.MockData;
using Xunit;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Tests.DataSeed;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Handlers.Commands;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.BusinessValidation.Implementation;
using Microsoft.AspNetCore.Http;
using UCABPagaloTodoMS.Infrastructure.Services.RabbitProducer;

namespace UCABPagaloTodoMS.Tests.UnitTestsApplication.Handlers.Command
{
    public class CierreHandlerTests
    {
        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        private readonly CierreHadler _handler;
        private readonly Mock<ILogger<CierreCommand>> _logger;
        private readonly Mock<IDbContextTransactionProxy> transactionMock;

        public CierreHandlerTests()
        {
            _dbContextMock = new();

            _dbContextMock.SetupDbContextData();


            _logger = new();
            transactionMock = new();
            _handler = new(_dbContextMock.Object, _logger.Object);
        }

        //Unit Test Success Admin
        [Fact]
        public async Task Handle_CierreConciliacion_SholudReturnok()
        {
            //ARRANGE

            //Create a new request
            var request = new CierreCommand("servicio3");

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(request.Servicio, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);


            //ACT
            var result = await _handler.Handle(request, CancellationToken.None);

            //ASSERT
            Assert.IsType<string>(result);
        }

        //Unit Test Failed 
        [Fact]
        public async Task Handle_CargarConciliacion_ShouldThrowExceptionNullrequestl()
        {

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(null, CancellationToken.None));
        }

        //Unit Test Failed 
        [Fact]
        public async Task Handle_CierreConciliacion_SholudThowSQLException()
        {
            //ARRANGE

            //Create a new request
            var request = new CierreCommand("NoExist");

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed 
        [Fact]
        public async Task Handle_CierreConciliacion_SholudThowCustomException()
        {
            //ARRANGE

            //Create a new request
            var request = new CierreCommand("servicio2");

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed 
        [Fact]
        public async Task Handle_CierreConciliacion_SholudThowException()
        {
            //ARRANGE

            //Create a new request
            var request = new CierreCommand("servicio4");

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Throws(new Exception());

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }
    }
}