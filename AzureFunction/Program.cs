using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PhotoStorageIsolated.Middlewares;
using System.IO;
using System.Threading.Tasks;
using PhotoStorageIsolated.Interfaces;
using PhotoStorageIsolated.Services;

namespace PhotoStorageIsolated
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureAppConfiguration(configurationBuilder =>
                {
                    configurationBuilder.AddCommandLine(args);
                    configurationBuilder.SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("local.settings.json", false, true)
                        .AddEnvironmentVariables();
                    var configuration = configurationBuilder.Build();
                })
                .ConfigureFunctionsWorkerDefaults(
                    builder =>
                    {
                        builder.UseMiddleware<ExceptionLoggingMiddleware>();
                    }
                ).ConfigureServices(services =>
                {
                    services.AddTransient<IBlobStoragePhotoService, BlobStoragePhotoSerivce>();
                })
                .Build();

            await host.RunAsync(); 
        }

    }
}
