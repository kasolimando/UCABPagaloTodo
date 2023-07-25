using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Core.Database;
using MediatR;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Validators;
using FluentValidation.Results;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.BusinessValidation;

namespace UCABPagaloTodoMS.Application.Handlers.Commands
{
     public class UpdateAdminsHandler : IRequestHandler<UpdateAdminsCommand, string>
     {
         private readonly IUCABPagaloTodoDbContext _dbContext;
         private readonly ILogger<UpdateAdminsHandler> _logger;

         public UpdateAdminsHandler(IUCABPagaloTodoDbContext dbContext,ILogger<UpdateAdminsHandler> logger)
         {
             _dbContext = dbContext;
             _logger = logger;
         }

        public Task<string> Handle(UpdateAdminsCommand request, CancellationToken cancellationToken)
         {
             try
             {
                if (request.Request is null)
                {
                    throw new CustomException(new() { "Disculpe hubo un error" });
                }
                else
                {
                    return HandleAsync(request);
                }
             }
             catch (Exception ex)
             {
                throw new CustomException(new() { "Disculpe hubo un error " , ex.Message });
            }
         }

         private async Task<string> HandleAsync(UpdateAdminsCommand request)
         {
             try
             {
                _logger.LogInformation("UpdateAdminsHandler.HandleAsync");
                var validator = new UpdateAdminValidator();
                ValidationResult result = validator.Validate(request);
                if (result.IsValid)
                {
                    var updateUserValidation = new UpdateUserValidation(_dbContext);
                    return await updateUserValidation.ValidateUpdateUser(request.Request, request.Username);
                }
                else
                {
                    throw new ValidatorException(result);
                }
                
            }
             catch (ValidatorException ex)
             {
                throw new CustomException(ex.GetMessages(), ex.GetCodes());
             }
            catch (SQLException ex)
            {
                throw new CustomException(ex.GetErrorMessage());
            }
            catch (CustomException ex)
             {
                throw new CustomException(ex.GetErrorMessage(), ex.GetErrorCode());
             }
             catch (Exception ex) { throw new CustomException(new() { "Disculpe, hubo un error, por favor intente mas tarde ", ex.Message }); }
        }
     }
}
