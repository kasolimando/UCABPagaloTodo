using System.Globalization;

namespace UCABPagaloTodoWeb.Utilities
{
    public static class FixDate
    {
        public static string FixFormatData(string fechaString)
        {
            DateTime fechaDateTime = DateTime.ParseExact(fechaString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            string fechaFormateada = fechaDateTime.ToString("dd/MM/yyyy");
            return fechaFormateada;
        }
    }
}
