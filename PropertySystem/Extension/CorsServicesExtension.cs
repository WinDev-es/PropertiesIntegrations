using DataTransferObjets.Configuration;

namespace PropertySystem.Extension
{
    public static class CorsServicesExtension
    {
        public static void ResolverCors(this IServiceCollection servicios)
        {
            servicios.AddCors(options =>
            {
                options.AddPolicy(name: StaticDefination.NameOrigins,
                    builder =>
                    {
                        builder.AllowAnyOrigin();
                        builder.AllowAnyMethod();
                        builder.AllowAnyHeader();
                    });
            });
        }
    }
}
