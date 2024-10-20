﻿using KebabStoreGen2.Application.Services;
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

            services.AddScoped<IKebabService, KebabsService>();
            services.AddScoped<IKebabsRepository, KebabRepository>();
            services.AddScoped<IImageService, ImagesService>();
        }
    }
}
