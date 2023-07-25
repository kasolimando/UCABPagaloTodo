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
    public class PrestadoresControllerTests
    {
        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        private readonly Mock<IMediator> _mediator;
        private readonly PrestadoresController _controller;
        private readonly Mock<ILogger<PrestadoresController>> _logger;

        //Create a new PrestadorRequest with Random Values
        private PrestadorRequest prestador = BuildDataContextFaker.BuildPrestadorRequest();


        public PrestadoresControllerTests()
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
        public async Task Controller_ConsultarPrestador_SholudReturnAOkResult()
        {
            //ARRANGE

            //ACT
            var result = await _controller.ConsultaPrestador("username8");

            //ASSERT
            Assert.IsType<OkObjectResult>(result.Result);
        }

        //Unit Test Success Failed by null username
        [Fact]
        public async Task Controller_ConsultarPrestador_SholudReturnANotFound()
        {
            //ARRANGE

            _mediator.Setup(m => m.Send(It.IsAny<ConsultarPrestadorQuery>(), default)).ThrowsAsync(new CustomException(new() { "" }));

            //ACT
            var result = await _controller.ConsultaPrestador(null);

            //ASSERT
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        //Unit Test Success ConsultaServiciosPorPrest
        [Fact]
        public async Task Controller_ConsultaServiciosPorPrest_SholudReturnAOkResult()
        {
            //ARRANGE

            //ACT
            var result = await _controller.ConsultaServiciosPorPrest("username8");

            //ASSERT
            Assert.IsType<OkObjectResult>(result.Result);
        }

        //Unit Test Success Failed by null username
        [Fact]
        public async Task Controller_ConsultaServiciosPorPrest_SholudReturnANotFound()
        {
            //ARRANGE

            _mediator.Setup(m => m.Send(It.IsAny<ConsultarServicioQuery>(), default)).ThrowsAsync(new CustomException(new() { "" }));

            //ACT
            var result = await _controller.ConsultaServiciosPorPrest(null);

            //ASSERT
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        //Unit Test Success UpdateStatus Prestador
        [Fact]
        public async Task Controller_UpdateStatusPrestador_SholudReturnAOkResult()
        {
            //ARRANGE

            var status = BuildDataContextFaker.BuildStatusUserRequest();

            //ACT
            var result = await _controller.UpdateStatusPrestador(status, "username5");

            //ASSERT
            Assert.IsType<OkObjectResult>(result);
        }

        //Unit Test Failed UpdateStatus Prestador
        [Fact]
        public async Task Controller_UpdateStatusPrestador_SholudReturnAConflict()
        {
            //ARRANGE

            var status = BuildDataContextFaker.BuildStatusUserRequest();

            _mediator.Setup(m => m.Send(It.IsAny<StatusUserCommand>(), default)).ThrowsAsync(new CustomException(new() { "" }));

            //ACT
            var result = await _controller.UpdateStatusPrestador(status, "NoExist");

            //ASSERT
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status409Conflict, statusCodeResult.StatusCode);
        }

        //Unit Test Success Add
        [Fact]
        public async Task Controller_AddConsumidor_SholudReturnACreated()
        {
            //ARRANGE

            prestador = BuildDataContextFaker.BuildPrestadorRequestWithEspecificEmailAndUsername("username7@gmail.com", "username7");

            //ACT
            var result = await _controller.AgregarPrestador(prestador);

            //ASSERT
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status201Created, statusCodeResult.StatusCode);
        }

        //Unit Test Failed Add by existent name
        [Fact]
        public async Task Controller_AgregarConsumidor_SholudReturnABadRequest()
        {
            //ARRANGE

            prestador = BuildDataContextFaker.BuildPrestadorRequestWithEspecificEmailAndUsername("username9@gmail.com", "username9");

            _mediator.Setup(m => m.Send(It.IsAny<AgregarPrestadorCommand>(), default)).ThrowsAsync(new CustomException(new() { "" }));

            //ACT
            var result = await _controller.AgregarPrestador(prestador);

            //ASSERT
            Assert.IsType<BadRequestObjectResult>(result);
        }

        //Unit Test Success Cambio de Clave
        [Fact]
        public async Task Controller_CambioClaveAdministrador_SholudReturnAOkResult()
        {
            //ARRANGE

            var cambio = BuildDataContextFaker.BuildCambioClaveUserRequestWithSpecificInfo("username9");

            //ACT
            var result = await _controller.CambioClavePrestador(cambio, "username9");

            //ASSERT
            Assert.IsType<OkObjectResult>(result);
        }

        //Unit Test Failed Cambio de Clave for Exception
        [Fact]
        public async Task Controller_CambioClavePrestador_SholudReturnABadResult()
        {
            //ARRANGE

            var cambio = BuildDataContextFaker.BuildCambioClaveUserRequestWithSpecificInfo("username9");

            _mediator.Setup(m => m.Send(It.IsAny<CambioClaveUserCommand>(), default)).ThrowsAsync(new CustomException(new() { "" }));

            //ACT
            var result = await _controller.CambioClavePrestador(cambio, "username3");

            //ASSERT
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status409Conflict, statusCodeResult.StatusCode);
        }

        //Unit Test Success UpdatePrestador Prestador
        [Fact]
        public async Task Controller_UpdatePrestador_SholudReturnAOkResult()
        {
            //ARRANGE

            var status = BuildDataContextFaker.BuildStatusUserRequest();

            //ACT
            var result = await _controller.UpdatePrestador(prestador, "username9");

            //ASSERT
            Assert.IsType<OkObjectResult>(result);
        }

        //Unit Test Failed UpdatePrestador Prestador
        [Fact]
        public async Task Controller_UpdatePrestador_SholudReturnAConflict()
        {
            //ARRANGE

            _mediator.Setup(m => m.Send(It.IsAny<UpdatePrestadoresCommand>(), default)).ThrowsAsync(new CustomException(new() { "" }));

            //ACT
            var result = await _controller.UpdatePrestador(prestador, "username9");

            //ASSERT
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status409Conflict, statusCodeResult.StatusCode);
        }
    }
}