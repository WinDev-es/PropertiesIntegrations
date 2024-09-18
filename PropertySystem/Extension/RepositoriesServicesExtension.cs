using Context.Data;
using Microsoft.EntityFrameworkCore;
using Repository.GenericRepository.Implentations;
using Repository.GenericRepository.Interfaces;
using Repository.SpecificRepository.Implentations;
using Repository.SpecificRepository.Interfaces;

namespace PropertySystem.Extension
{
    public static class RepositoriesServicesExtension
    {
        public static void ResolveWorkUnitDependency(this IServiceCollection services, string ConnectionChain)
        {
            // Configura el DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(ConnectionChain)); // O usa otro proveedor si es necesario

            // Registra las fábricas de servicios
            services.AddScoped<IUnitOfWork>(provider =>
                new UnitOfWork(provider.GetRequiredService<ApplicationDbContext>()));

            services.AddScoped<IPropertyRepository>(provider =>
                new PropertyRepository(provider.GetRequiredService<ApplicationDbContext>()));

            services.AddScoped<IPropertyImageRepository>(provider =>
                new PropertyImageRepository(provider.GetRequiredService<ApplicationDbContext>()));

            //services.AddScoped<IOwnerRepository>(provider =>
            //    new OwnerRepository(provider.GetRequiredService<ApplicationDbContext>()));
        }
    }
}
