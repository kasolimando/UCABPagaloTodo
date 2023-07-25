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
using Microsoft.Azure.Amqp.Transaction;

namespace UCABPagaloTodoMS.Tests.UnitTestsMS.Controllers
{
    public class DeudasControllerTests
    {
        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        private readonly Mock<IMediator> _mediator;
        private readonly DeudasController _controller;
        private readonly Mock<ILogger<DeudasController>> _logger;

        private IFormFile deuda = BuildDataContextFaker.BuildArchivo();


        public DeudasControllerTests()
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

        //Unit Test Success AgregarDeuda
        [Fact]
        public async Task Controller_AgregarDeuda_SholudReturnAOkResult()
        {
            //ARRANGE

            //ACT
            var result = await _controller.AgregarDeuda(deuda);

            //ASSERT
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status201Created, statusCodeResult.StatusCode);
        }

        //Unit Test Failed AgregarDeuda for Exception
        [Fact]
        public async Task Controller_AgregarDeuda_SholudReturnABadRequestResult()
        {
            //ARRANGE
            _mediator.Setup(m => m.Send(It.IsAny<AgregarDeudaCommand>(), default)).ThrowsAsync(new CustomException(new() { "" }));

            //ACT
            var result = await _controller.AgregarDeuda(deuda);

            //ASSERT
            Assert.IsType<BadRequestObjectResult>(result);
        }

        //Unit Test Success ConsultaDeudas
        [Fact]
        public async Task Controller_ConsultaDeudas_SholudReturnAOkResult()
        {
            //ARRANGE

            //ACT
            var result = await _controller.ConsultaDeudas("servicio3");

            //ASSERT
            Assert.IsType<OkObjectResult>(result.Result);
        }

        //Unit Test Failed ConsultaDeudas for Exception
        [Fact]
        public async Task Controller_ConsultaDeudas_SholudReturnANotFound()
        {
            //ARRANGE
            _mediator.Setup(m => m.Send(It.IsAny<ConsultarDeudasQuery>(), default)).ThrowsAsync(new CustomException(new() { "" }));

            //ACT
            var result = await _controller.ConsultaDeudas("servicio3");

            //ASSERT
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }
    }
}