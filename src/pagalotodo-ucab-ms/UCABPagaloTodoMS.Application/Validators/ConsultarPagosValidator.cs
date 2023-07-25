using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Application.Requests;
using static UCABPagaloTodoMS.Core.Entities.ServicioEntity;

namespace UCABPagaloTodoMS.Application.Validators
{
    [ExcludeFromCodeCoverage]
    public class ConsultarPagosValidator : AbstractValidator<ConsultarPagoQuery>
    {
        public ConsultarPagosValidator()
        {
            
            RuleFor(s => s.fechaInicio)
            .NotNull().WithMessage("Debe ingresar una fecha de inicio")
            .NotEmpty().WithMessage("Debe ingresar una fecha de inicio")
            .Must(fecha => ValidateDateFormatInicio(fecha)).WithMessage("El formato de fecha de inicio es inválido");

            RuleFor(s => s.fechaFin)
                .NotNull().WithMessage("Debe ingresar una fecha de fin")
                .NotEmpty().WithMessage("Debe ingresar una fecha de fin")
                .Must(fecha => ValidateDateFormatFin(fecha)).WithMessage("El formato de fecha de fin es inválido")
                .Must((query, fechaFin) => ValidateFinMayorInicio(query.fechaInicio, fechaFin)).WithMessage("La fecha de inicio no puede ser mayor que la fecha de fin")
                .Must((query, fechaFin) => ValidateDateRange(query.fechaInicio, fechaFin)).WithMessage("La fecha fin debe ser menor o igual a la fecha actual");
        }


        private bool ValidateDateFormatInicio(string fechaIni)
        {
            if (DateTime.TryParseExact(fechaIni, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))          
                return true;
            return false;

        }

        private bool ValidateDateFormatFin(string fechaFin)
        {
            if (DateTime.TryParseExact(fechaFin, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                return true;
            return false;

        }

        private bool ValidateDateRange(string fechaInicio, string fechaFin)
        {
            if (DateTime.TryParseExact(fechaInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var inicio) &&
                DateTime.TryParseExact(fechaFin, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var fin))
            {
                return fin.Date <= DateTime.Today;
            }

            return false;
        }

        private bool ValidateFinMayorInicio(string fechaInicio, string fechaFin)
        {
            DateTime DatefechaInicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime DatefechaFin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            if (DatefechaFin > DatefechaInicio)
                return true;
            return false;
        }
    }
}
