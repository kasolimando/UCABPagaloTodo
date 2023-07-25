using Moq;
using UCABPagaloTodoMS.Core.Database;
using Xunit;
using UCABPagaloTodoMS.Tests.DataSeed;
using UCABPagaloTodoMS.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Queries;
using Microsoft.AspNetCore.Mvc;
using UCABPagaloTodoMS.Tests.MockData;
using UCABPagaloTodoMS.Controllers;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace UCABPagaloTodoMS.Tests.UnitTestsMS.Controllers
{
    public class RecuperarClaveControllerTests
    {
        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        private readonly Mock<IMediator> _mediator;
        private readonly RecuperarClaveController _controller;
        private readonly Mock<ILogger<RecuperarClaveController>> _logger;

        public RecuperarClaveControllerTests()
        {
            _dbContextMock = new();

            _dbContextMock.SetupDbContextData();

            _mediator = new();
            _logger = new();
            _controller = new(_logger.Object, _mediator.Object);
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.ActionDescriptor = new ControllerActionDescriptor();
        }

        //Unit Test Success RecuperarClave
        [Fact]
        public async Task Controller_RecuperarClave_SholudReturnAOkResult()
        {
            //ARRANGE

            //ACT
            var result = await _controller.RecuperarClave("username5");

            //ASSERT
            Assert.IsType<OkObjectResult>(result);
        }

        //Unit Test Failed RecuperarClave
        [Fact]
        public async Task Controller_RecuperarClave_SholudReturnAConflict()
        {
            //ARRANGE
            _mediator.Setup(m => m.Send(It.IsAny<RecuperarClaveCommand>(), default)).ThrowsAsync(new CustomException(new() { "" }));

            //ACT
            var result = await _controller.RecuperarClave("username5");

            //ASSERT
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status409Conflict, statusCodeResult.StatusCode);
        }

    }
}
