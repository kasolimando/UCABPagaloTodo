using System.Reflection;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Infrastructure.Database;
using UCABPagaloTodoMS.Infrastructure.Settings;
using UCABPagaloTodoMS.Providers.Implementation;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using RestSharp;
using MediatR;
using UCABPagaloTodoMS.Application.Handlers.Queries;
using RabbitMQ.Client;
using UCABPagaloTodoMS.Application.Consumers;
using UCABPagaloTodoMS.Application.Handlers.Commands;
using UCABPagaloTodoMS.Infrastructure.Services.RabbitProducer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using System;

namespace UCABPagaloTodoMS;

[ExcludeFromCodeCoverage]
public class Startup
{
    private AppSettings _appSettings;
    private readonly string _allowAllOriginsPolicy = "AllowAllOriginsPolicy";

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
        VersionNumber = "v" + Assembly.GetEntryAssembly()!
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion;
        Folder = "docs";
    }
    private string Folder { get; }
    private string VersionNumber { get; }

    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(_allowAllOriginsPolicy,
                builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                });
        });
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        var appSettingsSection = Configuration.GetSection("AppSettings");
        _appSettings = appSettingsSection.Get<AppSettings>();
        services.Configure<AppSettings>(appSettingsSection);
        services.AddTransient<IUCABPagaloTodoDbContext, UCABPagaloTodoDbContext>();

        services.AddProviders(Configuration, Folder, _appSettings, environment);

        services.AddTransient<IServiceProvider, ServiceProvider>();

        services.AddMediatR(typeof(ConsultarAdminsQueryHandler).GetTypeInfo().Assembly);

        services.AddMediatR(typeof(GuardarDeudasHandler));

        services.AddMediatR(typeof(CargarConciliacionHandler));

        services.AddHostedService<ConsumerDeuda>();

        services.AddHostedService<ConsumerConciliacion>();

        services.AddSingleton(serviceProvider =>
        {
            return new ConnectionFactory
            {
                HostName = "localhost"
            };
        });

        services.AddScoped<IRabbitProducer, RabbitProducer>();

        services.AddScoped<IRabbitProducerConciliacion, RabbitProducerConciliacion>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
            };
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        var appSettingsSection = Configuration.GetSection("AppSettings");
        _appSettings = appSettingsSection.Get<AppSettings>();

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.ApplicationServices.GetService<IHostedService>();

        if (env.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

        }
        if (_appSettings.RequireSwagger)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("./" + Folder + "/swagger.json", $"UCABPagaloTodo Microservice({VersionNumber})");
            });
        }


        if (_appSettings.RequireAuthorization)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }

        if (_appSettings.RequireControllers)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health/ready",
                    new HealthCheckOptions { Predicate = check => check.Tags.Contains("ready") });
                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false });
            });
        }
    }
}
