﻿using Context.Data;
using Microsoft.EntityFrameworkCore;
using Repository.GenericRepository.Interfaces;
using System.Linq.Expressions;

namespace Repository.GenericRepository.Implentations
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext context;
        private readonly DbSet<T>? dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.dbSet = this.context.Set<T>();
        }
        #region Private Methods
        private IQueryable<T> ApplyIncludeApplyFilter(IQueryable<T> query, Expression<Func<T, bool>> filterById, string includeProperties)
        {
            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            if (filterById != null)
                query = query.Where(filterById);

            return query;
        }
        #endregion
        public virtual async Task Create(T entity, CancellationToken cancellationToken)
        {
            await dbSet!.AddAsync(entity, cancellationToken);
        }

        public virtual async Task Delete(Guid id, CancellationToken cancellationToken)
        {
            T? entityToDelete = await dbSet!.FindAsync(id, cancellationToken);
            if (entityToDelete != null)
            {
                if (context!.Entry(entityToDelete)!.State == EntityState.Detached)
                    dbSet!.Attach(entityToDelete);
                dbSet!.Remove(entityToDelete);
            }
        }

        public virtual void DeleteByRange(IEnumerable<T> entity)
        {
            dbSet!.RemoveRange(entity);
        }

        public virtual async Task<IEnumerable<T?>> ReadAll(CancellationToken cancellationToken, Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = dbSet!;
            query = ApplyIncludeApplyFilter(query, filter!, includeProperties);

            if (orderBy != null)
                return await orderBy(query).ToListAsync(cancellationToken);
            else
                return await query.ToListAsync();
        }

        public virtual async Task<T?> ReadById(CancellationToken cancellationToken, Expression<Func<T, bool>>? filter, string includeProperties = "")
        {
            IQueryable<T> query = dbSet!;
            query = ApplyIncludeApplyFilter(query, filter!, includeProperties);
            return await query.FirstOrDefaultAsync(cancellationToken);
        }
        public virtual async Task Update(Guid id, T newEntityToUpdate, CancellationToken cancellationToken)
        {
            T? entityToUpdate = await dbSet!.FindAsync(id, cancellationToken);

            if (entityToUpdate != null && entityToUpdate != newEntityToUpdate)
                context.Entry(context.Set<T>().Find(id)!)
                    .CurrentValues.SetValues(newEntityToUpdate);
        }
    }
}
