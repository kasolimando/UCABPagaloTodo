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

namespace UCABPagaloTodoMS.Tests.UnitTests.Controllers
{
    public class AdministradoresControllerTests
    {
        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        private readonly Mock<IMediator> _mediator;
        private readonly AdministradoresController _controller;
        private readonly Mock<ILogger<AdministradoresController>> _logger;

        //Create a new AdminsRequest with Random Values
        private AdminsRequest admin = BuildDataContextFaker.BuildAdminRequest();


        public AdministradoresControllerTests()
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
        public async Task Controller_ConsultarAdmin_SholudReturnAOkResult()
        {
            //ARRANGE

            //ACT
            var result = await _controller.ConsultaAdmin("username1");

            //ASSERT
            Assert.IsType<OkObjectResult>(result.Result);
        }

        //Unit Test Failed by null username
        [Fact]
        public async Task Controller_ConsultarAdmin_SholudReturnANotFound()
        {
            //ARRANGE

            _mediator.Setup(m => m.Send(It.IsAny<ConsultarAdminsQuery>(), default)).ThrowsAsync(new CustomException(new() { "" }));

            //ACT
            var result = await _controller.ConsultaAdmin("l");

            //ASSERT
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        //Unit Test Success Update
        [Fact]
        public async Task Controller_UpdateAdmin_SholudReturnAOkResult()
        {
            //ARRANGE

            admin = BuildDataContextFaker.BuildAdminRequestWithEspecificEmail("username1@gmail.com");

            //ACT
            var result = await _controller.UpdateAdmin(admin,"username1");

            //ASSERT
            Assert.IsType<OkObjectResult>(result);
        }

        //Unit Test Failed Update by non Existent User
        [Fact]
        public async Task Controller_UpdateAdmin_SholudReturnAConflict()
        {
            //ARRANGE

            admin = BuildDataContextFaker.BuildAdminRequestWithEspecificEmail("NoExist");

            _mediator.Setup(m => m.Send(It.IsAny<UpdateAdminsCommand>(), default)).ThrowsAsync(new CustomException(new() { "" }));

            //ACT
            var result = await _controller.UpdateAdmin(admin, "NoExist");

            //ASSERT
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status409Conflict, statusCodeResult.StatusCode);
        }

        //Unit Test Success UpdateStatus Admin
        [Fact]
        public async Task Controller_UpdateStatusAdmin_SholudReturnAOkResult()
        {
            //ARRANGE

            var status = BuildDataContextFaker.BuildStatusUserRequest();

            //ACT
            var result = await _controller.UpdateStatusAdmin(status, "username2");

            //ASSERT
            Assert.IsType<OkObjectResult>(result);
        }

        //Unit Test Success UpdateStatus Admin
        [Fact]
        public async Task Controller_UpdateStatusAdmin_SholudReturnAConflict()
        {
            //ARRANGE

            var status = BuildDataContextFaker.BuildStatusUserRequest();

            _mediator.Setup(m => m.Send(It.IsAny<StatusUserCommand>(), default)).ThrowsAsync(new CustomException(new() { "" }));

            //ACT
            var result = await _controller.UpdateStatusAdmin(status, "NoExist");

            //ASSERT
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status409Conflict, statusCodeResult.StatusCode);
        }

        //Unit Test Success Cambio de Clave
        [Fact]
        public async Task Controller_CambioClaveAdministrador_SholudReturnAOkResult()
        {
            //ARRANGE

            var cambio = BuildDataContextFaker.BuildCambioClaveUserRequestWithSpecificInfo("username1");

            //ACT
            var result = await _controller.CambioClaveAdministrador(cambio, "username1");

            //ASSERT
            Assert.IsType<OkObjectResult>(result);
        }

        //Unit Test Failed Cambio de Clave for Exception
        [Fact]
        public async Task Controller_CambioClaveAdministrador_SholudReturnAConflict()
        {
            //ARRANGE

            var cambio = BuildDataContextFaker.BuildCambioClaveUserRequestWithSpecificInfo("username1");

            _mediator.Setup(m => m.Send(It.IsAny<CambioClaveUserCommand>(), default)).ThrowsAsync(new CustomException(new() { "" }));

            //ACT
            var result = await _controller.CambioClaveAdministrador(cambio, "username3");

            //ASSERT
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status409Conflict, statusCodeResult.StatusCode);
        }
    }
}
