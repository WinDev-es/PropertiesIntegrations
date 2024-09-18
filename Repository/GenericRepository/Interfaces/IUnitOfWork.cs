using Repository.SpecificRepository.Interfaces;

namespace Repository.GenericRepository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        IPropertyRepository PropertyRepository { get; }
        IPropertyImageRepository PropertyImageRepository { get; }
        IOwnerRepository OwnerRepository { get; }
    }
}
