using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NeuroSync.Application.Common;
using NeuroSync.Application.DTOs.Reservas;
using NeuroSync.Application.Responses;
using NeuroSync.Domain.Entities;
using NeuroSync.Domain.Repositories;

namespace NeuroSync.Application.Services
{
    public class ReservaService : IReservaService
    {
        private readonly IReservaEstacaoRepository _reservaRepository;
        private readonly IEstacaoTrabalhoRepository _estacaoRepository;
        private readonly IRepository<AvaliacaoEstacao> _avaliacaoRepository;
        private readonly ILogger<ReservaService> _logger;

        public ReservaService(
            IReservaEstacaoRepository reservaRepository,
            IEstacaoTrabalhoRepository estacaoRepository,
            IRepository<AvaliacaoEstacao> avaliacaoRepository,
            ILogger<ReservaService> logger)
        {
            _reservaRepository = reservaRepository;
            _estacaoRepository = estacaoRepository;
            _avaliacaoRepository = avaliacaoRepository;
            _logger = logger;
        }

        public async Task<ReservaEstacaoDto> CriarAsync(CreateReservaEstacaoDto dto, CancellationToken cancellationToken = default)
        {
            if (dto.DataHoraFimPrevista <= dto.DataHoraInicioPrevista)
            {
                throw new BusinessException("Data/hora final deve ser maior que a data/hora inicial.");
            }

            var reserva = new ReservaEstacao(dto.UsuarioId, dto.EstacaoTrabalhoId, dto.StatusReservaId, dto.DataHoraInicioPrevista, dto.DataHoraFimPrevista)
            {
                OrigemReserva = dto.OrigemReserva,
                Observacoes = dto.Observacoes
            };

            await _reservaRepository.AddAsync(reserva, cancellationToken);
            await _reservaRepository.SaveChangesAsync(cancellationToken);

            return Map(reserva);
        }

        public async Task<PagedResult<ReservaEstacaoDto>> BuscarAsync(ReservaSearchRequest request, CancellationToken cancellationToken = default)
        {
            var query = _reservaRepository.Query().AsNoTracking();

            if (request.UsuarioId.HasValue)
            {
                query = query.Where(r => r.UsuarioId == request.UsuarioId);
            }

            if (request.EstacaoTrabalhoId.HasValue)
            {
                query = query.Where(r => r.EstacaoTrabalhoId == request.EstacaoTrabalhoId);
            }

            if (request.StatusReservaId.HasValue)
            {
                query = query.Where(r => r.StatusReservaId == request.StatusReservaId);
            }

            if (request.Inicio.HasValue)
            {
                query = query.Where(r => r.DataHoraInicioPrevista >= request.Inicio.Value);
            }

            if (request.Fim.HasValue)
            {
                query = query.Where(r => r.DataHoraFimPrevista <= request.Fim.Value);
            }

            query = query.OrderByDescending(r => r.DataHoraInicioPrevista);

            var total = await query.CountAsync(cancellationToken);
            var reservas = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<ReservaEstacaoDto>
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalRecords = total,
                Items = reservas.Select(Map).ToList()
            };
        }

        public async Task<ReservaEstacaoDto?> ObterAsync(int id, CancellationToken cancellationToken = default)
        {
            var reserva = await _reservaRepository.GetByIdAsync(id, cancellationToken);
            return reserva is null ? null : Map(reserva);
        }

        public async Task AtualizarStatusAsync(int id, int novoStatusId, string? motivoCancelamento, CancellationToken cancellationToken = default)
        {
            var reserva = await _reservaRepository.GetByIdAsync(id, cancellationToken);
            if (reserva == null)
            {
                throw new BusinessException("Reserva não encontrada.", HttpStatusCode.NotFound);
            }

            reserva.AtualizarStatus(novoStatusId, motivoCancelamento);
            _reservaRepository.Update(reserva);
            await _reservaRepository.SaveChangesAsync(cancellationToken);
        }

        public async Task<CompatibilidadeReservaDto> VerificarCompatibilidadeAsync(int id, CancellationToken cancellationToken = default)
        {
            var compativel = await _reservaRepository.ReservaCompativelAsync(id, cancellationToken);
            return new CompatibilidadeReservaDto { ReservaId = id, Compativel = compativel };
        }

        public async Task RegistrarAvaliacaoAsync(AvaliacaoEstacaoDto dto, CancellationToken cancellationToken = default)
        {
            var reserva = await _reservaRepository.GetByIdAsync(dto.ReservaEstacaoId, cancellationToken);
            if (reserva == null)
            {
                throw new BusinessException("Reserva não encontrada para avaliação.", HttpStatusCode.NotFound);
            }

            var avaliacao = new AvaliacaoEstacao(dto.ReservaEstacaoId, dto.NotaConfortoGeral)
            {
                NotaRuido = dto.NotaRuido,
                NotaLuz = dto.NotaLuz,
                NotaEstimuloVisual = dto.NotaEstimuloVisual,
                Comentario = dto.Comentario,
                DataAvaliacao = dto.DataAvaliacao
            };

            await _avaliacaoRepository.AddAsync(avaliacao, cancellationToken);
            await _avaliacaoRepository.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoverAsync(int id, CancellationToken cancellationToken = default)
        {
            var reserva = await _reservaRepository.GetByIdAsync(id, cancellationToken);
            if (reserva == null)
            {
                throw new BusinessException("Reserva não encontrada.", HttpStatusCode.NotFound);
            }

            var avaliacao = await _avaliacaoRepository.Query()
                .FirstOrDefaultAsync(a => a.ReservaEstacaoId == id, cancellationToken);
            if (avaliacao != null)
            {
                _avaliacaoRepository.Remove(avaliacao);
            }

            _reservaRepository.Remove(reserva);
            await _reservaRepository.SaveChangesAsync(cancellationToken);
        }

        private static ReservaEstacaoDto Map(ReservaEstacao reserva)
        {
            return new ReservaEstacaoDto
            {
                Id = reserva.Id,
                UsuarioId = reserva.UsuarioId,
                EstacaoTrabalhoId = reserva.EstacaoTrabalhoId,
                StatusReservaId = reserva.StatusReservaId,
                DataHoraInicioPrevista = reserva.DataHoraInicioPrevista,
                DataHoraFimPrevista = reserva.DataHoraFimPrevista,
                OrigemReserva = reserva.OrigemReserva,
                IndiceConforto = reserva.IndiceConfortoCalculado,
                Links = new[]
                {
                    new LinkDto { Rel = "self", Href = $"/api/reservas/{reserva.Id}", Method = "GET" },
                    new LinkDto { Rel = "estacao", Href = $"/api/estacoes/{reserva.EstacaoTrabalhoId}", Method = "GET" },
                    new LinkDto { Rel = "avaliacao", Href = $"/api/reservas/{reserva.Id}/avaliacao", Method = "POST" }
                }
            };
        }
    }
}
