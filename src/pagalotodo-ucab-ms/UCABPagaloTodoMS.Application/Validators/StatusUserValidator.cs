using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Requests;

namespace UCABPagaloTodoMS.Application.Validators
{
    [ExcludeFromCodeCoverage]
    public class StatusUserValidator : AbstractValidator<StatusUserCommand>
    {
        public StatusUserValidator()
        {
            RuleFor(a => a.Username)
                .NotNull().WithMessage("Debe ingresar un nombre de usuario")
                .NotEmpty().WithMessage("Debe ingresar un nombre de usuario")
                .MaximumLength(10).WithMessage("El nombre de usuario debe ser de maximo 10 caracteres");

            RuleFor(a => a.Request.Estatus)
                .NotNull().WithMessage("Debe ingresar un estatus para el usuario");
        }
    }
}
