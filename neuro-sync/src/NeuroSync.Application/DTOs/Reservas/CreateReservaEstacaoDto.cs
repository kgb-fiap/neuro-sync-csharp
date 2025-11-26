using System;
using System.ComponentModel.DataAnnotations;

namespace NeuroSync.Application.DTOs.Reservas
{
    public class CreateReservaEstacaoDto
    {
        [Required]
        public int UsuarioId { get; set; }

        [Required]
        public int EstacaoTrabalhoId { get; set; }

        [Required]
        public int StatusReservaId { get; set; }

        [Required]
        public DateTime DataHoraInicioPrevista { get; set; }

        [Required]
        public DateTime DataHoraFimPrevista { get; set; }

        [StringLength(20)]
        public string? OrigemReserva { get; set; }

        [StringLength(500)]
        public string? Observacoes { get; set; }
    }
}
