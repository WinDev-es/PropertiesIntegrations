using System.Linq.Expressions;

namespace Repository.GenericRepository.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        private const string Empty = "";
        Task Create(T entity, CancellationToken cancellationToken);
        Task<IEnumerable<T?>> ReadAll(
            CancellationToken cancellationToken,
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = Empty);
        Task<T?> ReadById(CancellationToken cancellationToken, Expression<Func<T, bool>>? filter, string includeProperties = Empty);
        Task Update(Guid id, T newEntityToUpdate, CancellationToken cancellationToken);
        Task Delete(Guid id, CancellationToken cancellationToken);
        void DeleteByRange(IEnumerable<T> entity);
    }
}
