using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Core.Database;

namespace UCABPagaloTodoMS.Application.BusinessValidation
{
    public class UpdateUserValidation : BaseValidation
    {

        private readonly IUCABPagaloTodoDbContext _dbContext;

        public UpdateUserValidation(IUCABPagaloTodoDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        /// <summary>
        ///     Validates the Business Rules To Update a Admin
        /// </summary>
        /// <remarks>
        /// <paramref name="Request"/> AdminsRequest with the informaci[on of the Update Request
        /// <paramref name="_dbContext"/> Context
        /// </remarks>
        /// <returns>Returns the username of the user updated</returns>
        /// 
        public async Task<string> ValidateUpdateUser(AdminsRequest request, string username)
        {
            using var transaction = _dbContext.BeginTransaction();
            var Admin = await AdminValidation.GetUserById(username, _dbContext);
            if (Admin.Correo == request.Correo)
            {
                var entity = AdminsMapper.MapRequestEntity(request, username);
                Admin = AdminsMapper.MapRequestUpdateEntity(Admin, entity);
                _dbContext.Administrador.Update(Admin);
                await _dbContext.SaveEfContextChanges(Admin.Username);
                transaction.Commit();
                return Admin.Username;
            }
            else
            {
                transaction?.Rollback();
                return ValidateCorreo(_dbContext, request.Correo);
            }
        }

        /// <summary>
        ///     Validates the Business Rules To Update a Consumidor
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - Request: ConsumidorRequest with the information of the Update Request
        ///         - _dbContext: Context
        /// </remarks>
        /// <returns>Returns the username of the user updated</returns>
        /// 

        public static async Task<string> ValidateUpdateConsumidor(string username, ConsumidorRequest Request, IUCABPagaloTodoDbContext _dbContext)
        {
            using var transaction = _dbContext.BeginTransaction();
            var Consumidor = await ConsumidorValidation.GetUserById(username, _dbContext);
            if (Consumidor.Correo == Request.Correo)
            {
                var entity = ConsumidoresMapper.MapRequestEntity(Request);
                Consumidor = ConsumidoresMapper.MapRequestUpdateEntity(Consumidor, entity);
                _dbContext.Consumidor.Update(Consumidor);
                await _dbContext.SaveEfContextChanges(username);
                transaction.Commit();
                return Consumidor.Username;
            }
            else
            {
                transaction?.Rollback();
                return ValidateCorreo(_dbContext, Request.Correo);
            }
        }

        /// <summary>
        ///     Validates the Business Rules To Update a Consumidor
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - _username: string that represents the user to be Update
        ///         - _result: ValidationResult that contains the result of the Validator
        ///         - Request: ConsumidorRequest with the information of the Update Request
        ///         - _dbContext: Context
        ///         - ExceptionValidator: ValidatorException the custom validation of the validators
        /// </remarks>
        /// <returns>Returns the username of the user updated</returns>
        /// 

        public async Task<string> ValidateUpdatePrestador(PrestadorRequest request, string username)
        {
            using var transaction = _dbContext.BeginTransaction();
            var infoPrestadorActual = await PrestadorValidation.GetUserById(username, _dbContext);
            var requestAsPrestador = PrestadoresMapper.MapRequestEntity(request);
            infoPrestadorActual = PrestadoresMapper.MapRequestUpdateEntity(infoPrestadorActual, requestAsPrestador);
            if (request.Estatus)
            {
                await PrestadorValidation.EnableUser(infoPrestadorActual, _dbContext);
            }
            else
            {
                await PrestadorValidation.DisableUserIfPossible(infoPrestadorActual, _dbContext);
            }
            transaction.Commit();
            return infoPrestadorActual.Username;
        }
    }
}
