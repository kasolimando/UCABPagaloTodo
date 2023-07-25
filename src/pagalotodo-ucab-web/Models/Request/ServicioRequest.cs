namespace UCABPagaloTodoWeb.Models.Request
{
    public class ServicioRequest
    {
        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }  // Si la queremos que no sea null deberiamos quitarle el ?
       
        public string Estatus { get; set; } = string.Empty;

        public string? Categoria { get; set; }

        public string TipoPago { get; set; } = string.Empty;

        public string PrestadorEntityId { get; set; } = string.Empty;

    }
}
