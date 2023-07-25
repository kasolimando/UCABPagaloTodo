using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Requests;

namespace UCABPagaloTodoMS.Application.Validators
{
    [ExcludeFromCodeCoverage]
    public class UpdateFormatoValidator : AbstractValidator<UpdateFormatosCommand>
    {
        public UpdateFormatoValidator()
        {
            RuleFor(f => f.Request.Campos)
                .NotNull().WithMessage("Debe ingresar por lo menos un campo");

            RuleFor(f => f.Request.Longitud)
                .NotNull().WithMessage("Debe ingresar La longitud de los campos");

            RuleFor(f => f.Request.Servicio)
               .NotNull().WithMessage("Debe indicar el servicio al que pertenece el formato");
        }
    }
}
