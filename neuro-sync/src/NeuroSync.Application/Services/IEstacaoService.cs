using System;
using System.Threading;
using System.Threading.Tasks;
using NeuroSync.Application.DTOs.Estacoes;
using NeuroSync.Application.Responses;

namespace NeuroSync.Application.Services
{
    public interface IEstacaoService
    {
        Task<EstacaoTrabalhoDto> CriarAsync(CreateEstacaoTrabalhoDto dto, CancellationToken cancellationToken = default);
        Task<EstacaoTrabalhoDto?> ObterAsync(int id, CancellationToken cancellationToken = default);
        Task<PagedResult<EstacaoTrabalhoDto>> BuscarAsync(EstacaoSearchRequest request, CancellationToken cancellationToken = default);
        Task AtualizarAsync(int id, CreateEstacaoTrabalhoDto dto, CancellationToken cancellationToken = default);
        Task RemoverAsync(int id, CancellationToken cancellationToken = default);
        Task<IndiceConfortoDto> CalcularIndiceConfortoAsync(int id, DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);
        Task<TaxaOcupacaoDto> CalcularTaxaOcupacaoAsync(int id, DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);
    }
}
