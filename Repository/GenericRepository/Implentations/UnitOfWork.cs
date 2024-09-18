using Context.Data;
using Repository.GenericRepository.Interfaces;
using Repository.SpecificRepository.Implentations;
using Repository.SpecificRepository.Interfaces;

namespace Repository.GenericRepository.Implentations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext context;
        public IPropertyRepository PropertyRepository { get; private set; }
        public IPropertyImageRepository PropertyImageRepository { get; private set; }
        public IOwnerRepository OwnerRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
            PropertyRepository = new PropertyRepository(this.context);
            PropertyImageRepository = new PropertyImageRepository(this.context);
            OwnerRepository = new OwnerRepository(this.context);
        }
        public void Dispose()
        {
            context.Dispose();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}
