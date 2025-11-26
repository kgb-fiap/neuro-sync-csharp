using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NeuroSync.Application.Common;
using NeuroSync.Application.DTOs;
using NeuroSync.Application.DTOs.Usuarios;
using NeuroSync.Application.Responses;
using NeuroSync.Domain.Entities;
using NeuroSync.Domain.Repositories;

namespace NeuroSync.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogger<UsuarioService> _logger;

        public UsuarioService(IUsuarioRepository usuarioRepository, ILogger<UsuarioService> logger)
        {
            _usuarioRepository = usuarioRepository;
            _logger = logger;
        }

        public async Task<UsuarioDto> CriarAsync(CreateUsuarioDto dto, CancellationToken cancellationToken = default)
        {
            var existente = await _usuarioRepository.ObterPorEmailAsync(dto.EmailCorporativo, cancellationToken);
            if (existente != null)
            {
                throw new BusinessException("Já existe um usuário com este e-mail corporativo.");
            }

            var usuario = new Usuario(dto.SetorId, dto.NomeCompleto, dto.EmailCorporativo, PasswordHasher.Hash(dto.Senha))
            {
                MatriculaInterna = dto.MatriculaInterna,
                Telefone = dto.Telefone,
                DataAdmissao = dto.DataAdmissao,
                FlagNeurodivergente = dto.FlagNeurodivergente
            };

            await _usuarioRepository.AddAsync(usuario, cancellationToken);
            await _usuarioRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Usuário {Email} criado com sucesso.", usuario.EmailCorporativo);

            return Map(usuario);
        }

        public async Task<UsuarioDto> AtualizarAsync(UpdateUsuarioDto dto, CancellationToken cancellationToken = default)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (usuario == null)
            {
                throw new BusinessException("Usuário não encontrado.", HttpStatusCode.NotFound);
            }

            usuario.NomeCompleto = dto.NomeCompleto;
            usuario.EmailCorporativo = dto.EmailCorporativo;
            usuario.MatriculaInterna = dto.MatriculaInterna;
            usuario.Telefone = dto.Telefone;
            usuario.DataAdmissao = dto.DataAdmissao;
            usuario.FlagNeurodivergente = dto.FlagNeurodivergente;
            if (dto.Ativo) usuario.Ativar(); else usuario.Desativar();

            _usuarioRepository.Update(usuario);
            await _usuarioRepository.SaveChangesAsync(cancellationToken);

            return Map(usuario);
        }

        public async Task<UsuarioDto?> ObterAsync(int id, CancellationToken cancellationToken = default)
        {
            var usuario = await _usuarioRepository.Query()
                .Include(u => u.Perfis)
                    .ThenInclude(up => up.Perfil)
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            return usuario is null ? null : Map(usuario);
        }

        public async Task<PagedResult<UsuarioDto>> BuscarAsync(PagedRequest request, CancellationToken cancellationToken = default)
        {
            var query = _usuarioRepository.Query()
                .Include(u => u.Perfis)
                    .ThenInclude(up => up.Perfil)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.SortBy))
            {
                query = request.SortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase)
                    ? query.OrderByDescending(u => EF.Property<object>(u, request.SortBy))
                    : query.OrderBy(u => EF.Property<object>(u, request.SortBy));
            }
            else
            {
                query = query.OrderBy(u => u.NomeCompleto);
            }

            var total = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<UsuarioDto>
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalRecords = total,
                Items = items.Select(Map).ToList()
            };
        }

        public async Task RemoverAsync(int id, CancellationToken cancellationToken = default)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id, cancellationToken);
            if (usuario == null)
            {
                throw new BusinessException("Usuário não encontrado.", HttpStatusCode.NotFound);
            }

            _usuarioRepository.Remove(usuario);
            await _usuarioRepository.SaveChangesAsync(cancellationToken);
        }

        private static UsuarioDto Map(Usuario usuario)
        {
            var perfis = usuario.Perfis.Select(p => p.Perfil?.NomePerfil ?? string.Empty)
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .ToList();

            return new UsuarioDto
            {
                Id = usuario.Id,
                NomeCompleto = usuario.NomeCompleto,
                EmailCorporativo = usuario.EmailCorporativo,
                Telefone = usuario.Telefone,
                MatriculaInterna = usuario.MatriculaInterna,
                Neurodivergente = usuario.FlagNeurodivergente,
                Ativo = usuario.Ativo,
                Perfis = perfis
            };
        }
    }
}
