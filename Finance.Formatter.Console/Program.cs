using Finance.Formatter.Core;
using Finance.Formatter.Models.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace Finance.Formatter.Console
{
    public class TestClass
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public static void Main(string[] args)
        {
            System.Console.WriteLine("Input args:", args);

            foreach (var arg in args)
            {
                System.Console.WriteLine(arg);
            }

            IConfigurationBuilder builder = new ConfigurationBuilder()
                        .AddJsonFile("appSettings.json");

            IConfigurationRoot configuration = builder.Build();

            var services = new ServiceCollection();

            services.Configure<AppConfig>(configuration.GetSection("AppConfig"));

            services.AddLogging();
            DIConfiguration.RegisterCoreServices(services);

            ServiceProvider = services.BuildServiceProvider();

            var processingService = ServiceProvider.GetRequiredService<ProcessingService>();
            var exportService = ServiceProvider.GetRequiredService<ExportService>();

            var processed = processingService.ProcessData(args);
            exportService.ExportTransactions(processed, "C:\\Temp\\output2.xlsx");

            var psInfo = new ProcessStartInfo
            {
                FileName = "C:\\Temp\\output2.xlsx",
                UseShellExecute = true
            };

            Process.Start(psInfo);
        }
    }
}
