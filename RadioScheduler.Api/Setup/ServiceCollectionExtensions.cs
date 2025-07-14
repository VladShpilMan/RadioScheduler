using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using RadioScheduler.Application.Commands;
using RadioScheduler.Application.DTOs;
using RadioScheduler.Application.Interfaces;
using RadioScheduler.Application.Queries.GetDailyReport;
using RadioScheduler.Application.Queries.GetShowById;
using RadioScheduler.Application.Queries.GetShowsByDate;
using RadioScheduler.Infrastructure.Logging;
using RadioScheduler.Infrastructure.Logging.Options;
using RadioScheduler.Infrastructure.Repositories;
using RadioScheduler.Infrastructure.Services;
using ILogger = RadioScheduler.Infrastructure.Logging.Interfaces.ILogger;

namespace RadioScheduler.Api.Setup
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            // MediatR configuration
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(CreateShowCommand).Assembly);
                cfg.Lifetime = ServiceLifetime.Scoped;
            });

            services.AddScoped<IMediator, Mediator>();
            services.AddScoped<IRequestHandler<CreateShowCommand, Guid>, CreateShowCommandHandler>();
            services.AddScoped<IRequestHandler<GetShowsByDateQuery, IEnumerable<ShowDto>>, GetShowsByDateQueryHandler>();
            services.AddScoped<IRequestHandler<GetShowByIdQuery, ShowDto>, GetShowByIdQueryHandler>();
            services.AddScoped<IRequestHandler<GetDailyReportQuery, DailyReportDto>, GetDailyReportQueryHandler>();

            // Logging configuration
            services.Configure<LoggingOptions>(configuration.GetSection("AppLogging"));
            services.AddSingleton<ILogger>(provider =>
            {
                LoggingOptions options = provider.GetRequiredService<IOptions<LoggingOptions>>().Value;
                string logFilePath = options.FileLogger?.Path ?? throw new InvalidOperationException("FileLogger.Path is not configured in appsettings.json");
                return options.LoggerType switch
                {
                    "File" => new FileLogger(logFilePath),
                    _ => new FileLogger(logFilePath)
                };
            });

            // Repository and notification services
            services.AddSingleton<IShowRepository, InMemoryShowRepository>();
            services.AddScoped<INotificationService, ConsoleNotificationService>();

            // Controllers and API configuration
            services.AddControllers();
            services.AddEndpointsApiExplorer();

            // Swagger configuration
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RadioScheduler API", Version = "v1" });
            });

            // CORS configuration
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            return services;
        }
    }
}

