using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NeuroSync.Application.DTOs.Preferencias;
using NeuroSync.Domain.Entities;
using NeuroSync.Domain.Repositories;

namespace NeuroSync.Application.Services
{
    public class PreferenciaSensorialService : IPreferenciaSensorialService
    {
        private readonly IRepository<PreferenciaSensorial> _preferenciaRepository;

        public PreferenciaSensorialService(IRepository<PreferenciaSensorial> preferenciaRepository)
        {
            _preferenciaRepository = preferenciaRepository;
        }

        public async Task<PreferenciaSensorialDto> CriarAsync(CreatePreferenciaSensorialDto dto, CancellationToken cancellationToken = default)
        {
            var preferencia = new PreferenciaSensorial(dto.UsuarioId, dto.DataInicioVigencia)
            {
                RuidoMaxDb = dto.RuidoMaxDb,
                LuzMinLux = dto.LuzMinLux,
                LuzMaxLux = dto.LuzMaxLux,
                ToleranciaVisual = dto.ToleranciaVisual,
                PrefereZona = dto.PrefereZona,
                ObservacoesPreferencia = dto.ObservacoesPreferencia
            };

            await _preferenciaRepository.AddAsync(preferencia, cancellationToken);
            await _preferenciaRepository.SaveChangesAsync(cancellationToken);

            return Map(preferencia);
        }

        public async Task<IEnumerable<PreferenciaSensorialDto>> ObterPorUsuarioAsync(int usuarioId, CancellationToken cancellationToken = default)
        {
            var lista = await _preferenciaRepository.Query()
                .Where(p => p.UsuarioId == usuarioId)
                .OrderByDescending(p => p.DataInicioVigencia)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return lista.Select(Map).ToList();
        }

        private static PreferenciaSensorialDto Map(PreferenciaSensorial preferencia)
        {
            return new PreferenciaSensorialDto
            {
                Id = preferencia.Id,
                UsuarioId = preferencia.UsuarioId,
                RuidoMaxDb = preferencia.RuidoMaxDb,
                LuzMinLux = preferencia.LuzMinLux,
                LuzMaxLux = preferencia.LuzMaxLux,
                ToleranciaVisual = preferencia.ToleranciaVisual,
                PrefereZona = preferencia.PrefereZona,
                Observacoes = preferencia.ObservacoesPreferencia,
                DataInicioVigencia = preferencia.DataInicioVigencia,
                DataFimVigencia = preferencia.DataFimVigencia,
                Ativo = preferencia.Ativo
            };
        }
    }
}
