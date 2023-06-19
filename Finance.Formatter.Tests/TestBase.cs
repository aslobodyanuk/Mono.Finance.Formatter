using Finance.Formatter.Core;
using Finance.Formatter.Models.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Reflection;

namespace Finance.Formatter.Tests
{
    public abstract class TestBase
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public ILogger Logger { get; private set; }

        protected TestBase()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                        .AddJsonFile("appSettings.json");

            IConfigurationRoot configuration = builder.Build();

            var services = new ServiceCollection();

            services.Configure<AppConfig>(configuration.GetSection("AppConfig"));

            services.AddLogging();
            DIConfiguration.RegisterCoreServices(services);

            ServiceProvider = services.BuildServiceProvider();
            Logger = ServiceProvider.GetRequiredService<ILogger<TestBase>>();

            AutoResolveServices();
        }

        protected void CloseAllExcelInstances()
        {
            var process = Process.GetProcessesByName("Excel");

            foreach (Process p in process)
            {
                if (!string.IsNullOrEmpty(p.ProcessName))
                {
                    try
                    {
                        p.Kill();
                    }
                    catch { }
                }
            }
        }

        protected void OpenExcelFile(string filePath)
        {
            var psInfo = new ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true
            };

            Process.Start(psInfo);
        }

        private void AutoResolveServices()
        {
            Type type = GetType();

            FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            IEnumerable<FieldInfo> autoResolveFields = fields.Where(x => x.GetCustomAttributes(typeof(AutoResolveAttribute), false).Any());

            foreach (FieldInfo field in autoResolveFields)
            {
                object service = ServiceProvider.GetRequiredService(field.FieldType);
                field.SetValue(this, service);
            }

            PropertyInfo[] properties = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);
            IEnumerable<PropertyInfo> autoResolveProperties = properties.Where(x => x.GetCustomAttributes(typeof(AutoResolveAttribute), false).Any());

            foreach (PropertyInfo property in autoResolveProperties)
            {
                object service = ServiceProvider.GetRequiredService(property.PropertyType);
                property.SetValue(this, service);
            }
        }
    }
}
