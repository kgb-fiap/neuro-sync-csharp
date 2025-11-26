using System.Threading;
using System.Threading.Tasks;
using NeuroSync.Domain.Entities;

namespace NeuroSync.Domain.Repositories
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}
