
using FluentValidation;
using KebabStoreGen2.Application.Services;
using KebabStoreGen2.Core.Abstractions;
using KebabStoreGen2.Core.Contracts;
using KebabStoreGen2.DataAccess;
using KebabStoreGen2.DataAccess.Repositories;
using KebabStoreGen2.Validation.Validators;
using Microsoft.EntityFrameworkCore;

namespace KebabStoreGen2.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<KebabStoreGen2DbContext>(
                options =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(KebabStoreGen2DbContext)));
                });



            builder.Services.AddScoped<IKebabService, KebabsService>();
            builder.Services.AddScoped<IKebabsRepository, KebabRepository>();
            builder.Services.AddScoped<IImageService, ImagesService>();

            builder.Services.AddTransient<IValidator<KebabsRequest>, KebabStoreGen2Validator>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
