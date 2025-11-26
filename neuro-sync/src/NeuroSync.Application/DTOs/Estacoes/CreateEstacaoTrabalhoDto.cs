using System.ComponentModel.DataAnnotations;

namespace NeuroSync.Application.DTOs.Estacoes
{
    public class CreateEstacaoTrabalhoDto
    {
        [Required]
        public int ZonaSensorialId { get; set; }

        [Required, StringLength(50)]
        public string CodigoEstacao { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Descricao { get; set; }

        public bool PermiteReserva { get; set; } = true;
        public bool PermiteUsoEspontaneo { get; set; } = true;

        [Required, StringLength(20)]
        public string StatusEstacao { get; set; } = "ATIVA";

        [StringLength(500)]
        public string? Observacoes { get; set; }
    }
}
