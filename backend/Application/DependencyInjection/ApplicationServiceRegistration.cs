using Application.ChainHandler;
using Application.Interfaces;
using Application.Services;
using Application.Servicies;
using Application.Validators;
using CorrelationId.Abstractions;
using Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace StarWars.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddSingleton<ISwapiFacadeService, SwapiFacadeService>();
            services.AddSingleton<IValidator<Starship>, PreFlightChecks>();
            services.AddSingleton<IStarshipHandler, ValidationHandler>();
            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddSingleton<ObjectPool<ValidationHandler>>(provider =>
            {
                var poolProvider = provider.GetRequiredService<ObjectPoolProvider>();
                return poolProvider.Create(new ValidationHandlerPolicy(provider));
            });







            return services;
        }
    }
}
