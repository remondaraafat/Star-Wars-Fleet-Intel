using Application.Interfaces;
using CorrelationId.DependencyInjection;
using Infrastructure.Client;
using Infrastructure.SwapiProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using System.Net.Http;

namespace Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Bind settings
            services.Configure<SwapiClientSettings>(configuration.GetSection("SwapiClient"));

            // Optionally validate settings
            services.AddOptions<SwapiClientSettings>()
                .Validate(s => Uri.IsWellFormedUriString(s.BaseUrl, UriKind.Absolute), "SwapiClient:BaseUrl must be a valid URL")
                .Validate(s => s.TimeoutSeconds > 0, "TimeoutSeconds must be > 0");

            // Register HttpClient using the options
            services.AddHttpClient<ISwapiClient, SwapiClient>((sp, client) =>
            {
                var settings = sp.GetRequiredService<IOptions<SwapiClientSettings>>().Value;
                client.BaseAddress = new Uri(settings.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(settings.TimeoutSeconds);
            })
            .AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(4)
                }));

            // Register other services
            services.AddTransient<IFakeSwapiProvider, FakeSwapiProvider>();
            services.AddDefaultCorrelationId();


            return services;
        }
    }
}
