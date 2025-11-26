using System;

namespace NeuroSync.Application.DTOs.Preferencias
{
    public class PreferenciaSensorialDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public decimal? RuidoMaxDb { get; set; }
        public decimal? LuzMinLux { get; set; }
        public decimal? LuzMaxLux { get; set; }
        public int? ToleranciaVisual { get; set; }
        public string? PrefereZona { get; set; }
        public string? Observacoes { get; set; }
        public DateTime DataInicioVigencia { get; set; }
        public DateTime? DataFimVigencia { get; set; }
        public bool Ativo { get; set; }
    }
}
