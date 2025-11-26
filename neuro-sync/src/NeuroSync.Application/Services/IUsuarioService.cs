using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NeuroSync.Application.DTOs;
using NeuroSync.Application.DTOs.Usuarios;
using NeuroSync.Application.Responses;

namespace NeuroSync.Application.Services
{
    public interface IUsuarioService
    {
        Task<UsuarioDto> CriarAsync(CreateUsuarioDto dto, CancellationToken cancellationToken = default);
        Task<UsuarioDto> AtualizarAsync(UpdateUsuarioDto dto, CancellationToken cancellationToken = default);
        Task<UsuarioDto?> ObterAsync(int id, CancellationToken cancellationToken = default);
        Task<PagedResult<UsuarioDto>> BuscarAsync(PagedRequest request, CancellationToken cancellationToken = default);
        Task RemoverAsync(int id, CancellationToken cancellationToken = default);
    }
}
