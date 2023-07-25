using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Requests;

namespace UCABPagaloTodoMS.Application.Validators
{
    [ExcludeFromCodeCoverage]
    public class StatusServicioValidator : AbstractValidator<StatusServiciosCommand>
    {
        public StatusServicioValidator()
        {
            RuleFor(a => a.Servicio)
                .NotNull().WithMessage("Debe ingresar un servicio")
                .NotEmpty().WithMessage("Debe ingresar un servicio");

            RuleFor(a => a.Request.Estatus)
                .NotNull().WithMessage("Debe ingresar un estatus para el usuario");
        }

    }
}
