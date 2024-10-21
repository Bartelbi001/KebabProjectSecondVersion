using KebabStoreGen2.API.Extensions;
using Serilog;

namespace KebabStoreGen2.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Log.Information("Starting the application");

                var builder = WebApplication.CreateBuilder(args);

                builder.Services.ConfigureServices(builder.Configuration);
                builder.Services.AddSerilogServices();
                builder.Services.AddValidatorServices();
                builder.Services.AddSwaggerServices();
                builder.Services.AddAuthenticationServices(builder.Configuration); // «аготовка на Gen3, будет разделен на 2 сервиса

                var app = builder.Build();
                app.ConfigureMiddleware();

                Log.Information("Application configured successfully");
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
