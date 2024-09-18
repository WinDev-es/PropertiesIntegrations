using AutoMapper;
using DataTransferObjets.Configuration;
using DataTransferObjets.Profiles;
using NSwag.AspNetCore;
using PropertySystem.Extension;
using PropertySystem.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Configura servicios

builder.Services.ResolveServiceLogicDependency();
builder.Services.ResolveWorkUnitDependency(builder.Configuration.GetConnectionString(StaticDefination.DatabaseConnection));
builder.Services.ResolverCors();
builder.Services.ResolveDependencyServicesSwagger(builder.Configuration);
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new PropertySystemProfile());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddControllers();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseRouting();
app.UseOpenApi(); // Servir el archivo OpenAPI/Swagger especificado

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();  // Este genera el archivo swagger.json
    app.UseSwaggerUI(c =>
    {
        // Definir el endpoint de Swagger
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Properties Integrations.PropertySystem");
    });
}
else
{
    app.UseExceptionHandler("/Error"); // Opcional: página de errores genérica
    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseOpenApi(); // Servir el archivo OpenAPI/Swagger especificado
    app.UseSwaggerUi(settings =>
    {
        settings.DefaultModelsExpandDepth = 1; // Cambia según tu configuración
        settings.OAuth2Client = new OAuth2ClientSettings
        {
            ClientId = "5890ac83-afea-4d39-8c1a-51f8fa72f348",
            AppName = "swagger-ui-cliente"
        };
    });
}
app.MapControllers();
app.Run();
