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
    public class AgregarFormatoHandlerTests
    {
        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        private readonly Mock<IMediator> _mediator;
        private readonly AgregarFormatoHandler _handler;
        private readonly Mock<ILogger<AgregarFormatoCommand>> _logger;
        //Create a mock of IDbContextTransactionProxy to mock the transactions
        private readonly Mock<IDbContextTransactionProxy> transactionMock;

        //Create a new FormatosRequest with Random Values
        private FormatosRequest formato = BuildDataContextFaker.BuildFormatosRequest();


        public AgregarFormatoHandlerTests()
        {
            _dbContextMock = new();

            _dbContextMock.SetupDbContextData();

            _mediator = new();
            _logger = new();
            _handler = new(_dbContextMock.Object, _logger.Object);
            transactionMock = new();
        }

        //Unit Test Success
        [Fact]
        public async Task Handle_AgregarFormato_ShouldReturnTheName()
        {
            //ARRANGE

            formato = BuildDataContextFaker.BuildFormatosRequestWithEspecifiServicio("servicio2");

            //Create a new request
            var request = new AgregarFormatoCommand(formato);

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(formato.Servicio, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ACT
            var result = await _handler.Handle(request, CancellationToken.None);

            //ASSERT
            Assert.NotNull(result);
        }

        //Unit Test Failed by Non Exist Servicio
        [Fact]
        public async Task Handle_AgregarFormato_SholudThrowException()
        {
            //ARRANGE

            formato = BuildDataContextFaker.BuildFormatosRequestWithEspecifiServicio("NoCumplo");

            //Create a new request
            var request = new AgregarFormatoCommand(formato);

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(formato.Servicio, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed by Empty Request
        [Fact]
        public async Task Handle_AgregarFormato_ShouldThrowExceptionFormEmptyRequest()
        {
            //ARRANGE

            //Create a new request
            var request = new AgregarFormatoCommand(null);

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(formato.Servicio, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed by validator
        [Fact]
        public async Task Handle_AgregarFormato_SholudThrowExceptionByValidator()
        {
            //ARRANGE

            formato = BuildDataContextFaker.BuildFormatosRequestWithEspecifiServicio(null);

            //Create a new request
            var request = new AgregarFormatoCommand(formato);

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(formato.Servicio, default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed by Non Exist Servicio
        [Fact]
        public async Task Handle_AgregarFormato_SholudThrowExceptionGerenal()
        {
            //ARRANGE

            formato = BuildDataContextFaker.BuildFormatosRequestWithEspecifiServicio("servicio1");

            //Create a new request
            var request = new AgregarFormatoCommand(formato);

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges(formato.Servicio, default)).Throws(new Exception());

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed by Non Exist Servicio
        [Fact]
        public async Task Handle_AgregarFormato_SholudThrowCustomException()
        {
            //ARRANGE

            formato = BuildDataContextFaker.BuildFormatosRequestWithEspecifiServicio("servicio3");

            //Create a new request
            var request = new AgregarFormatoCommand(formato);

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }
    }
}