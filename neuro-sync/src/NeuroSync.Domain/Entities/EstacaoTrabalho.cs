using System.Collections.Generic;

namespace NeuroSync.Domain.Entities
{
    public class EstacaoTrabalho : BaseEntity
    {
        public int ZonaSensorialId { get; set; }
        public string CodigoEstacao { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public bool PermiteReserva { get; set; } = true;
        public bool PermiteUsoEspontaneo { get; set; } = true;
        public string StatusEstacao { get; set; } = "ATIVA";
        public string? Observacoes { get; set; }

        public ZonaSensorial? ZonaSensorial { get; private set; }
        public ICollection<Sensor> Sensores { get; private set; } = new List<Sensor>();
        public ICollection<ReservaEstacao> Reservas { get; private set; } = new List<ReservaEstacao>();

        protected EstacaoTrabalho() { }

        public EstacaoTrabalho(int zonaSensorialId, string codigoEstacao)
        {
            ZonaSensorialId = zonaSensorialId;
            CodigoEstacao = codigoEstacao;
        }

        public void AlterarStatus(string novoStatus)
        {
            StatusEstacao = novoStatus;
        }
    }
}
