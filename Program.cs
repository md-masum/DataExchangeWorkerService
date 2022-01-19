using DataExchangeWorkerService.Helpers;
using DataExchangeWorkerService.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DataExchangeWorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;
                    WorkerOptions options = configuration.GetSection("WorkerOptions").Get<WorkerOptions>();
                    services.AddSingleton(options);
                    services.AddSingleton<EtlProcessor>();
                    services.AddHostedService<Worker>();
                });
    }
}
