using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NeuroSync.Domain.Entities;
using NeuroSync.Domain.Repositories;
using NeuroSync.Infrastructure.Persistence;

namespace NeuroSync.Infrastructure.Repositories
{
    public class UsuarioRepository : EfRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(NeuroSyncDbContext context) : base(context)
        {
        }

        public async Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Include(u => u.Perfis)
                    .ThenInclude(up => up.Perfil)
                .FirstOrDefaultAsync(u => u.EmailCorporativo == email, cancellationToken);
        }
    }
}
