using System.Collections.Generic;

namespace NeuroSync.Domain.Entities
{
    public class ZonaSensorial : BaseEntity
    {
        public int FilialId { get; private set; }
        public string NomeZona { get; private set; } = string.Empty;
        public string? TipoZona { get; private set; }
        public string? Descricao { get; private set; }
        public decimal? RuidoMedioEstimadoDb { get; private set; }
        public decimal? LuzMediaEstimadoLux { get; private set; }
        public string? CaracteristicaVisual { get; private set; }
        public int? CapacidadeEstimada { get; private set; }

        public Filial? Filial { get; private set; }
        public ICollection<EstacaoTrabalho> Estacoes { get; private set; } = new List<EstacaoTrabalho>();

        protected ZonaSensorial() { }

        public ZonaSensorial(int filialId, string nomeZona)
        {
            FilialId = filialId;
            NomeZona = nomeZona;
        }
    }
}
