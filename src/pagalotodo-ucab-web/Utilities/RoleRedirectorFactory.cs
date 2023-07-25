using System.Security.Claims;
using UCABPagaloTodoWeb.Services.Implementation;
using UCABPagaloTodoWeb.Services.Interface;

namespace UCABPagaloTodoWeb.Utilities
{
    public class RoleRedirectorFactory
    {
        private readonly Dictionary<string, Type> _redirectors;

        public RoleRedirectorFactory()
        {
            _redirectors = new Dictionary<string, Type>()
        {
            { "Administrador", typeof(AdminRedirector) },
            { "Consumidor", typeof(ConsumidorRedirector) },
            { "Prestador", typeof(PrestadorRedirector) }
        };
        }

        public IRoleRedirector GetRedirector(Claim roleClaim)
        {
            var roleName = roleClaim?.Value ?? "";
            _redirectors.TryGetValue(roleName, out var redirectorType);
            return (IRoleRedirector)Activator.CreateInstance(redirectorType);
        }
    }
}
