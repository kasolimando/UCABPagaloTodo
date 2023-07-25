using UCABPagaloTodoMS.Core.Database;
using MediatR;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Application.Responses;
using Microsoft.EntityFrameworkCore;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Mappers;

namespace UCABPagaloTodoMS.Application.Handlers.Queries
{
     public class ConsultarAdminsQueryHandler : IRequestHandler<ConsultarAdminsQuery, AdminsResponse>
     {
         private readonly IUCABPagaloTodoDbContext _dbContext;
         private readonly ILogger<ConsultarAdminsQuery> _logger;
         

         public ConsultarAdminsQueryHandler(IUCABPagaloTodoDbContext dbContext, ILogger<ConsultarAdminsQuery> logger)
         {
             _dbContext = dbContext;
             _logger = logger;
         }

         public Task<AdminsResponse> Handle(ConsultarAdminsQuery request, CancellationToken cancellationToken)
         {
             try
             {
                if (request is null || request.username is null)
                 {
                    throw new CustomException(new() { "Debe ingresar un username" });
                }
                 else
                 {
                    return HandleAsync(request.username);
                 }
             }
             catch (CustomException)
             {
                throw new CustomException(new() { "Disculpe hubo un error intente mas tarde" });
             }
         }

         private async Task<AdminsResponse> HandleAsync(string _username)
         {
             try
             {
                 _logger.LogInformation("ConsultarAdminsQueryHandler.HandleAsync");

                var result = _dbContext.Administrador.Where(c => c.Username == _username).Select(c => AdminsMapper.MapEntityAResponse(c));
                if (!result.Any()) 
                    throw new CustomException(new() { "Disculpe el usuario no existe" }, new() { "1475" });
                return await result.FirstOrDefaultAsync();
             }
            catch (CustomException ex)
            {
                throw new CustomException(ex.GetErrorMessage(), ex.GetErrorCode());
            }
            catch (Exception ex) { throw new CustomException(new() { "Disculpe hubo un error intente mas tarde", ex.Message }); }
        }
     }
}
