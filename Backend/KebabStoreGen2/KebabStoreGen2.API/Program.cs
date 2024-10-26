using KebabStoreGen2.API.Extensions;

namespace KebabStoreGen2.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.ConfigureServices(builder.Configuration);
            builder.Services.AddSerilogServices();
            builder.Services.AddValidatorServices();
            builder.Services.AddSwaggerServices();
            builder.Services.AddAuthenticationServices(builder.Configuration); // «аготовка на Gen3, будет разделен на 2 сервиса

            var app = builder.Build();
            app.ConfigureMiddleware();

            app.Run();
        }
    }
}
