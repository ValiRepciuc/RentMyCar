using System.Reflection;
using Domain.Database;
using Domain.Repositories;
using Infrastructure.Exceptions;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Presentation;

public static class Configuration
{
    public static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DatabaseContext>(options =>
        {
            // configurezi ConnectionString etc.
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });
        return services;
    }


    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "RentMyCar", Version = "v1" });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });
        });

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped<ITokenService, TokenService>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<ICarRepository, CarRepository>()
            .AddScoped<ICarService, CarService>()
            .AddScoped<IBookingRepository, IBookingRepository.BookingRepository>()
            .AddScoped<IBookingService, BookingService>();
        
        return services;
    }
    public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder app) => app.UseExceptionHandler(builder =>
    {
        builder.Run(async context =>
        {
            var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
            if (contextFeature != null)
            {
                if (contextFeature.Error.InnerException is null)
                {
                    context.Items["Exception"] = contextFeature.Error.Message;
                    context.Items["StackTrace"] = contextFeature.Error.StackTrace;
                }
                else
                {
                    context.Items["Exception"] = $"{contextFeature.Error.Message}\n{contextFeature.Error.InnerException.Message}";
                    context.Items["StackTrace"] = $"{contextFeature.Error.StackTrace}\n{contextFeature.Error.InnerException.StackTrace}";
                }

                context.Response.StatusCode = contextFeature.Error switch
                {
                    BadRequestException => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status500InternalServerError
                };

                if (contextFeature.Error is AggregateException aggregateException && aggregateException.InnerExceptions.Any(e => e is BadRequestException))
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(aggregateException.InnerExceptions.Select(e => e.Message));
                }
                else
                {
                    var errors = new List<string> { contextFeature.Error.Message };

                    if (contextFeature.Error.InnerException is not null)
                        errors.Add(contextFeature.Error.InnerException.Message);

                    await context.Response.WriteAsJsonAsync(errors);
                }
            }
        });
    });
}