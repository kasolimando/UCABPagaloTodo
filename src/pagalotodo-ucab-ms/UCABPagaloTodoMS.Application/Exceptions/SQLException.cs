
namespace UCABPagaloTodoMS.Application.Exceptions
{
    //Descripcion:
    //Customización de excepciones propias del manejo de la base de datos.
    public class SQLException : Exception
    {
        private List<string>? ErrorMessage { get; set; } //Guarda la lista de todos los mensajes de error posibles.

        private string ErrorCode { get; set; } = string.Empty; //Guarda un código de error.

        //Descripcion:
        //Constructores para cada tipo de excepción customizada.

        public SQLException(List<string> _ErrorMessage)
        {
            ErrorMessage = _ErrorMessage;
        }

        public List<string> GetErrorMessage()
        {
            return this.ErrorMessage;
        }
    }
}
