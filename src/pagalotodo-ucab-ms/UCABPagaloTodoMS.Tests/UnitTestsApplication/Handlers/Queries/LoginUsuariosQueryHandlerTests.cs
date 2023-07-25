using Moq;
using UCABPagaloTodoMS.Core.Database;
using Xunit;
using UCABPagaloTodoMS.Tests.DataSeed;
using UCABPagaloTodoMS.Application.Exceptions;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Handlers.Queries;
using UCABPagaloTodoMS.Application.Queries;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using UCABPagaloTodoMS.Application.Responses;

namespace UCABPagaloTodoMS.Tests.UnitTests.Handlers.Queries
{
    public class LoginUsuariosQueryHandlerTests
    {
        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        private readonly LoginUsuariosQueryHandler _handler;
        private readonly Mock<ILogger<LoginUsuariosQuery>> _logger;
        private readonly Mock<IConfiguration> _config;


        public LoginUsuariosQueryHandlerTests()
        {
            _dbContextMock = new();

            _dbContextMock.SetupDbContextData();
            _config = new();
;            _logger = new();
            _handler = new(_dbContextMock.Object, _logger.Object, _config.Object);
        }

        //Unit Test Passed
        [Fact]
        public async Task Handle_LoginUsuarios_SholudReturnALoginResponse()
        {
            //ARRANGE

            //Create a new request
            var request = new LoginUsuariosQuery("username2","username1");

            _config.Setup(x => x.GetSection("Jwt:Key").Value).Returns("PagaloTodoUCAB12345");

            //ACT
            var result = await _handler.Handle(request, CancellationToken.None);

            //ASSERT
            Assert.IsType<LoginResponse>(result);
        }

        //Unit Test Failed
        [Fact]
        public async Task Handle_LoginUsuarios_SholudThrowExceptionForEmptyRequest()
        {
            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(null, CancellationToken.None));


        }

        //Unit Test Failed by a Non existent Username
        [Fact]
        public async Task Handle_ConsultarAdmin_ShouldThrowAExceptionValidator()
        {
            //ARRANGE

            //Create a new request
            var request = new LoginUsuariosQuery(null, "username1");

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Failed by a Non existent Username
        [Fact]
        public async Task Handle_ConsultarAdmin_ShouldThrowAExceptionNoUser()
        {
            //ARRANGE

            //Create a new request
            var request = new LoginUsuariosQuery("NoExist", "username1");

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Passed
        [Fact]
        public async Task Handle_LoginUsuarios_SholudThrowGeneralExceptual()
        {
            //ARRANGE

            //Create a new request
            var request = new LoginUsuariosQuery("username1", "username1");

            _config.Setup(x => x.GetSection("Jwt:Key").Value).Throws(new Exception());

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }

        //Unit Test Passed
        [Fact]
        public async Task Handle_LoginUsuarios_SholudThrowcustomExceptual()
        {
            //ARRANGE

            //Create a new request
            var request = new LoginUsuariosQuery("username1", "username1");

            _config.Setup(x => x.GetSection("Jwt:Key").Value).Throws(new CustomException(new() { ""}));

            //ASSERT
            await Assert.ThrowsAsync<CustomException>(async () => await _handler.Handle(request, CancellationToken.None));
        }
    }
}
