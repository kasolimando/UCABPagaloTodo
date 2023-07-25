using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Requests;

namespace UCABPagaloTodoMS.Application.Validators
{
    [ExcludeFromCodeCoverage]
    public class CambiarClaveValidator : AbstractValidator<CambioClaveUserCommand>
    {
        public CambiarClaveValidator()
        {
            RuleFor(a => a.Request.Clave_nueva)
                .NotNull().WithMessage("Debe ingresar la clave actual")
                .NotEmpty().WithMessage("Debe ingresar la clave actual");

            RuleFor(a => a.Username)
                .NotNull().WithMessage("Debe ingresar el usuario")
                .NotEmpty().WithMessage("Debe ingresar el usuario");

            RuleFor(a => a.Request.Clave_actual)
                .NotNull().WithMessage("Debe ingresar la clave nueva")
                .NotEmpty().WithMessage("Debe ingresar la clave nueva");
        }
    }
}
