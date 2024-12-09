using KebabStoreGen2.Application.Services;
using KebabStoreGen2.Core.Abstractions;
using KebabStoreGen2.DataAccess;
using KebabStoreGen2.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KebabStoreGen2.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddDbContext<KebabStoreGen2DbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString(nameof(KebabStoreGen2DbContext)));
            });

            services.AddScoped<IKebabService, KebabService>();
            services.AddScoped<IKebabRepository, KebabRepository>();
            services.AddScoped<IIngredientService, IngredientService>();
            services.AddScoped<IIngredientRepository, IngredientRepository>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<NutritionAndWeightCalculatorService, CalculateTotalNutritionsService>();
        }
    }
}