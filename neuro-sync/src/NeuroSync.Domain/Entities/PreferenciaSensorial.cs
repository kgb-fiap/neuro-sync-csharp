using System;

namespace NeuroSync.Domain.Entities
{
    public class PreferenciaSensorial : BaseEntity
    {
        public int UsuarioId { get; set; }
        public decimal? RuidoMaxDb { get; set; }
        public decimal? LuzMinLux { get; set; }
        public decimal? LuzMaxLux { get; set; }
        public int? ToleranciaVisual { get; set; }
        public string? PrefereZona { get; set; }
        public string? ObservacoesPreferencia { get; set; }
        public DateTime DataInicioVigencia { get; set; }
        public DateTime? DataFimVigencia { get; set; }

        public Usuario? Usuario { get; private set; }

        protected PreferenciaSensorial() { }

        public PreferenciaSensorial(int usuarioId, DateTime inicioVigencia)
        {
            UsuarioId = usuarioId;
            DataInicioVigencia = inicioVigencia;
        }
    }
}
