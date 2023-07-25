using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCABPagaloTodoMS.Application.Exceptions
{
    [Serializable]
    //Descripcion:
    //Customización de excepciones.

    public class CustomException : Exception
    {
        //Descripcion:
        //Constructores para cada tipo de excepción customizada.
        
        public List<string>? ErrorMessage { get; set; }= new List<string>(); //Guarda la lista de todos los mensajes de error posibles.

        private List<string>? ErrorCode { get; set; } = new List<string>(); //Guarda la lista de todos los códigos de error posibles.

        public CustomException(List<string> _ErrorMessage, List<string> _ErrorCode)
        {
            ErrorMessage = _ErrorMessage;
            ErrorCode = _ErrorCode;
        }
        public CustomException(List<string> _ErrorMessage)
        {
            ErrorMessage = _ErrorMessage;
        }

        public List<string> GetErrorMessage()
        {
            return this.ErrorMessage;
        }

        public List<string> GetErrorCode()
        {
            return this.ErrorCode;
        }
    }
}
