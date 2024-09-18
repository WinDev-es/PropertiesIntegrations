using DataTransferObjects.Dto.System;
using DataTransferObjets.Configuration;

namespace PropertySystem.Extension
{
    public static class SwaggerServicesExtension
    {
        public static void ResolveDependencyServicesSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            AzureConfigurations configuracionAzure = configuration.GetSection(StaticDefination.AzureActiveDirectory).Get<AzureConfigurations>();
            string scope = $"https://{configuracionAzure.Domain}{configuracionAzure.ClientId}/access_as_user";
            string autorizacion = $"{configuracionAzure.Instance}{configuracionAzure.Domain}{configuracionAzure.SignUpSignInPolicyId}/oauth2/v2.0/authorize";
            string token = $"{configuracionAzure.Instance}{configuracionAzure.Domain}{configuracionAzure.SignUpSignInPolicyId}/oauth2/v2.0/token";
            services.AddEndpointsApiExplorer(); // Añadido para OpenAPI/Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "System Property APIS MicroService",
                    Version = "1.0",
                    Description = "Set of end points exposed as tests. " +
                    "This microservice exposes a set of endpoints related " +
                    "to real estate, allowing the management of properties," +
                    " associated images and the search for properties according " +
                    "to different criteria. Allowing efficient and flexible " +
                    "management of both properties and the images associated with " +
                    "them. The microservice architecture is geared towards facilitating" +
                    " CRUD (Create, Read, Update, Delete) operations for properties and" +
                    " their images, as well as advanced queries that allow users to find" +
                    " properties according to specific criteria"
                });

            });
        }
    }
}
