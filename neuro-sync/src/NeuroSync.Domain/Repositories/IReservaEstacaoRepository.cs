using System.Threading;
using System.Threading.Tasks;
using NeuroSync.Domain.Entities;

namespace NeuroSync.Domain.Repositories
{
    public interface IReservaEstacaoRepository : IRepository<ReservaEstacao>
    {
        Task<bool> ReservaCompativelAsync(int reservaId, CancellationToken cancellationToken = default);
    }
}
