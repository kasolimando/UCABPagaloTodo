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

namespace UCABPagaloTodoMS.Tests.UnitTests.Controllers
{
    public class ServiciosControllerTests
    {
        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        private readonly Mock<IMediator> _mediator;
        private readonly ServiciosController _controller;
        private readonly Mock<ILogger<ServiciosController>> _logger;

        //Create a new ServicioRequest with Random Values
        private ServicioRequest servicio = BuildDataContextFaker.BuildServicioRequest();


        public ServiciosControllerTests()
        {
            _dbContextMock = new();

            _dbContextMock.SetupDbContextData();

            _mediator = new();
            _logger = new();
            _controller = new(_logger.Object, _mediator.Object);
        }

        //Unit Test Success Consult
        [Fact]
        public async Task Controller_ConsultarServicio_SholudReturnAOkResult()
        {
            //ARRANGE

            //ACT
            var result = await _controller.ConsultaServicio("servicio1");

            //ASSERT
            Assert.IsType<OkObjectResult>(result.Result);
        }

        //Unit Test Success Failed by null name
        [Fact]
        public async Task Controller_ConsultarServicio_SholudReturnANotFound()
        {
            //ARRANGE

            _mediator.Setup(m => m.Send(It.IsAny<ConsultarServicioQuery>(), default)).ThrowsAsync(new CustomException(new() { "" }));

            //ACT
            var result = await _controller.ConsultaServicio("NoExist");

            //ASSERT
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        //Unit Test Success Update
        [Fact]
        public async Task Controller_UpdateServicio_SholudReturnAOkResult()
        {
            //ARRANGE

            servicio = BuildDataContextFaker.BuildServicioRequest();

            //ACT
            var result = await _controller.UpdateServicio(servicio);

            //ASSERT
            Assert.IsType<OkObjectResult>(result);
        }

        //Unit Test Failed Update by non Existent name
        [Fact]
        public async Task Controller_UpdateServicio_SholudReturnAConflict()
        {
            //ARRANGE

            servicio = BuildDataContextFaker.BuildServicioRequestWithAnEspecificName("NoExist", "NoExist");

            _mediator.Setup(m => m.Send(It.IsAny<UpdateServiciosCommand>(), default)).ThrowsAsync(new CustomException(new() { "" }));

            //ACT
            var result = await _controller.UpdateServicio(servicio);

            //ASSERT
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status409Conflict, statusCodeResult.StatusCode);
        }

        //Unit Test Success Add
        [Fact]
        public async Task Controller_AddServicio_SholudReturnACreated()
        {
            //ARRANGE

            servicio = BuildDataContextFaker.BuildServicioRequestWithAnEspecificName("username9", "servicio50");

            //ACT
            var result = await _controller.AgregarServicio(servicio);

            //ASSERT
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status201Created, statusCodeResult.StatusCode);
        }

        //Unit Test Failed Agregar
        [Fact]
        public async Task Controller_AgregarServicio_SholudReturnABadRequest()
        {
            //ARRANGE

            servicio = BuildDataContextFaker.BuildServicioRequestWithAnEspecificName("username9", "servicio5");

            _mediator.Setup(m => m.Send(It.IsAny<AgregarServicioCommand>(), default)).ThrowsAsync(new CustomException(new() { "" }));

            //ACT
            var result = await _controller.AgregarServicio(servicio);

            //ASSERT
            Assert.IsType<BadRequestObjectResult>(result);
        }

        //Unit Test Failed Update Status
        [Fact]
        public async Task Controller_UpdateStatusServicio_SholudReturnAConflict()
        {
            //ARRANGE

            var status = BuildDataContextFaker.BuildStatusServicioRequest();

            _mediator.Setup(m => m.Send(It.IsAny<StatusServiciosCommand>(), default)).ThrowsAsync(new CustomException(new() { "" }));

            //ACT
            var result = await _controller.UpdateStatusServicio(status,"servicio1");

            //ASSERT
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status409Conflict, statusCodeResult.StatusCode);
        }

        //Unit Test Succes Update Status
        [Fact]
        public async Task Controller_UpdateStatusServicio_SholudReturnAOkRequest()
        {
            //ARRANGE

            var status = BuildDataContextFaker.BuildStatusServicioRequest();

            //ACT
            var result = await _controller.UpdateStatusServicio(status, "servicio1");

            //ASSERT
            Assert.IsType<OkObjectResult>(result);
        }
    }
}