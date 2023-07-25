namespace UCABPagaloTodoWeb.Models.Response
{
    public class PagoResponse
    {
        public string Consumidor { get; set; } = string.Empty;

        public double Monto { get; set; }

        public string Fecha { get; set; }

        public bool Aprobado { get; set; }

        public bool Cierre { get; set; }

        public string Servicio { get; set; } = string.Empty;
    }
}
