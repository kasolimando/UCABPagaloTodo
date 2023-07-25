using MediatR;
using System.ComponentModel.DataAnnotations;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;
using XAct.Users;

namespace UCABPagaloTodoMS.Application.Queries
{
     public class ConsultarPagoQuery : IRequest<List<PagoResponse>>
     {
         public string servicio = string.Empty;

         public string consumidor = string.Empty;

         public string fechaInicio = string.Empty;

         public string fechaFin = string.Empty;
        

        public ConsultarPagoQuery(string? _servicio, string? _consumidor, string _fechaInicio, string _fechaFin)
         {
             servicio = _servicio;
             consumidor = _consumidor;
             fechaInicio = _fechaInicio;
             fechaFin = _fechaFin;
             
         }

     }

   
}
