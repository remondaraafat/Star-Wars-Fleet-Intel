
using API.Extensions;
using CorrelationId;
using CorrelationId.DependencyInjection;
using Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using StarWars.Application;
using StarWars.Models;
using System.Security.Claims;
using System.Text;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpContextAccessor();

            // Configure Serilog
            builder.Host.UseSerilog((ctx, lc) => lc
                .ReadFrom.Configuration(ctx.Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithCorrelationId());

            // Add services to the container.
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.ConfigureSwagger();
            builder.Services.AddControllers();
            
            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });
            //correlation id
            builder.Services.AddCorrelationId(options =>
            {
                options.CorrelationIdGenerator = () => Guid.NewGuid().ToString();
                options.RequestHeader = "X-Correlation-Id";
                options.ResponseHeader = "X-Correlation-Id";
                options.IncludeInResponse = true;
                options.UpdateTraceIdentifier = true;
                options.EnforceHeader = false;
                options.AddToLoggingScope = true;
            });

            

            var app = builder.Build();

            app.UseCorrelationId();
            // Swagger 
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShipConnect API");
                c.RoutePrefix = string.Empty; 
            });

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("AllowAngularApp");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            

            app.Run();
        }
    }
}
