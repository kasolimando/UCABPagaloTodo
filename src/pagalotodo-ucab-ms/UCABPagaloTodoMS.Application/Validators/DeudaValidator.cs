using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Requests;

namespace UCABPagaloTodoMS.Application.Validators
{
    [ExcludeFromCodeCoverage]
    public class DeudaValidator : AbstractValidator<AgregarDeudaCommand>
    {
        public DeudaValidator()
        {

            RuleFor(s => s.Archivo.FileName)
                .NotNull().WithMessage("Debe ingresar un servicio")
                .NotEmpty().WithMessage("Debe ingresar un servicio");
        }
    }
}
