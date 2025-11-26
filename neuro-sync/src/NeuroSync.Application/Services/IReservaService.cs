using System.Threading;
using System.Threading.Tasks;
using NeuroSync.Application.DTOs.Estacoes;
using NeuroSync.Application.DTOs.Reservas;
using NeuroSync.Application.Responses;

namespace NeuroSync.Application.Services
{
    public interface IReservaService
    {
        Task<ReservaEstacaoDto> CriarAsync(CreateReservaEstacaoDto dto, CancellationToken cancellationToken = default);
        Task<PagedResult<ReservaEstacaoDto>> BuscarAsync(ReservaSearchRequest request, CancellationToken cancellationToken = default);
        Task<ReservaEstacaoDto?> ObterAsync(int id, CancellationToken cancellationToken = default);
        Task AtualizarStatusAsync(int id, int novoStatusId, string? motivoCancelamento, CancellationToken cancellationToken = default);
        Task<CompatibilidadeReservaDto> VerificarCompatibilidadeAsync(int id, CancellationToken cancellationToken = default);
        Task RegistrarAvaliacaoAsync(AvaliacaoEstacaoDto dto, CancellationToken cancellationToken = default);
        Task RemoverAsync(int id, CancellationToken cancellationToken = default);
    }
}
