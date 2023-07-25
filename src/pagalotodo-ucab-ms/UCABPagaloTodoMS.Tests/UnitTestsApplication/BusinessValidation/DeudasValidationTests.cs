using Moq;
using UCABPagaloTodoMS.Application.BusinessValidation;
using UCABPagaloTodoMS.Core.Database;
using Xunit;
using UCABPagaloTodoMS.Tests.DataSeed;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Tests.MockData;

namespace UCABPagaloTodoMS.Tests.UnitTestsApplication.BusinessValidation
{
    public class DeudasValidationTests
    {
        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        //Create a mock of IDbContextTransactionProxy to mock the transactions
        private readonly Mock<IDbContextTransactionProxy> transactionMock;

        public DeudasValidationTests()
        {
            _dbContextMock = new();

            _dbContextMock.SetupDbContextData();
            transactionMock = new();
        }

        //Unit Test Success
        [Fact]
        public void TestValidateDeuda_ShouldThrowSQLException()
        {
            //ARRANGE

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ACT
            var act = () => DeudasValidation.HaveDeudasPendientesConsumidor(_dbContextMock.Object, "username6");

            //ASSERT
            Assert.IsNotType<SQLException>(act);
        }

        //Unit Test Failed
        [Fact]
        public void TestValidateDeuda_ShouldntThrowSQLExeption()
        {
            //ARRANGE

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);


            //ASSERT
            Assert.Throws<SQLException>(() => DeudasValidation.HaveDeudasPendientesConsumidor(_dbContextMock.Object, "username4"));

        }

        //Unit Test Failed
        [Fact]
        public async Task TestGuardarDeuda_ShouldntThrowExceptionNoUser()
        {
            //ARRANGE
            var request = BuildDataContextFaker.BuildGuardarDeudaRequestBadMonto("NoExist","90");

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ACT
            var result = await DeudasValidation.GuardarDeuda(_dbContextMock.Object, request);

            //ASSERT
            Assert.IsType<Guid>(result);

        }

        //Unit Test Failed
        [Fact]
        public async Task TestGuardarDeuda_ShouldntThrowExceptionNoMonto()
        {
            //ARRANGE
            var request = BuildDataContextFaker.BuildGuardarDeudaRequestBadMonto("username5","-90");

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ACT
            var result = await DeudasValidation.GuardarDeuda(_dbContextMock.Object, request);

            //ASSERT
            Assert.IsType<Guid>(result);

        }

        //Unit Test Failed
        [Fact]
        public async Task TestGuardarDeuda_ShouldntThrowExceptionDeudaNull()
        {
            //ARRANGE
            var request = BuildDataContextFaker.BuildGuardarDeudaRequestBadMonto("username6","90");

            //Configure the dbContextMock object for the transactions (BeginTransaction, commit and rollback)
            _dbContextMock.Setup(x => x.BeginTransaction()).Returns(transactionMock.Object);

            //ACT
            var result = await DeudasValidation.GuardarDeuda(_dbContextMock.Object, request);

            //ASSERT
            Assert.IsType<Guid>(result);

        }
    }
}