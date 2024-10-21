using Serilog;

namespace KebabStoreGen2.API.Extensions;

public static class SerilogExtensions
{
    public static void AddSerilogServices(this IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/KebabStoreGen2.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

        services.AddSingleton(Log.Logger);
    }
}
