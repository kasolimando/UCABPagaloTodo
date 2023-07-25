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

namespace UCABPagaloTodoMS.Tests.UnitTests.Controllers
{
    public class ConsumidoresControllerTests
    {
        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        private readonly Mock<IMediator> _mediator;
        private readonly ConsumidoresController _controller;
        private readonly Mock<ILogger<ConsumidoresController>> _logger;

        //Create a new ConsumidorRequest with Random Values
        private ConsumidorRequest consumidor = BuildDataContextFaker.BuildConsumidorRequest();


        public ConsumidoresControllerTests()
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

        //Unit Test Success Consult
        [Fact]
        public async Task Controller_ConsultarConsumidor_SholudReturnAOkResult()
        {
            //ARRANGE

            //ACT
            var result = await _controller.ConsultaConsumidor("username4");

            //ASSERT
            Assert.IsType<OkObjectResult>(result.Result);
        }

        //Unit Test Success Failed by null username
        [Fact]
        public async Task Controller_ConsultarConsumidor_SholudReturnANotFound()
        {
            //ARRANGE

            _mediator.Setup(m => m.Send(It.IsAny<ConsultarConsumidorQuery>(), default)).ThrowsAsync(new CustomException(new() { "" }));

            //ACT
            var result = await _controller.ConsultaConsumidor("NoExist");

            //ASSERT
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        //Unit Test Success Update
        [Fact]
        public async Task Controller_UpdateConsumidor_SholudReturnAOkResult()
        {
            //ARRANGE

            consumidor = BuildDataContextFaker.BuildConsumidorRequestWithEspecificEmailAndUsername("username5@gmail.com", "username5");

            //ACT
            var result = await _controller.UpdateConsumidor(consumidor, "username5");

            //ASSERT
            Assert.IsType<OkObjectResult>(result);
        }

        //Unit Test Failed Update by non Existent User
        [Fact]
        public async Task Controller_UpdateConsumidor_SholudReturnAConflict()
        {
            //ARRANGE

            consumidor = BuildDataContextFaker.BuildConsumidorRequestWithEspecificEmailAndUsername("NoExist", "NoExist");

            _mediator.Setup(m => m.Send(It.IsAny<UpdateConsumCommand>(), default)).ThrowsAsync(new CustomException(new() { "" }));

            //ACT
            var result = await _controller.UpdateConsumidor(consumidor, "NoExist");

            //ASSERT
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status409Conflict, statusCodeResult.StatusCode);
        }

        //Unit Test Success UpdateStatus consumidor
        [Fact]
        public async Task Controller_UpdateStatusConsumidor_SholudReturnAOkResult()
        {
            //ARRANGE

            var status = BuildDataContextFaker.BuildStatusUserRequest();

            //ACT
            var result = await _controller.UpdateStatusConsumidor(status,"username6");

            //ASSERT
            Assert.IsType<OkObjectResult>(result);
        }

        //Unit Test Success UpdateStatus consumidor
        [Fact]
        public async Task Controller_UpdateStatusconsumidor_SholudReturnAConflic()
        {
            //ARRANGE

            var status = BuildDataContextFaker.BuildStatusUserRequest();

            _mediator.Setup(m => m.Send(It.IsAny<StatusUserCommand>(), default)).ThrowsAsync(new CustomException(new() { "" }));

            //ACT
            var result = await _controller.UpdateStatusConsumidor(status, "NoExist");

            //ASSERT
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status409Conflict, statusCodeResult.StatusCode);
        }

        //Unit Test Success Add
        [Fact]
        public async Task Controller_AddConsumidor_SholudReturnACreated()
        {
            //ARRANGE

            consumidor = BuildDataContextFaker.BuildConsumidorRequestWithEspecificEmailAndUsername("username7@gmail.com", "username7");

            //ACT
            var result = await _controller.AgregarConsumidor(consumidor);

            //ASSERT
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status201Created, statusCodeResult.StatusCode);
        }

        //Unit Test Failed Add by existent name
        [Fact]
        public async Task Controller_AgregarConsumidor_SholudReturnABadRequest()
        {
            //ARRANGE

            consumidor = BuildDataContextFaker.BuildConsumidorRequestWithEspecificEmailAndUsername("username5@gmail.com", "username5");

            _mediator.Setup(m => m.Send(It.IsAny<AgregarConsumidorCommand>(), default)).ThrowsAsync(new CustomException(new() { "" }));

            //ACT
            var result = await _controller.AgregarConsumidor(consumidor);

            //ASSERT
            Assert.IsType<BadRequestObjectResult>(result);
        }

        //Unit Test Success Cambio de Clave
        [Fact]
        public async Task Controller_CambioClaveConsumidor_SholudReturnAOkResult()
        {
            //ARRANGE

            var cambio = BuildDataContextFaker.BuildCambioClaveUserRequestWithSpecificInfo("username4");

            //ACT
            var result = await _controller.CambioClaveConsumidor(cambio, "username4");

            //ASSERT
            Assert.IsType<OkObjectResult>(result);
        }

        //Unit Test Failed Cambio de Clave for Exception
        [Fact]
        public async Task Controller_CambioClaveConsumidor_SholudReturnAConflict()
        {
            //ARRANGE

            var cambio = BuildDataContextFaker.BuildCambioClaveUserRequestWithSpecificInfo("username4");

            _mediator.Setup(m => m.Send(It.IsAny<CambioClaveUserCommand>(), default)).ThrowsAsync(new CustomException(new() { "" }));

            //ACT
            var result = await _controller.CambioClaveConsumidor(cambio, "username3");

            //ASSERT
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status409Conflict, statusCodeResult.StatusCode);
        }

        //Unit Test Success Cambio de Clave
        [Fact]
        public async Task Controller_ConsultaDeudas_SholudReturnAOkResult()
        {
            //ARRANGE

            //ACT
            var result = await _controller.ConsultaDeudas( "username4");

            //ASSERT
            Assert.IsType<OkObjectResult>(result.Result);
        }

        //Unit Test Failed Cambio de Clave for Exception
        [Fact]
        public async Task Controller_ConsultaDeudas_SholudReturnANotFound()
        {
            //ARRANGE

            _mediator.Setup(m => m.Send(It.IsAny<ConsultarDeudasQuery>(), default)).ThrowsAsync(new CustomException(new() { "" }));

            //ACT
            var result = await _controller.ConsultaDeudas("username4");

            //ASSERT
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }
    }
}