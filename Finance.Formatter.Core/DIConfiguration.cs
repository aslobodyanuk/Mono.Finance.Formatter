using AutoMapper;
using Finance.Formatter.Core.Mapper;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Finance.Formatter.Core
{
    public static class DIConfiguration
    {
        public static MauiAppBuilder RegisterCoreMAUIServices(this MauiAppBuilder builder)
        {
            var services = builder.Services;

            builder.RegisterConfiguration();
            services.RegisterCoreServices();

            return builder;
        }

        public static IServiceCollection RegisterCoreServices(this IServiceCollection services)
        {
            services.AddTransient<ExchangeService>();
            services.AddTransient<ProcessingService>();

            services.AddSingleton<FilePickerService>();
            services.AddSingleton<CSVParserService>();
            services.AddSingleton<ExportService>();

            services.AddSingleton<IMapperProvider, MapperProvider>();
            services.AddSingleton(x => GetMapper(x));

            return services;
        }

        public static void RegisterConfiguration(this MauiAppBuilder builder)
        {
            var coreAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;

            using (var stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream($"{coreAssemblyName}.appSettings.json"))
            {
                var config = new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build();

                builder.Configuration.AddConfiguration(config);
            }
        }

        private static IMapper GetMapper(IServiceProvider serviceProvider)
        {
            var provider = serviceProvider.GetRequiredService<IMapperProvider>();
            return provider.GetMapper();
        }
    }
}
