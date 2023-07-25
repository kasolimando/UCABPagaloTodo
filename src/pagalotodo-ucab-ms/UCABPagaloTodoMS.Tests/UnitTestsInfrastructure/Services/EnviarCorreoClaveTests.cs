using Moq;
using UCABPagaloTodoMS.Application.BusinessValidation;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Tests.MockData;
using Xunit;
using FluentValidation;
using MailKit.Net.Smtp;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Tests.DataSeed;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Core.Entities;
using UCABPagaloTodoMS.Application.Requests;
using MailKit.Security;
using MimeKit;
using UCABPagaloTodoMS.Tests.UnitTestsApplication.BusinessValidation;
using System.Net.Mail;
using UCABPagaloTodoMS.Infrastructure.Services.Hotmail;

namespace UCABPagaloTodoMS.Tests.UnitTestsInfrastructure.Services
{
    public class EnviarCorreoClaveTests
    {

        public EnviarCorreoClaveTests()
        {
        }

        //Unit Test Success
        [Fact]
        public async Task TestEnviarCorreoClave_ShouldReturnTheNewClave()
        {
            //ACT
            var nuevaclave = EnviarCorreoClave.EnviarCorreoRecuperacion("destinatario@gmail.com");

            //ASSERT
            Assert.NotEmpty(nuevaclave);
        }
    }
}