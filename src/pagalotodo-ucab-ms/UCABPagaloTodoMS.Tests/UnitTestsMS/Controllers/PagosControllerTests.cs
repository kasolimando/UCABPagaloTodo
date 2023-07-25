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
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Controllers;
using UCABPagaloTodoMS.Application.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace UCABPagaloTodoMS.Tests.UnitTestsMS.Controllers
{
    public class PagosControllerTests
    {
        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        private readonly Mock<IMediator> _mediator;
        private readonly PagosController _controller;
        private readonly Mock<ILogger<PagosController>> _logger;

        private PagoRequest pago = BuildDataContextFaker.BuildPagoRequest();

        public PagosControllerTests()
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

        //Unit Test Success AgregarPago
        [Fact]
        public async Task Controller_AgregarPago_SholudReturnACreated()
        {
            //ARRANGE

            //ACT
            var result = await _controller.AgregarPago(pago);

            //ASSERT
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status201Created, statusCodeResult.StatusCode);
        }

        //Unit Test Failed AgregarPago for Exception
        [Fact]
        public async Task Controller_AgregarPago_SholudReturnABadRequestResult()
        {
            //ARRANGE
            _mediator.Setup(m => m.Send(It.IsAny<AgregarPagoCommand>(), default)).ThrowsAsync(new CustomException(new() { "" }));

            //ACT
            var result = await _controller.AgregarPago(pago);

            //ASSERT
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}