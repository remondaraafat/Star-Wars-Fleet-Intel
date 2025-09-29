using Application.Interfaces;
using CorrelationId.Abstractions;
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
            //services.AddTransient<ISwapiFacadeService, SwapiFacadeService>();
            //services.AddValidatorsFromAssembly(typeof(StarshipValidator).Assembly); // FluentValidation
            //services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>(); // ObjectPool
            //services.AddSingleton<ObjectPool<ValidationHandler>>(sp =>
            //  sp.GetRequiredService<ObjectPoolProvider>().Create(new DefaultPooledObjectPolicy<ValidationHandler>()));
            //services.AddScoped<ICorrelationContextAccessor, Infrastructure.CorrelationContextAccessor>(); // CorrelationId

//            services.AddHttpClient<SwapiClient>(c =>
//            {
//                c.BaseAddress = new Uri(builder.Configuration["Swapi:BaseUrl"]);
//            })
//.AddPolicyHandler(HttpPolicyExtensions
//    .HandleTransientHttpError()
//    .WaitAndRetryAsync(new[]
//    {
//        TimeSpan.FromSeconds(1),
//        TimeSpan.FromSeconds(2),
//        TimeSpan.FromSeconds(4)
//    }));
            return services;
        }
    }
}
