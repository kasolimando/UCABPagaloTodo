using System.Diagnostics.CodeAnalysis;

namespace UCABPagaloTodoMS.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public class FormatoConEntity
    {
        public Guid Id { get; set; }

        public string NombreCampo { get; set; } = string.Empty;

        public string TipoDato { get; set; } = string.Empty;
        
    }
}
