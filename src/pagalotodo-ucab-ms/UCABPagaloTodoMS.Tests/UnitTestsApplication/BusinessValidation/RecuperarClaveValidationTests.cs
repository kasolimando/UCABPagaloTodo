using Moq;
using UCABPagaloTodoMS.Application.BusinessValidation;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Tests.MockData;
using Xunit;
using FluentValidation;
using MailKit.Net.Smtp;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Tests.DataSeed;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Core.Entities;
using UCABPagaloTodoMS.Application.Requests;
using MimeKit;
using static UCABPagaloTodoMS.Core.Entities.ServicioEntity;

namespace UCABPagaloTodoMS.Tests.UnitTestsApplication.BusinessValidation
{
    public class RecuperarClaveValidationTests
    {
        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        //Create a mock of IDbContextTransactionProxy to mock the transactions
        private readonly Mock<IDbContextTransactionProxy> transactionMock;
        private readonly RecuperarClaveValidation recuperarClaveValidation;

        public RecuperarClaveValidationTests()
        {
            _dbContextMock = new();

            _dbContextMock.SetupDbContextData();
            transactionMock = new();
            recuperarClaveValidation = new(_dbContextMock.Object);
        }

        //Unit Test Success in try
        [Fact]
        public async void TestValidateConsumidor_Success()
        {
            //ARRANGE

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges("hola", default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ACT
            var result = await recuperarClaveValidation.GetUser("username4");

            //ASSERT
            Assert.IsType<ConsumidorEntity>(result);
        }

        //Unit Test Success in try
        [Fact]
        public async void TestValidatePrestador_Success()
        {
            //ARRANGE

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges("hola", default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ACT
            var result = await recuperarClaveValidation.GetUser("username8");

            //ASSERT
            Assert.IsType<PrestadorEntity>(result);
        }

    }
}