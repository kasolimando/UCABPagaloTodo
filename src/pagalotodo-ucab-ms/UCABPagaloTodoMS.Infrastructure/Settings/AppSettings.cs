using System.Diagnostics.CodeAnalysis;

namespace UCABPagaloTodoMS.Infrastructure.Settings;

[ExcludeFromCodeCoverage]
public class AppSettings
{
    public string? TermsOfService { get; set; }

    public string? UCABUrl { get; set; }

    public string? SharedMail { get; set; }

    public string? MicroserviceName { get; set; }

    public string? ApiName { get; set; }

    public string? SwaggerStyle { get; set; }

    public bool RequireDatabase { get; set; }

    public bool RequireAzureStorage { get; set; }

    public bool RequireAuthorization { get; set; }

    public bool RequireSwagger { get; set; }

    public bool RequireControllers { get; set; }

    public string? RequestChannel { get; set; }

    public string? ApiUserName { get; set; }
}
