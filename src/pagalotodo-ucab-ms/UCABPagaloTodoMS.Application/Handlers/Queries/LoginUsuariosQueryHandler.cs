using UCABPagaloTodoMS.Core.Database;
using MediatR;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Application.Exceptions;
using XAct;
using UCABPagaloTodoMS.Application.BusinessValidation;
using Microsoft.Extensions.Configuration;
using UCABPagaloTodoMS.Application.Validators;
using FluentValidation.Results;

namespace UCABPagaloTodoMS.Application.Handlers.Queries
{
     public class LoginUsuariosQueryHandler : IRequestHandler<LoginUsuariosQuery, LoginResponse>
    {
         private readonly IUCABPagaloTodoDbContext _dbContext;
         private readonly ILogger<LoginUsuariosQuery> _logger;
        private readonly IConfiguration _config;


        public LoginUsuariosQueryHandler(IUCABPagaloTodoDbContext dbContext, ILogger<LoginUsuariosQuery> logger, IConfiguration config)
         {
             _dbContext = dbContext;
             _logger = logger;
            _config = config;

         }

         public Task<LoginResponse> Handle(LoginUsuariosQuery request, CancellationToken cancellationToken)
         {
             try
             {

                
                 if (request is null)
                 {
                    throw new CustomException(new() { "Disculpe, hubo un error, por favor intente mas tarde" }); ;
                }
                 else
                 {
                     return HandleAsync(request);
                 }

             }
             catch (Exception ex) { throw new CustomException(new() { "Disculpe, hubo un error, por favor intente mas tarde", ex.Message }); }
         }

         private async Task<LoginResponse> HandleAsync(LoginUsuariosQuery request)
         {
             try
             {
                 _logger.LogInformation("LoginUsuariosQueryHandler.HandleAsync");
                var validator = new LoginValidator();
                ValidationResult result = validator.Validate(request);
                if (result.IsValid)
                {
                    var loginValidation = new LoginValidation(_dbContext);
                    request.clave = Encriptacion.EncriptarClave(request.clave);
                    var user = await loginValidation.ValidateCredentials(request);
                    var loginResponse = new LoginResponse()
                    {
                        Username = user.Username,
                        TipoUsuario = user.GetType().Name.Replace("Entity","")
                    };
                    loginResponse.TipoUsuario = AuthenticationValidation.Generate(loginResponse, _config);
                    return loginResponse;
                }
                else
                {
                    throw new ValidatorException(result);
                }
            }
            catch (ValidatorException ex)
            {
                throw new CustomException(ex.GetMessages());
            }
            catch (SQLException ex)
            {
                throw new CustomException(ex.GetErrorMessage());
            }
            catch (CustomException ex)
            {
                throw new CustomException(ex.GetErrorMessage(), ex.GetErrorCode());
            }
            catch (Exception ex) { throw new CustomException(new() { "Disculpe, hubo un error, por favor intente mas tarde", ex.Message }); }
         }
    }
}
