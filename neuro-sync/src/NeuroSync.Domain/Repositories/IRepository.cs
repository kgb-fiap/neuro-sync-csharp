using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NeuroSync.Domain.Entities;

namespace NeuroSync.Domain.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> Query();
        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        void Update(T entity);
        void Remove(T entity);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
