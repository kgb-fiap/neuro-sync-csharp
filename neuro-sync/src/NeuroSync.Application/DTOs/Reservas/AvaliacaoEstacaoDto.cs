using System;
using System.ComponentModel.DataAnnotations;

namespace NeuroSync.Application.DTOs.Reservas
{
    public class AvaliacaoEstacaoDto
    {
        [Required]
        public int ReservaEstacaoId { get; set; }

        [Range(1, 5)]
        public int NotaConfortoGeral { get; set; }

        [Range(1, 5)]
        public int? NotaRuido { get; set; }

        [Range(1, 5)]
        public int? NotaLuz { get; set; }

        [Range(1, 5)]
        public int? NotaEstimuloVisual { get; set; }

        [StringLength(500)]
        public string? Comentario { get; set; }

        public DateTime DataAvaliacao { get; set; } = DateTime.UtcNow;
    }
}
