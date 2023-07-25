using Moq;
using UCABPagaloTodoMS.Application.BusinessValidation;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Tests.MockData;
using Xunit;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Tests.DataSeed;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.BusinessValidation.Interfaces;
using UCABPagaloTodoMS.Application.BusinessValidation.Implementation;

namespace UCABPagaloTodoMS.Tests.UnitTestsApplication.BusinessValidation.Implementation
{
    public class ConsumidorStatusUserValidationTests
    {
        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        private readonly IStatusUserValidator statusValidation;
        //Create a mock of IDbContextTransactionProxy to mock the transactions
        private readonly Mock<IDbContextTransactionProxy> transactionMock;


        public ConsumidorStatusUserValidationTests()
        {
            _dbContextMock = new();

            _dbContextMock.SetupDbContextData();

            statusValidation = new ConsumidorStatusUserValidation();
            transactionMock = new();
        }

        //Unit Test success consumdior
        [Fact]
        public async Task TestStatusAdmin_ShouldReturnStringEstatusTrue()
        {
            //ARRANGE
            var data = BuildDataContextFaker.BuildStatusUserRequestEspecific(true);

            var request = new StatusUserCommand(data,new ConsumidorStatusUserValidation(), "usernameC");

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ACT
            var result = await statusValidation.ValidateStatusUser(request, _dbContextMock.Object);

            //ASSERT
            Assert.IsType<string>(result);
        }

        //Unit Test success consumdior
        [Fact]
        public async Task TestStatusAdmin_ShouldReturnStringEstatusFalse()
        {
            //ARRANGE
            var data = BuildDataContextFaker.BuildStatusUserRequestEspecific(false);

            var request = new StatusUserCommand(data, new ConsumidorStatusUserValidation(), "username6");

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ACT
            var result = await statusValidation.ValidateStatusUser(request, _dbContextMock.Object);

            //ASSERT
            Assert.IsType<string>(result);
        }
    }
}