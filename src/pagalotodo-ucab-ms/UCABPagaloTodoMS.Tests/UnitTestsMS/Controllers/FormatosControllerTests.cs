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
    public class FormatosControllerTests
    {
        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        private readonly Mock<IMediator> _mediator;
        private readonly FormatosController _controller;
        private readonly Mock<ILogger<FormatosController>> _logger;

        private FormatosRequest FormatoRequest = BuildDataContextFaker.BuildFormatosRequest();

        public FormatosControllerTests()
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

        //Unit Test Success AgregarFormato
        [Fact]
        public async Task Controller_AgregarFormato_SholudReturnACreated()
        {
            //ARRANGE

            //ACT
            var result = await _controller.AgregarFormato(FormatoRequest);

            //ASSERT
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status201Created, statusCodeResult.StatusCode);
        }

        //Unit Test Failed AgregarFormato
        [Fact]
        public async Task Controller_AgregarFormato_SholudReturnABadRequest()
        {
            //ARRANGE

            _mediator.Setup(m => m.Send(It.IsAny<AgregarFormatoCommand>(), default)).ThrowsAsync(new CustomException(new() { "" }));

            //ACT
            var result = await _controller.AgregarFormato(FormatoRequest);

            //ASSERT
            Assert.IsType<BadRequestObjectResult>(result);
        }

        //Unit Test Success ConsultaFormatos
        [Fact]
        public async Task Controller_ConsultaFormatos_SholudReturnAOkResult()
        {
            //ARRANGE

            //ACT
            var result = await _controller.ConsultaFormatos("servicio1");

            //ASSERT
            Assert.IsType<OkObjectResult>(result.Result);
        }

        //Unit Test Failed ConsultaFormatos
        [Fact]
        public async Task Controller_ConsultaFormatos_SholudReturnANotFound()
        {
            //ARRANGE

            _mediator.Setup(m => m.Send(It.IsAny<ConsultarFormatoQuery>(), default)).ThrowsAsync(new CustomException(new() { "" }));

            //ACT
            var result = await _controller.ConsultaFormatos("servicio90");

            //ASSERT
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        //Unit Test Success UpdateFormato
        [Fact]
        public async Task Controller_UpdateFormato_SholudReturnAOkResult()
        {
            //ARRANGE

            //ACT
            var result = await _controller.UpdateFormato(FormatoRequest);

            //ASSERT
            Assert.IsType<OkObjectResult>(result);
        }

        //Unit Test Failed UpdateFormato
        [Fact]
        public async Task Controller_UpdateFormato_SholudReturnAConflict()
        {
            //ARRANGE

            _mediator.Setup(m => m.Send(It.IsAny<UpdateFormatosCommand>(), default)).ThrowsAsync(new CustomException(new() { "" }));

            //ACT
            var result = await _controller.UpdateFormato(FormatoRequest);

            //ASSERT
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status409Conflict, statusCodeResult.StatusCode);
        }

    }
}
