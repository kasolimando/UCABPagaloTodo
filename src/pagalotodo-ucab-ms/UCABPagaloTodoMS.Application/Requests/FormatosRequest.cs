using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using UCABPagaloTodoMS.Core.Entities;
using static UCABPagaloTodoMS.Core.Entities.ServicioEntity;

namespace UCABPagaloTodoMS.Application.Requests
{
    [ExcludeFromCodeCoverage]
    public class FormatosRequest
    {
        [Display(Name ="Campos")]
        public List<string> Campos { get; set; } = new List<string>();

        [Display(Name = "Longitud")]
        public List<int> Longitud { get; set; } = new List<int>();

        public string Servicio { get; set; } = string.Empty;

        public bool Requerido { get; set; }

    }
}
