using System;
using System.Threading;
using System.Threading.Tasks;
using NeuroSync.Domain.Entities;

namespace NeuroSync.Domain.Repositories
{
    public interface IEstacaoTrabalhoRepository : IRepository<EstacaoTrabalho>
    {
        Task<decimal> CalcularIndiceConfortoAsync(int estacaoId, DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);
        Task<decimal> CalcularTaxaOcupacaoAsync(int estacaoId, DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);
    }
}
