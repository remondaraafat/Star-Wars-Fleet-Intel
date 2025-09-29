namespace API.Extensions
{
    public static class ApplicationBuilderExtensions
    {

        public static IApplicationBuilder UseSwaggerUIWithSetup(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShipConnect API v1");
                c.RoutePrefix = string.Empty; // لو عايز Swagger يفتح على طول على / (الصفحة الرئيسية)
            });

            return app;
        }
    }

}
