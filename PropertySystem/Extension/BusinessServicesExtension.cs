using BusinessLogic.Contracts;
using BusinessLogic.ServicesLogic;

public static class BusinessServicesExtension
{
    public static void ResolveServiceLogicDependency(this IServiceCollection services)
    {
        services.AddScoped<IPropertyService, PropertyService>();
        services.AddScoped<IPropertyImageService, PropertyImageService>();


        //var assemblyBusinessLogic = AppDomain.CurrentDomain.Load("BusinessLogic");

        //services.Scan(scan => scan
        //    .FromAssemblies(assemblyBusinessLogic)
        //    .AddClasses(classes => classes
        //        .Where(type => type.Name.Contains("Contracts"))) // Filtra las clases por nombre
        //    .AsImplementedInterfaces() // Registra las clases como sus interfaces
        //    .WithTransientLifetime()); // Configura el ciclo de vida como Transient
    }
}