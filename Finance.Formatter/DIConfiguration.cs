namespace Finance.Formatter
{
    public static class DIConfiguration
    {
        public static MauiAppBuilder RegisterDependencies(this MauiAppBuilder builder)
        {
            var services = builder.Services;

            services.AddSingleton<AppShell>();
            services.AddSingleton<MainPage>();

            return builder;
        }
    }
}
