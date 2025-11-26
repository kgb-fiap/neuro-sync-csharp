using System;

namespace NeuroSync.Application.DTOs.Reservas
{
    public class ReservaSearchRequest : PagedRequest
    {
        public int? UsuarioId { get; set; }
        public int? EstacaoTrabalhoId { get; set; }
        public int? StatusReservaId { get; set; }
        public DateTime? Inicio { get; set; }
        public DateTime? Fim { get; set; }
    }
}
