using UCABPagaloTodoMS.Core.Database;
using MediatR;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Application.Responses;
using Microsoft.EntityFrameworkCore;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Application.BusinessValidation;

namespace UCABPagaloTodoMS.Application.Handlers.Queries
{
    /*
       <summary>
         Handler the ConsultarServicioQuery
       </summary>
       <remarks>
            Description
               GET Servicios
       </remarks>
       <typeparam name="TRequest">ConsultarServicioQuery</typeparam>
       <typeparam name="TResponse">List<FormatosResponse></typeparam>
       <response>
           Response for a request
           Accepted:
               - Task<string> = List<ServiciosResponse>
           Failed:
               - Error en formato de campos = ValidatorException
               - Error de data = SQLException
               - Resto de errores = Exception
               Retorna los tres errores en el tipo CustomException
       </response>
       <returns>Returns a list with the Servicios</returns>
   */
    public class ConsultarServiciosQueryHandler : IRequestHandler<ConsultarServicioQuery, List<ServiciosResponse>>
     {
         private readonly IUCABPagaloTodoDbContext _dbContext;
         private readonly ILogger<ConsultarServicioQuery> _logger;

         public ConsultarServiciosQueryHandler(IUCABPagaloTodoDbContext dbContext, ILogger<ConsultarServicioQuery> logger)
         {
             _dbContext = dbContext;
             _logger = logger;
         }

        /*
       <summary>
           Handle the Request
       </summary>
       <remarks>
            Description
               Handles a Request and check if the request is null
            Parametros
              - request = TRequest to consult a Servicio (ConsultarServicioQuery)
               - CancellationToken 
       </remarks>
       <response>
           Accepted:
               - Task<string> = List<ServiciosResponse>
           Failed:
               - CustomException
       </response>
       <returns>Returns a list with the Servicios</returns>    
       */
        public Task<List<ServiciosResponse>> Handle(ConsultarServicioQuery request, CancellationToken cancellationToken)
         {
             try
             {
                
                 if (request is null)
                 {
                    throw new CustomException(new() { "Disculpe hubo un error intente mas tarde" });
                }
                 else
                 {
                    
                     return HandleAsync(request.servicio, request.tipo);
                 }

             }
             catch (Exception ex) { throw new CustomException(new() { "Disculpe hubo un error intente mas tarde", ex.Message }); }
         }


        /*
      <summary>
          Handles the Request asynchronously
      </summary>
      <remarks>
           Description
              Handles a request, check the information before add
           Parametros
              - _servicio = string to consult a Servicio (ConsultarServicioQuery)
      </remarks>
      <response>
          Accepted:
              - async Task<string> = List<ServiciosResponse>
           Failed:
              - CustomException
      </response>
      <returns>Returns a list with the Servicios</returns>    
      */
        private async Task<List<ServiciosResponse>> HandleAsync(string _servicio, string _tipo)
        {
            try
            {
                //If _servicio is null returns all the Servicios y el tipo es "servicio"
                if (_servicio is null && _tipo.Equals("servicio"))
                {
                    //Consult the Servicios's name to find the Servicios,
                    //MapEntityAResponse change the Servicios to ServiciosResponse
                    var result = _dbContext.Servicio.Select(c => ServiciosMapper.MapEntityAResponse(c));
                    return await result.ToListAsync();
                }
                else
                {
                    // si el tipo es "prestador"
                    if (_tipo.Equals("prestador"))
                    {
                        // si el campo esta vacio lanzara una excepcion
                        if (_servicio is null)
                        {
                            throw new CustomException(new() { "Debe ingresar un prestador de servicio para continuar" }, new() { "99" });

                        }
                        var result1 = _dbContext.Servicio.Where(c => c.PrestadorEntityId == _servicio).Select(c => ServiciosMapper.MapEntityAResponse(c));
                        // si result1 es empty lanza excepcion
                        if (!result1.Any())
                        {
                            throw new CustomException(new() { "Disculpe el prestador no tiene servicios registrados" }, new() { "1475" });

                        }
                        return await result1.ToListAsync();
                    }
                    // si el tipo es "servicio"
                    if (_tipo.Equals("servicio"))
                    {
                        //Get the Guid with the Servicio's name
                        var servicio = await ServicioValidation.GetServicio(_servicio, _dbContext);
                        //Consult the Servicios's name to find the Servicios,
                        //MapEntityAResponse change the Servicios to ServiciosResponse
                        var result = _dbContext.Servicio.Where(c => c.Id == servicio.Id).Select(c => ServiciosMapper.MapEntityAResponse(c));
                        return await result.ToListAsync();
                    }
                    // obliga a retornar algo en todas las opciones posibles
                    return null;
                }
            }
            catch (SQLException ex)
            {
                throw new CustomException(ex.GetErrorMessage());
            }
            catch (CustomException ex)
            {
                throw new CustomException(ex.GetErrorMessage(), ex.GetErrorCode());
            }
            catch (Exception ex) { throw new CustomException(new() { "Disculpe, hubo un error, por favor intente mas tarde", ex.Message }); }
        }
    }
}
