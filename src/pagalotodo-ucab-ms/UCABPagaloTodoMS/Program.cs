using Destructurama;
using Microsoft.AspNetCore;
using Serilog;
using System.Diagnostics.CodeAnalysis;

namespace UCABPagaloTodoMS;

[ExcludeFromCodeCoverage]
public static class Program
{
    public static int Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        try
        {
            Log.Information("Starting web host");
            AppContext.SetSwitch("SqlServer.EnableLegacyTimestampBehavior", true);
            CreateHostBuilder(args).Build().Run();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .UseSerilog((context, services, configuration) => configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .Destructure.JsonNetTypes())
            .ConfigureAppConfiguration((_, config) =>
            {
                config.AddEnvironmentVariables("NASSA_");
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            }).UseDefaultServiceProvider(options =>
    options.ValidateScopes = false);
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
    WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>()
        .UseUrls("http://localhost:5000", "https://localhost:5000");
}
