using UCABPagaloTodoMS.Core.Database;
using MediatR;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Application.Responses;
using Microsoft.EntityFrameworkCore;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Application.BusinessValidation;
using System.Globalization;
using System.Web;
using FluentValidation.Results;
using UCABPagaloTodoMS.Application.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;

namespace UCABPagaloTodoMS.Application.Handlers.Queries
{
    /*
       <summary>
         Handler the ConsultarPagoQuery
       </summary>
       <remarks>
            Description
               GET Pagos
       </remarks>
       <typeparam name="TRequest">ConsultarPagoQuery</typeparam>
       <typeparam name="TResponse">List<PagosResponse></typeparam>
       <response>
           Response for a request
           Accepted:
               - Task<string> = List<PagosResponse>
           Failed:
               - Error en formato de campos = ValidatorException
               - Error de data = SQLException
               - Resto de errores = Exception
               Retorna los tres errores en el tipo CustomException
       </response>
       <returns>Returns a list with the Pagos</returns>
   */
    public class ConsultarPagosQueryHandler : IRequestHandler<ConsultarPagoQuery, List<PagoResponse>>
     {
         private readonly IUCABPagaloTodoDbContext _dbContext;
         private readonly ILogger<ConsultarPagoQuery> _logger;

         public ConsultarPagosQueryHandler(IUCABPagaloTodoDbContext dbContext, ILogger<ConsultarPagoQuery> logger)
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
              - request = TRequest to consult a Pago (ConsultarPagoQuery)
               - CancellationToken 
       </remarks>
       <response>
           Accepted:
               - Task<string> = List<PagosResponse>
           Failed:
               - CustomException
       </response>
       <returns>Returns a list with the Pagos</returns>    
       */
        public Task<List<PagoResponse>> Handle(ConsultarPagoQuery request, CancellationToken cancellationToken)
         {
             try
             {
                
                 if (request is null)
                 {
                    throw new CustomException(new() { "Disculpe hubo un error intente mas tarde" });
                }
                 else
                 {
                    
                     return HandleAsync(request);
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
                - _servicio = string to consult a Pago (ConsultarPagoQuery)
                - _tipo = string to know what kind of consult is (ConsultarPagoQuery)
                - _fecha = string to consult a Pago by fecha (ConsultarPagoQuery)
      </remarks>
      <response>
          Accepted:
              - async Task<string> = List<PagosResponse>
           Failed:
              - CustomException
      </response>
      <returns>Returns a list with the Pagos</returns>    
      */
        private async Task<List<PagoResponse>> HandleAsync(ConsultarPagoQuery request)
        {

            try
            {
                _logger.LogInformation("ConsultarPagosHandler");
                //Se validan con el Validator el formato de los campos
                var validator = new ConsultarPagosValidator();
                ValidationResult result = validator.Validate(request);
                if (result.IsValid)
                {
                    DateTime fechaInicio = DateTime.ParseExact(request.fechaInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime fechaFin = DateTime.ParseExact(request.fechaFin, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    if (request.servicio != "" && request.consumidor != "")
                    {
                        throw new CustomException(new() { "Debe elegir el tipo de consulta a realizar, los campos 'consumidor' y 'servicio' no pueden estar llenos simultaneamente, escoja uno, o en su defecto, ninguno." });
                    }
                    else
                    {
                        if (request.servicio == "" && request.consumidor == "")
                        {
                            var pagos = await _dbContext.Pago.ToListAsync();
                            var servicios = await _dbContext.Servicio.ToListAsync();


                            // Filtrar los pagos por la fecha recibida
                            var pagosFiltrados = pagos.Where(p => p.Fecha.Date >= fechaInicio && p.Fecha.Date <= fechaFin).ToList();

                            if (!pagosFiltrados.Any())
                                throw new CustomException(new() { "Disculpe, no se encuentran pagos para la fecha indicada" }, new() { "777" });

                            // Realizar el mapeo de los pagos filtrados a PagosResponse y reemplazar el ID del servicio por el nombre
                            var pagosList = pagosFiltrados.Select(pago =>
                            {
                                var servicio = servicios.FirstOrDefault(s => s.Id == pago.ServicioEntityId);
                                var nombreServicio = servicio?.Nombre;
                                return PagosMapper.MapEntityAResponse(pago, nombreServicio);
                            }).ToList();

                            // Devolver la lista de pagos filtrados

                            return pagosList;
                        }

                        if (request.servicio == "" && request.consumidor != "")
                        {
                            var pagos = await _dbContext.Pago.Where(c => c.ConsumidorEntityId == request.consumidor).ToListAsync();


                            // Consulta los pagos para el servicio y fecha proporcionados y los mapea a respuestas de pagos

                            var pagosFiltrados = pagos.Where(p => p.Fecha.Date >= fechaInicio && p.Fecha.Date <= fechaFin).Select(p => PagosMapper.MapEntityAResponse(p, _dbContext.Servicio.FirstOrDefault(s => s.Id == p.ServicioEntityId).Nombre)).ToList();

                            if (!pagosFiltrados.Any())
                                throw new CustomException(new() { "Disculpe, no se encuentran pagos para la fecha indicada" }, new() { "777" });


                            return pagosFiltrados;
                        }

                        if (request.servicio != "" && request.consumidor == "")
                        {
                            // Obtiene el ID del servicio a través de su nombre
                            var servicio = await ServicioValidation.GetServicio(request.servicio, _dbContext);
                            

                            var pagos = await _dbContext.Pago.Where(c => c.ServicioEntityId == servicio.Id).ToListAsync();


                            // Consulta los pagos para el servicio y fecha proporcionados y los mapea a respuestas de pagos

                            var pagosFiltrados = pagos.Where(p => p.Fecha.Date >= fechaInicio && p.Fecha.Date <= fechaFin).Select(p => PagosMapper.MapEntityAResponse(p, request.servicio)).ToList();

                            if (!pagosFiltrados.Any())
                                throw new CustomException(new() { "Disculpe, no se encuentran pagos para la fecha indicada" }, new() { "777" });


                            return pagosFiltrados;
                        }
                        return null;

                    }
                }
                else
                {
                    throw new ValidatorException(result);
                }
            }
            catch (ValidatorException ex)
            {
                throw new CustomException(ex.GetMessages(), ex.GetCodes());
            }
            catch (CustomException ex)
            {
                throw new CustomException(ex.GetErrorMessage(), ex.GetErrorCode());
            }
            catch (Exception ex){ throw new CustomException(new() { "Disculpe, hubo un error, por favor intente mas tarde ", ex.Message });}
        }
    }
}
