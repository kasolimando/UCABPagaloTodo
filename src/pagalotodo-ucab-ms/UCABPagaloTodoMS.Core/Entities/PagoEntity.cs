using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace UCABPagaloTodoMS.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public class PagoEntity
    {
        public Guid Id { get; set; }

        public double Monto { get; set; }

        public DateTime Fecha { get; set; }

        public bool Aprobado { get; set; }

        public bool Cierre { get; set; }

        public Guid ServicioEntityId { get; set; }

        public string ConsumidorEntityId { get; set; } = string.Empty;

        public ServicioEntity Servicio { get; set; } 

        public ConsumidorEntity Consumidor { get; set; }

        public DateTime FechaCierre { get; set; }

    }
}
