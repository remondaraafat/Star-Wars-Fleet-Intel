using CorrelationId;
using CorrelationId.Abstractions;
using Infrastructure.SwapiProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddHttpClient<SwapiClient>(client =>
            //{
            //    client.BaseAddress = new Uri(configuration["SwapiClient:BaseUrl"]);
            //    client.Timeout = TimeSpan.FromSeconds(configuration.GetValue<int>("SwapiClient:TimeoutSeconds"));
            //});
            //services.AddTransient<SwapiClient>();
            services.AddTransient<IFakeSwapiProvider, FakeSwapiProvider>();
            services.AddScoped<CorrelationContextAccessor>();
            services.AddScoped<ICorrelationContextAccessor>(sp => sp.GetRequiredService<CorrelationContextAccessor>());
            return services;
        }
    }
}
