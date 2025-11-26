using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NeuroSync.Application.DTOs.Preferencias;

namespace NeuroSync.Application.Services
{
    public interface IPreferenciaSensorialService
    {
        Task<PreferenciaSensorialDto> CriarAsync(CreatePreferenciaSensorialDto dto, CancellationToken cancellationToken = default);
        Task<IEnumerable<PreferenciaSensorialDto>> ObterPorUsuarioAsync(int usuarioId, CancellationToken cancellationToken = default);
    }
}
