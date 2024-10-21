using KebabStoreGen2.API.Middlewares;

namespace KebabStoreGen2.API.Extensions;

public static class MiddlewareExtensions
{
    public static void ConfigureMiddleware(this IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<GlobalExceptionMiddleware>();
        app.UseHttpsRedirection();

        if (app.ApplicationServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
