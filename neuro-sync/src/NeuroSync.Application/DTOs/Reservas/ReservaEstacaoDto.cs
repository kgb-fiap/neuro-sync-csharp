using System;
using System.Collections.Generic;
using NeuroSync.Application.Responses;

namespace NeuroSync.Application.DTOs.Reservas
{
    public class ReservaEstacaoDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int EstacaoTrabalhoId { get; set; }
        public int StatusReservaId { get; set; }
        public DateTime DataHoraInicioPrevista { get; set; }
        public DateTime DataHoraFimPrevista { get; set; }
        public string? OrigemReserva { get; set; }
        public decimal? IndiceConforto { get; set; }
        public IEnumerable<LinkDto> Links { get; set; } = new List<LinkDto>();
    }
}
