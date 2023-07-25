using FluentValidation.Results;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Validators;
using UCABPagaloTodoMS.Core.Database;
using static Microsoft.Azure.Amqp.Serialization.SerializableType;

namespace UCABPagaloTodoMS.Application.Exceptions
{
    //Descripcion:
    //Customización de excepciones propias del validator.
    public class ValidatorException : Exception
    {
        private List<string> ErrorMessage { get; set; }  = new List<string>();

        private List<string> ErrorCode { get; set; } = new List<string>();

        //Descripcion:
        //Constructores para cada tipo de excepción customizada.

        public ValidatorException(ValidationResult result)
        {

            foreach (var error in result.Errors)
            {
                ErrorMessage.Add(error.ErrorMessage);
                ErrorCode.Add(error.ErrorCode);
            }
        }

        public List<string> GetMessages()
        {
            return this.ErrorMessage;
        }

        public List<string> GetCodes()
        {
            return this.ErrorCode;
        }

    }

}
