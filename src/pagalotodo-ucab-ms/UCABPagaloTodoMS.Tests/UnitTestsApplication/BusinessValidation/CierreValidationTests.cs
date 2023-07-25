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
    public class CierreValidationTests
    {
        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        //Create a mock of IDbContextTransactionProxy to mock the transactions
        private readonly Mock<IDbContextTransactionProxy> transactionMock;

        public CierreValidationTests()
        {
            _dbContextMock = new();

            _dbContextMock.SetupDbContextData();
            transactionMock = new();
        }

        //Unit Test Success in try
        [Fact]
        public void TestValidateCierre_Success()
        {
            //ARRANGE
            var pago = BuildDataContextFaker.BuildPago();

            var formato = BuildDataContextFaker.BuildFormato();

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges("hola", default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ACT
            var result = CierreValidation.Campo(pago, formato);

            //ASSERT
            Assert.IsType<string>(result);
        }

    }
}