using System.Threading;
using System.Threading.Tasks;
using NeuroSync.Application.DTOs.Auth;

namespace NeuroSync.Application.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);
    }
}
