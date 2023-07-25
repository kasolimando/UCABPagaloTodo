using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Core.Database;
using Microsoft.EntityFrameworkCore;
using UCABPagaloTodoMS.Core.Entities;

namespace UCABPagaloTodoMS.Application.BusinessValidation
{
    public class FormatoValidation
    {

        /// <summary>
        ///     Validates the Business Rules To create a Formato
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - _result: ValidationResult that contains the result of the Validator
        ///         - Request: FormatosRequest with the informatioon of the new Formato
        ///         - _dbContext: Context
        ///         - ExceptionValidator: ValidatorException the custom validation of the validators
        /// </remarks>
        /// <returns>Returns the servicio's name of the formato added</returns>
        /// 
        public static async Task<string> ValidateFormatoAdd(FormatosRequest Request, IUCABPagaloTodoDbContext _dbContext)
        {
            //Open the transaction
            using var transaction = _dbContext.BeginTransaction();
            //The Request.Servicio is the name, so this get the Servicio's Guid
            var Servicio = await ServicioValidation.GetServicio(Request.Servicio, _dbContext);
            //If the Serivico already has a Formato
            if (await _dbContext.FormatoServicio.Where(f => f.ServicioEntityId == Servicio.Id).FirstOrDefaultAsync() is null)
            {
                for (int i = 0; i < Request.Campos.Count; i++)
                {
                    var Campo = await _dbContext.Formato.Where(c => c.NombreCampo == Request.Campos[i]).FirstOrDefaultAsync();
                    //Change the FormatoConEntity to a FormatosResponse
                    var entity = FormatoMapper.MapRequestEntity(Request.Longitud[i], Campo.Id, Servicio.Id, Request.Requerido);
                    _dbContext.FormatoServicio.Add(entity);
                    await _dbContext.SaveEfContextChanges(Request.Servicio);
                }
                transaction.Commit();
                return Request.Servicio;
            }
            else
            {
                transaction?.Rollback();
                throw new CustomException(new() { "Disculpe el servicio ya tiene un formato registrado" });
            }
        }


        /// <summary>
        ///     Validates the Business Rules To Update a Formato
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - _result: ValidationResult that contains the result of the Validator
        ///         - Request: FormatosRequest with the information of the Update Request
        ///         - _dbContext: Context
        ///         - ExceptionValidator: ValidatorException the custom validation of the validators
        /// </remarks>
        /// <returns>Returns servicio's formato updated</returns>
        /// 
        public static async Task<string> ValidateFormatoUpdate(FormatosRequest Request, IUCABPagaloTodoDbContext _dbContext)
        {
            using var transaction = _dbContext.BeginTransaction();
            var _Servicio = await ServicioValidation.GetServicio(Request.Servicio, _dbContext);
            var actual = await _dbContext.FormatoServicio.Where(f => f.ServicioEntityId == _Servicio.Id && f.FormatoCon.NombreCampo == Request.Campos[0]).FirstOrDefaultAsync();
            //If the Serivico has a Formato
            if (actual is not null)
            {
                //Change the FormatosResponse to a FormatoConEntity
                var entity = FormatoMapper.MapRequestEntity(Request.Longitud[0], actual.FormatoConEntityId, _Servicio.Id, Request.Requerido);
                //Change the old values to the new values in a FormatoConEntity
                actual = FormatoMapper.MapRequestUpdateEntity(actual, entity);
                _dbContext.FormatoServicio.Update(actual);
                await _dbContext.SaveEfContextChanges(Request.Servicio);
                transaction.Commit();
                return Request.Servicio;
            }
            else
            {
                transaction?.Rollback();
                throw new CustomException(new() { "El servicio no tiene registrado un formato" });
            }
        }

        public static async Task<List<FormatoServicioEntity>> GetFormato(ServicioEntity servicio, IUCABPagaloTodoDbContext dbContext)
        {
            var Formato = await dbContext.FormatoServicio.Include(fs => fs.FormatoCon).Where(fs => fs.ServicioEntityId == servicio.Id && fs.Requerido == true).OrderBy(f => f.FormatoCon.NombreCampo).ToListAsync();
            //If ListaPagos > 0 the service has payments in false 
            if (Formato.Count() == 0)
                throw new CustomException(new() { "El servicio debe tener un formato antes de hacer el cierre" });
            return Formato;
        }
    }
}
