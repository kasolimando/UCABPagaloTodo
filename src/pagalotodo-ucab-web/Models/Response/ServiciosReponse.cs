namespace UCABPagaloTodoWeb.Models.Response
{
    public class ServiciosResponse
    {
        public string Nombre { get; set; } = string.Empty;

        public string PrestadorEntityId { get; set; } = string.Empty;

        public string? Descripcion { get; set; }  // Si la queremos que no sea null deberiamos quitarle el ?

        public string? Categoria { get; set; } = string.Empty;

        public string Estatus { get; set; } = string.Empty;

        public string TipoPago { get; set; } = string.Empty;
    }
}
