using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NeuroSync.Domain.Entities;
using NeuroSync.Domain.Repositories;
using NeuroSync.Infrastructure.Persistence;

namespace NeuroSync.Infrastructure.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly NeuroSyncDbContext Context;
        protected readonly DbSet<T> DbSet;

        public EfRepository(NeuroSyncDbContext context)
        {
            Context = context;
            DbSet = context.Set<T>();
        }

        public IQueryable<T> Query() => DbSet.AsQueryable();

        public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await DbSet.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await DbSet.AddAsync(entity, cancellationToken);
        }

        public void Update(T entity) => DbSet.Update(entity);

        public void Remove(T entity) => DbSet.Remove(entity);

        public Task SaveChangesAsync(CancellationToken cancellationToken = default) => Context.SaveChangesAsync(cancellationToken);
    }
}
