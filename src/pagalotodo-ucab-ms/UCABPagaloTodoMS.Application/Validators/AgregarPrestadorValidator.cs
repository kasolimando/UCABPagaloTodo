using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Requests;

namespace UCABPagaloTodoMS.Application.Validators
{
    [ExcludeFromCodeCoverage]
    public class AgregarPrestadorValidator : AbstractValidator<AgregarPrestadorCommand>
    {
        public AgregarPrestadorValidator()
        {
            RuleFor(a => a.Request.Username)
                .NotNull().WithMessage("Debe ingresar un nombre de usuario")
                .NotEmpty().WithMessage("Debe ingresar un nombre de usuario")
                .MaximumLength(10).WithMessage("El nombre de usuario debe ser de maximo 10 caracteres");

            RuleFor(a => a.Request.Clave)
                .NotNull().WithMessage("Debe ingresar una clave")
                .NotEmpty().WithMessage("Debe ingresar una clave");

            RuleFor(a => a.Request.Correo)
                .NotNull().WithMessage("Debe ingresar un email")
                .NotEmpty().WithMessage("Debe ingresar un email")
                .EmailAddress().WithMessage("Debe ser un email ejemplo@gmail.com");

            RuleFor(a => a.Request.DocIdentidad)
                .NotNull().WithMessage("Debe ingresar su documento de identificacion")
                .NotEmpty().WithMessage("Debe ingresar su documento de identificacion");

            RuleFor(a => a.Request.TipoVj)
                .NotNull().WithMessage("Debe ingresar su tipo de documento")
                .NotEmpty().WithMessage("Debe ingresar su tipo de documento");

            RuleFor(c => c.Request)
              .Must(ValidateTipo).WithMessage("Debe ingresar un tipo de documento valido");

            RuleFor(s => s.Request)
               .Must(ValidateUsername).WithMessage("El nombre de usuario no debe tener espacios en blanco");
        }


        // <summary>
        ///     Validates TipoVj
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - request: PrestadorRequest, it has to be in the list Tipos to be accepted
        /// </remarks>
        /// <returns>Returns a bool with the result of the search</returns>
        /// 

        private static bool ValidateTipo(PrestadorRequest request)
        {
            List<string> Tipos = new() { "v", "j", "g", "p", "e" };

            if (Tipos.Contains(request.TipoVj.ToLower()))
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
        private static bool ValidateUsername(PrestadorRequest request)
        {

            if (request.Username.Contains(' '))
                return false;

            return true;
        }

    }
}
