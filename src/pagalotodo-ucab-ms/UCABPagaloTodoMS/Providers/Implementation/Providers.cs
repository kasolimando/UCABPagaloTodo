using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Diagnostics.CodeAnalysis;
using UCABPagaloTodoMS.Infrastructure.Database;
using UCABPagaloTodoMS.Infrastructure.Settings;
using UCABPagaloTodoMS.Providers.Interface;

namespace UCABPagaloTodoMS.Providers.Implementation
{
    [ExcludeFromCodeCoverage]
    public class Providers : IProviders
    {
        private const string AllowAllOriginsPolicy = "_AllowAllOriginsPolicy";

        public IServiceCollection AddAuthorizationServices(IServiceCollection services, IConfiguration configuration,
            AppSettings appSettings)
        {
            services.AddAuthorization();
            services.AddAuthentication("Bearer").AddIdentityServerAuthentication(options =>
            {
                options.Authority = configuration["IdentityServerUrl"];
                options.RequireHttpsMetadata = false;
                options.ApiName = appSettings.ApiName;
            });
            return services;
        }


        public IServiceCollection AddControllers(IServiceCollection services, IConfiguration configuration,
            AppSettings appSettings)
        {
            services.AddControllers();
           
            return services;
        }

        public IServiceCollection AddCors(IServiceCollection services)
        {
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddCors(options =>
            {
                options.AddPolicy(AllowAllOriginsPolicy,
                    builder =>
                    {
                        builder.AllowAnyMethod();
                        builder.AllowAnyHeader();
                    });
            });
            return services;
        }

        public IServiceCollection AddDatabaseService(IServiceCollection services, IConfiguration configuration,
            string environment, bool isRequired)
        {
            string DBConnectionString = configuration["DBConnectionString"];
            services.AddDbContext<UCABPagaloTodoDbContext>(options => options.UseSqlServer(DBConnectionString));
           

            services.AddHealthChecks()
                .AddDbContextCheck<UCABPagaloTodoDbContext>(null, null, new[] { "ready" });
            return services;
        }

        public IServiceCollection AddHealthCheck(IServiceCollection services, IConfiguration configuration,
            AppSettings appSettings)
        {
            services.AddHealthChecks();
            return services;
        }


        public IServiceCollection AddSwagger(IServiceCollection services, string versionNumber, AppSettings appSettings)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(versionNumber,
                    new OpenApiInfo
                    {
                        Title = "API Starter",
                        Version = versionNumber,
                        Description = "An API to perform The Starter Ops",
                        Contact = new OpenApiContact
                        {
                            Name = "Ucab DS",
                            Email = appSettings.SharedMail,
                            Url = new Uri(appSettings.UCABUrl)
                        },
                        License = new OpenApiLicense { Name = "UCAB", Url = new Uri(appSettings.UCABUrl) }
                    });
                c.AddSecurityDefinition("Authorization",
                    new OpenApiSecurityScheme
                    {
                        Description =
                            "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                        In = ParameterLocation.Header,
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        BearerFormat = "JWT"
                    });

                c.AddSecurityDefinition("Application-Key",
                    new OpenApiSecurityScheme
                    {
                        Description = "Identify the client from the api management and the subscription that uses it",
                        In = ParameterLocation.Header,
                        Name = "Application-Key",
                        Type = SecuritySchemeType.ApiKey
                    });
            });
            return services;
        }
    }
}
