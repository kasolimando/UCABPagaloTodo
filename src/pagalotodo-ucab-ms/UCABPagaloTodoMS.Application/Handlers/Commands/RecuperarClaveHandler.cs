using UCABPagaloTodoMS.Core.Database;
using MediatR;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.BusinessValidation;
using UCABPagaloTodoMS.Application.Commands;
using XAct;

namespace UCABPagaloTodoMS.Application.Handlers.Commands
{
    public class RecuperarClaveHandler : IRequestHandler<RecuperarClaveCommand, RecuperarClaveResponse>
    {
        private readonly IUCABPagaloTodoDbContext _dbContext;
        private readonly ILogger<RecuperarClaveCommand> _logger;


        public RecuperarClaveHandler(IUCABPagaloTodoDbContext dbContext, ILogger<RecuperarClaveCommand> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public Task<RecuperarClaveResponse> Handle(RecuperarClaveCommand request, CancellationToken cancellationToken)
        {
            try
            {

                if (request is null || request.username.IsNullOrEmpty())
                {
                    throw new CustomException(new() { "Disculpe, hubo un error, por favor intente mas tarde" }); ;
                }
                else
                {
                    return HandleAsync(request);
                }

            }
            catch (Exception ex) { throw new CustomException(new() { "Disculpe, hubo un error, por favor intente mas tarde ", ex.Message }); }
        }

        private async Task<RecuperarClaveResponse> HandleAsync(RecuperarClaveCommand request)
        {
            try
            {
                _logger.LogInformation("RecuperarClaveQueryHandler.HandleAsync");
                var recuperarClaveValidation = new RecuperarClaveValidation(_dbContext);
                var user = await recuperarClaveValidation.GetUser(request.username);
                return new RecuperarClaveResponse()
                {
                    Correo = user.Correo
                };
            }
            catch (SQLException ex)
            {
                throw new CustomException(ex.GetErrorMessage());
            }
            catch (Exception ex)
            {
                throw new CustomException(new() { "Disculpe, hubo un error, por favor intente mas tarde ", ex.Message });
            }
        }
    }
}
