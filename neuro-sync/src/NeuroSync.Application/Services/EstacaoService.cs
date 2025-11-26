using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NeuroSync.Application.Common;
using NeuroSync.Application.DTOs.Estacoes;
using NeuroSync.Application.Responses;
using NeuroSync.Domain.Entities;
using NeuroSync.Domain.Repositories;

namespace NeuroSync.Application.Services
{
    public class EstacaoService : IEstacaoService
    {
        private readonly IEstacaoTrabalhoRepository _estacaoRepository;
        private readonly ILogger<EstacaoService> _logger;

        public EstacaoService(IEstacaoTrabalhoRepository estacaoRepository, ILogger<EstacaoService> logger)
        {
            _estacaoRepository = estacaoRepository;
            _logger = logger;
        }

        public async Task<EstacaoTrabalhoDto> CriarAsync(CreateEstacaoTrabalhoDto dto, CancellationToken cancellationToken = default)
        {
            var existente = await _estacaoRepository.Query()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.CodigoEstacao == dto.CodigoEstacao, cancellationToken);

            if (existente != null)
            {
                throw new BusinessException("Já existe estação com este código.");
            }

            var estacao = new EstacaoTrabalho(dto.ZonaSensorialId, dto.CodigoEstacao)
            {
                Descricao = dto.Descricao,
                PermiteReserva = dto.PermiteReserva,
                PermiteUsoEspontaneo = dto.PermiteUsoEspontaneo,
                Observacoes = dto.Observacoes,
                StatusEstacao = dto.StatusEstacao
            };

            await _estacaoRepository.AddAsync(estacao, cancellationToken);
            await _estacaoRepository.SaveChangesAsync(cancellationToken);

            return Map(estacao);
        }

        public async Task<EstacaoTrabalhoDto?> ObterAsync(int id, CancellationToken cancellationToken = default)
        {
            var estacao = await _estacaoRepository.Query()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

            return estacao is null ? null : Map(estacao);
        }

        public async Task<PagedResult<EstacaoTrabalhoDto>> BuscarAsync(EstacaoSearchRequest request, CancellationToken cancellationToken = default)
        {
            var query = _estacaoRepository.Query().AsNoTracking();

            if (request.ZonaSensorialId.HasValue)
            {
                query = query.Where(e => e.ZonaSensorialId == request.ZonaSensorialId);
            }

            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                query = query.Where(e => e.StatusEstacao == request.Status);
            }

            if (!string.IsNullOrWhiteSpace(request.Codigo))
            {
                query = query.Where(e => e.CodigoEstacao.Contains(request.Codigo));
            }

            if (!string.IsNullOrWhiteSpace(request.SortBy))
            {
                query = request.SortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase)
                    ? query.OrderByDescending(e => EF.Property<object>(e, request.SortBy))
                    : query.OrderBy(e => EF.Property<object>(e, request.SortBy));
            }
            else
            {
                query = query.OrderBy(e => e.CodigoEstacao);
            }

            var total = await query.CountAsync(cancellationToken);
            var estacoes = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<EstacaoTrabalhoDto>
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalRecords = total,
                Items = estacoes.Select(Map).ToList()
            };
        }

        public async Task AtualizarAsync(int id, CreateEstacaoTrabalhoDto dto, CancellationToken cancellationToken = default)
        {
            var estacao = await _estacaoRepository.GetByIdAsync(id, cancellationToken);
            if (estacao == null)
            {
                throw new BusinessException("Estação não encontrada.", HttpStatusCode.NotFound);
            }

            estacao.ZonaSensorialId = dto.ZonaSensorialId;
            estacao.CodigoEstacao = dto.CodigoEstacao;
            estacao.Descricao = dto.Descricao;
            estacao.PermiteReserva = dto.PermiteReserva;
            estacao.PermiteUsoEspontaneo = dto.PermiteUsoEspontaneo;
            estacao.StatusEstacao = dto.StatusEstacao;
            estacao.Observacoes = dto.Observacoes;

            _estacaoRepository.Update(estacao);
            await _estacaoRepository.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoverAsync(int id, CancellationToken cancellationToken = default)
        {
            var estacao = await _estacaoRepository.GetByIdAsync(id, cancellationToken);
            if (estacao == null)
            {
                throw new BusinessException("Estação não encontrada.", HttpStatusCode.NotFound);
            }

            _estacaoRepository.Remove(estacao);
            await _estacaoRepository.SaveChangesAsync(cancellationToken);
        }

        public async Task<IndiceConfortoDto> CalcularIndiceConfortoAsync(int id, DateTime inicio, DateTime fim, CancellationToken cancellationToken = default)
        {
            var resultado = await _estacaoRepository.CalcularIndiceConfortoAsync(id, inicio, fim, cancellationToken);
            return new IndiceConfortoDto
            {
                EstacaoId = id,
                Inicio = inicio,
                Fim = fim,
                IndiceConforto = resultado
            };
        }

        public async Task<TaxaOcupacaoDto> CalcularTaxaOcupacaoAsync(int id, DateTime inicio, DateTime fim, CancellationToken cancellationToken = default)
        {
            var resultado = await _estacaoRepository.CalcularTaxaOcupacaoAsync(id, inicio, fim, cancellationToken);
            return new TaxaOcupacaoDto
            {
                EstacaoId = id,
                Inicio = inicio,
                Fim = fim,
                TaxaOcupacao = resultado
            };
        }

        private static EstacaoTrabalhoDto Map(EstacaoTrabalho estacao)
        {
            return new EstacaoTrabalhoDto
            {
                Id = estacao.Id,
                CodigoEstacao = estacao.CodigoEstacao,
                ZonaSensorialId = estacao.ZonaSensorialId,
                Status = estacao.StatusEstacao,
                PermiteReserva = estacao.PermiteReserva,
                PermiteUsoEspontaneo = estacao.PermiteUsoEspontaneo,
                Links = new[]
                {
                    new LinkDto { Rel = "self", Href = $"/api/estacoes/{estacao.Id}", Method = "GET" },
                    new LinkDto { Rel = "reservas", Href = $"/api/estacoes/{estacao.Id}/reservas", Method = "GET" },
                    new LinkDto { Rel = "indice_conforto", Href = $"/api/estacoes/{estacao.Id}/indice-conforto", Method = "GET" },
                    new LinkDto { Rel = "taxa_ocupacao", Href = $"/api/estacoes/{estacao.Id}/taxa-ocupacao", Method = "GET" }
                }
            };
        }
    }
}
