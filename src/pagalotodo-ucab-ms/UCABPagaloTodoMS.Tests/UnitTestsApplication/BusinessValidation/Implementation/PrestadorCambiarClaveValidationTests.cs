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
    public class PrestadorCambiarClaveValidationTests
    {
        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        private readonly ICambiarClaveUser cambiarClaveValidation;
        //Create a mock of IDbContextTransactionProxy to mock the transactions
        private readonly Mock<IDbContextTransactionProxy> transactionMock;


        public PrestadorCambiarClaveValidationTests()
        {
            _dbContextMock = new();

            _dbContextMock.SetupDbContextData();

            cambiarClaveValidation = new PrestadorCambiarClaveValidation();
            transactionMock = new();
        }

        //Unit Test failed Admin
        [Fact]
        public async Task TestCambiarClaveAdmin_ShouldThrowExceptionDifferentClave()
        {
            //ARRANGE

            //Create a new StatusUserRequest with Random Values
            var request = BuildDataContextFaker.BuildCambioClaveUserRequestWithSpecificInfo("Error");

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            var data = new CambioClaveUserCommand(request, new PrestadorCambiarClaveValidation(), "username9");

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await cambiarClaveValidation.ValidateCambioClaveUser(data, _dbContextMock.Object));

        }

        //Unit Test failed Admin
        [Fact]
        public async Task TestCambiarClaveAdmin_ShouldReturnaString()
        {
            //ARRANGE

            //Create a new StatusUserRequest with Random Values
            var request = BuildDataContextFaker.BuildCambioClaveUserRequestWithSpecificInfo("username9");

            //Configure the dbContextMock object to do nothing when SaveEfContextChanges() is called
            _dbContextMock.Setup(x => x.SaveEfContextChanges("username9", default)).ReturnsAsync(true);

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            var data = new CambioClaveUserCommand(request, new ConsumidorCambiarClaveValidation(), "username9");

            //ACT
            var result = await cambiarClaveValidation.ValidateCambioClaveUser(data, _dbContextMock.Object);

            //ASSERT
            Assert.IsType<string>(result);

        }
    }
}