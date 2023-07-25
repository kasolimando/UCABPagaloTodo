using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using UCABPagaloTodoMS.Application.Queries;

namespace UCABPagaloTodoMS.Application.Validators
{
    [ExcludeFromCodeCoverage]
    public class LoginValidator : AbstractValidator<LoginUsuariosQuery>
    {
        public LoginValidator()
        {
            RuleFor(f => f.username)
                .NotNull().WithMessage("Debe ingresar el nombre de usuario");

            RuleFor(f => f.clave)
                .NotNull().WithMessage("Debe ingresar la clave");

        }
    }
}
