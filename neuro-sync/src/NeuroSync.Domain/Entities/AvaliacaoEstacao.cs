using System;

namespace NeuroSync.Domain.Entities
{
    public class AvaliacaoEstacao : BaseEntity
    {
        public int ReservaEstacaoId { get; private set; }
        public int NotaConfortoGeral { get; private set; }
        public int? NotaRuido { get; set; }
        public int? NotaLuz { get; set; }
        public int? NotaEstimuloVisual { get; set; }
        public string? Comentario { get; set; }
        public DateTime DataAvaliacao { get; set; } = DateTime.UtcNow;

        public ReservaEstacao? Reserva { get; private set; }

        protected AvaliacaoEstacao() { }

        public AvaliacaoEstacao(int reservaId, int notaConfortoGeral)
        {
            ReservaEstacaoId = reservaId;
            NotaConfortoGeral = notaConfortoGeral;
        }
    }
}
