using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Requests;
using static UCABPagaloTodoMS.Core.Entities.ServicioEntity;

namespace UCABPagaloTodoMS.Application.Validators
{
    [ExcludeFromCodeCoverage]
    public class ServicioValidator : AbstractValidator<AgregarServicioCommand>
    {
        public ServicioValidator()
        {
            RuleFor(s => s.Request.Nombre)
                .NotNull().WithMessage("Debe ingresar un nombre de servicio")
                .NotEmpty().WithMessage("Debe ingresar un nombre de servicio");

            RuleFor(s => s.Request.TipoPago)
                .NotNull().WithMessage("Debe ingresar un tipo de pago")
                .NotEmpty().WithMessage("Debe ingresar un tipo de pago");

            RuleFor(s => s.Request)
                .Must(ValidateTipoPago).WithMessage("Debe ingresar un tipo de pago valido (confirmacion o contado)");

            RuleFor(s => s.Request.PrestadorEntityId)
                .NotNull().WithMessage("Debe ingresar un prestador")
                .NotEmpty().WithMessage("Debe ingresar un prestador");

            RuleFor(c => c.Request)
              .Must(ValidateStatus).WithMessage("Debe ingresar un tipo de estatus valido");

            RuleFor(s => s.Request)
               .Must(ValidateNombre).WithMessage("El nombre de usuario no debe tener espacios en blanco");
        }

        /// <summary>
        ///     Validates Status
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - request: ServicioRequest, it has to be in the list Tipos to be accepted
        /// </remarks>
        /// <returns>Returns a bool with the result of the search</returns>
        /// 


        private static bool ValidateStatus(ServicioRequest request)
        {
            List<string> Tipos = new() { "proximamente", "activo","inactivo" };

            if (Tipos.Contains(request.Estatus.ToLower()))
                return true;

            return false;
        }

        /// <summary>
        ///     Validates TipoPago
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - request: ServicioRequest, it has to be in the list Tipos to be accepted
        /// </remarks>
        /// <returns>Returns a bool with the result of the search</returns>
        /// 
        private static bool ValidateTipoPago(ServicioRequest request)
        {
            List<string> Tipos = new() { "confirmacion", "contado"};

            if (Tipos.Contains(request.TipoPago.ToLower()))
                return true;

            return false;
        }

        /// <summary>
        ///     Validates Nombre
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - request: ServicioRequest, it has to be in the list Tipos to be accepted
        /// </remarks>
        /// <returns>Returns a bool with the result of the search</returns>
        /// 
        private static bool ValidateNombre(ServicioRequest request)
        {

            if (request.Nombre.Contains(" "))
                return false;

            return true;
        }
    }
}
