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
     public class ConsultarConsumidoresQueryHandler : IRequestHandler<ConsultarConsumidorQuery, List<ConsumidoresResponse>>
     {
         private readonly IUCABPagaloTodoDbContext _dbContext;
         private readonly ILogger<ConsultarConsumidorQuery> _logger;

         public ConsultarConsumidoresQueryHandler(IUCABPagaloTodoDbContext dbContext, ILogger<ConsultarConsumidorQuery> logger)
         {
             _dbContext = dbContext;
             _logger = logger;
         }

         public Task<List<ConsumidoresResponse>> Handle(ConsultarConsumidorQuery request, CancellationToken cancellationToken)
         {
             try
             {
                
                 if (request is null)
                 {
                    throw new CustomException(new() { "Disculpe, hubo un error, por favor intente mas tarde" });
                }
                 else
                 {
                    
                     return HandleAsync(request.username);
                 }

             }
             catch (Exception ex) { throw new CustomException(new() { "Disculpe, hubo un error, por favor intente mas tarde", ex.Message }); }
         }

         private async Task<List<ConsumidoresResponse>> HandleAsync(string _username)
         {
             try
             {
                 _logger.LogInformation("ConsultarConsumidoresQueryHandler.HandleAsync");

                 if (_username is null)
                {
                    var result = _dbContext.Consumidor.Select(c => ConsumidoresMapper.MapEntityAResponse(c));


                    return await result.ToListAsync();
                } else
                {
                    var result = _dbContext.Consumidor.Where(c => c.Username == _username).Select(c => ConsumidoresMapper.MapEntityAResponse(c));

                    if (result.Count() == 0)
                        throw new CustomException(new() { "Disculpe el usuario que busca no se encuentra registrado" }, new() { "1475" });
                    
                    return await result.ToListAsync();

                }
                

            }
            catch (CustomException ex)
            {
                throw new CustomException(ex.GetErrorMessage(), ex.GetErrorCode());
            }
            catch (Exception ex) { throw new CustomException(new() { "Disculpe, hubo un error, por favor intente mas tarde", ex.Message }); }
        }
     }
}
