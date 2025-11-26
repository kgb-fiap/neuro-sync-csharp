using System;
using System.ComponentModel.DataAnnotations;

namespace NeuroSync.Application.DTOs.Preferencias
{
    public class CreatePreferenciaSensorialDto
    {
        [Required]
        public int UsuarioId { get; set; }

        [Required]
        public DateTime DataInicioVigencia { get; set; }

        public decimal? RuidoMaxDb { get; set; }
        public decimal? LuzMinLux { get; set; }
        public decimal? LuzMaxLux { get; set; }
        public int? ToleranciaVisual { get; set; }
        [StringLength(30)]
        public string? PrefereZona { get; set; }
        [StringLength(500)]
        public string? ObservacoesPreferencia { get; set; }
    }
}
