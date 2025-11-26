using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NeuroSync.Application.Common;
using NeuroSync.Application.DTOs.Auth;
using NeuroSync.Domain.Repositories;

namespace NeuroSync.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUsuarioRepository usuarioRepository, IOptions<JwtSettings> jwtOptions, ILogger<AuthService> logger)
        {
            _usuarioRepository = usuarioRepository;
            _jwtSettings = jwtOptions.Value;
            _logger = logger;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
        {
            var usuario = await _usuarioRepository.ObterPorEmailAsync(request.Email, cancellationToken);
            if (usuario == null)
            {
                throw new BusinessException("Usuário ou senha inválidos.", HttpStatusCode.Unauthorized);
            }

            if (!SenhaValida(request.Senha, usuario.SenhaHash))
            {
                usuario.RegistrarTentativaFalha();
                await _usuarioRepository.SaveChangesAsync(cancellationToken);
                throw new BusinessException("Usuário ou senha inválidos.", HttpStatusCode.Unauthorized);
            }

            usuario.RegistrarLoginSucesso();
            await _usuarioRepository.SaveChangesAsync(cancellationToken);

            var perfis = usuario.Perfis.Select(p => p.Perfil?.NomePerfil)
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .Cast<string>()
                .ToList();

            var tokenString = GerarToken(usuario.Id, usuario.EmailCorporativo, usuario.NomeCompleto, perfis);

            _logger.LogInformation("Usuário {Email} autenticado com sucesso.", usuario.EmailCorporativo);

            return new AuthResponseDto
            {
                Token = tokenString,
                ExpiraEm = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                UsuarioId = usuario.Id,
                Nome = usuario.NomeCompleto,
                Email = usuario.EmailCorporativo,
                Perfis = perfis
            };
        }

        private bool SenhaValida(string senhaDigitada, string valorArmazenado)
        {
            if (string.IsNullOrWhiteSpace(valorArmazenado))
            {
                return false;
            }

            var hashDigitada = PasswordHasher.Hash(senhaDigitada);
            if (hashDigitada.Equals(valorArmazenado, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            // fallback para cenários em que o banco ainda guarda a senha em texto puro
            if (!EhSha256(valorArmazenado) && senhaDigitada.Equals(valorArmazenado, StringComparison.Ordinal))
            {
                return true;
            }

            return false;
        }

        private static bool EhSha256(string valor)
        {
            if (valor.Length != 64)
            {
                return false;
            }

            foreach (var c in valor)
            {
                var isHex = (c >= '0' && c <= '9') ||
                            (c >= 'a' && c <= 'f') ||
                            (c >= 'A' && c <= 'F');
                if (!isHex)
                {
                    return false;
                }
            }

            return true;
        }

        private string GerarToken(int usuarioId, string email, string nome, IEnumerable<string> perfis)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, usuarioId.ToString()),
                new(JwtRegisteredClaimNames.Email, email),
                new("name", nome)
            };

            claims.AddRange(perfis.Select(perfil => new Claim(ClaimTypes.Role, perfil)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
