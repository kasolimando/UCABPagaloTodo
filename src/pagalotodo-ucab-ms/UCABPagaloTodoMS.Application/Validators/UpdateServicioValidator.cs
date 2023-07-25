using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Requests;
using static UCABPagaloTodoMS.Core.Entities.ServicioEntity;

namespace UCABPagaloTodoMS.Application.Validators
{
    [ExcludeFromCodeCoverage]
    public class UpdateServicioValidator : AbstractValidator<UpdateServiciosCommand>
    {
        public UpdateServicioValidator()
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
            List<string> Tipos = new() { "confirmacion", "contado" };

            if (Tipos.Contains(request.TipoPago.ToLower()))
                return true;

            return false;
        }
    }
}
