using BusinessLogic.Contracts;
using BusinessLogic.ServicesLogic;

public static class BusinessServicesExtension
{
    public static void ResolveServiceLogicDependency(this IServiceCollection services)
    {
        services.AddScoped<IPropertyService, PropertyService>();
        services.AddScoped<IPropertyImageService, PropertyImageService>();
    }
}