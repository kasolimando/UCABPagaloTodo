
using FluentValidation;
using UCABPagaloTodoMS.Application.Commands;

namespace UCABPagaloTodoMS.Application.Validators
{
    public class AgregarPagoValidator : AbstractValidator<AgregarPagoCommand>
    {
        public AgregarPagoValidator()
        {

            RuleFor(s => s.Request.Consumidor)
                .NotNull().WithMessage("Debe ingresar un consumidor")
                .NotEmpty().WithMessage("Debe ingresar un consumidor");

            RuleFor(s => s.Request.Servicio)
                .NotNull().WithMessage("Debe ingresar un servicio")
                .NotEmpty().WithMessage("Debe ingresar un servicio");
        }
    }
}
