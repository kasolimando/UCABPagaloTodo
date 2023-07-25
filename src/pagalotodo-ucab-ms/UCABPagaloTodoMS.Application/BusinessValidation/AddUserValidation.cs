

using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Core.Database;

namespace UCABPagaloTodoMS.Application.BusinessValidation
{
    public class AddUserValidation : BaseValidation
    {
        /// <summary>
        ///     Validates the Business Rules To create a Consumidor
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - _result: ValidationResult that contains the result of the Validator
        ///         - Request: ConsumidorRequest with the informatioon of the new Consumidor
        ///         - _dbContext: Context
        ///         - ExceptionValidator: ValidatorException the custom validation of the validators
        /// </remarks>
        /// <returns>Returns the username of the user added</returns>
        /// 

        public static async Task<string> ValidateAddConsumidor(ConsumidorRequest Request, IUCABPagaloTodoDbContext _dbContext)
        {
            using var transaction = _dbContext.BeginTransaction();
            ValidateUsername(_dbContext, Request.Username);
            ValidateCorreo(_dbContext, Request.Correo);
            var entity = ConsumidoresMapper.MapRequestEntity(Request);
            _dbContext.Consumidor.Add(entity);
            await _dbContext.SaveEfContextChanges(entity.Username);
            transaction.Commit();
            return entity.Username;
        }

        /// <summary>
        ///     Validates the Business Rules To create a Prestador
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - _result: ValidationResult that contains the result of the Validator
        ///         - Request: PrestadorRequest with the information of the new Prestador
        ///         - _dbContext: Context
        ///         - ExceptionValidator: ValidatorException the custom validation of the validators
        /// </remarks>
        /// <returns>Returns the username of the user added</returns>
        /// 
        public static async Task<string> ValidateAddUser(PrestadorRequest Request, IUCABPagaloTodoDbContext _dbContext)
        {
            using var transaction = _dbContext.BeginTransaction();
            ValidateUsername(_dbContext, Request.Username);
            ValidateCorreo(_dbContext, Request.Correo);
            var entity = PrestadoresMapper.MapRequestEntity(Request);
            _dbContext.Prestador.Add(entity);
            await _dbContext.SaveEfContextChanges(entity.Username);
            transaction.Commit();
            return entity.Username;
        }
    }
}
