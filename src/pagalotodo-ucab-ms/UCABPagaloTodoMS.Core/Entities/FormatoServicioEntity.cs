using System.Diagnostics.CodeAnalysis;

namespace UCABPagaloTodoMS.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public class FormatoServicioEntity
    {
        public Guid Id { get; set; }

        public Guid ServicioEntityId { get; set; }

        public Guid FormatoConEntityId { get; set; }

        public ServicioEntity Servicio { get; set; }

        public FormatoConEntity FormatoCon { get; set; }

        public bool Requerido { get; set; }

        public int Logitud { get; set; }
    }
}
