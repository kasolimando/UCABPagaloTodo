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
using System.Text;

namespace UCABPagaloTodoMS.Tests.UnitTestsMS.Controllers
{
    public class ConciliacionControllerTests
    {
        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        private readonly Mock<IMediator> _mediator;
        private readonly ConciliacionesController _controller;
        private readonly Mock<ILogger<ConciliacionesController>> _logger;

        private IFormFile conciliacion = BuildDataContextFaker.BuildArchivo();

        public ConciliacionControllerTests()
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

        //Unit Test Success Conciliacion
        [Fact]
        public async Task Controller_Conciliacion_SholudReturnACreated()
        {
            //ARRANGE
            //ACT
            var result = await _controller.Conciliacion(conciliacion);

            //ASSERT
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status201Created, statusCodeResult.StatusCode);
        }

        //Unit Test Failed Conciliacion for Exception
        [Fact]
        public async Task Controller_Conciliacion_SholudReturnABadRequestResult()
        {
            //ARRANGE
            _mediator.Setup(m => m.Send(It.IsAny<CargarConciliacionCommand>(), default)).ThrowsAsync(new CustomException(new() { "" }));

            //ACT
            var result = await _controller.Conciliacion(conciliacion);

            //ASSERT
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}