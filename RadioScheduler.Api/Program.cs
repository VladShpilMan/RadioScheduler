using RadioScheduler.Api.Middleware;
using RadioScheduler.Api.Setup;

namespace RadioScheduler.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Register services
            builder.Services.AddServices(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                IConfigurationSection swaggerSettings = builder.Configuration.GetSection("Swagger");
                c.SwaggerEndpoint(
                    swaggerSettings["EndpointUrl"],
                    swaggerSettings["ApiName"]
                );
            });

            app.UseCors("AllowAll");
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }

    }
}
