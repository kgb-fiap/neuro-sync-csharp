using System;

namespace NeuroSync.Domain.Entities
{
    public class ReservaEstacao : BaseEntity
    {
        public int UsuarioId { get; set; }
        public int EstacaoTrabalhoId { get; set; }
        public int StatusReservaId { get; private set; }
        public DateTime DataHoraSolicitacao { get; set; } = DateTime.UtcNow;
        public DateTime DataHoraInicioPrevista { get; set; }
        public DateTime DataHoraFimPrevista { get; set; }
        public DateTime? DataHoraCheckin { get; private set; }
        public DateTime? DataHoraCheckout { get; private set; }
        public string? OrigemReserva { get; set; }
        public string? MotivoCancelamento { get; private set; }
        public decimal? IndiceConfortoCalculado { get; private set; }
        public string? Observacoes { get; set; }

        public Usuario? Usuario { get; private set; }
        public EstacaoTrabalho? EstacaoTrabalho { get; private set; }
        public StatusReserva? Status { get; private set; }
        public AvaliacaoEstacao? Avaliacao { get; private set; }

        protected ReservaEstacao() { }

        public ReservaEstacao(int usuarioId, int estacaoId, int statusId, DateTime inicio, DateTime fim)
        {
            UsuarioId = usuarioId;
            EstacaoTrabalhoId = estacaoId;
            StatusReservaId = statusId;
            DataHoraInicioPrevista = inicio;
            DataHoraFimPrevista = fim;
        }

        public void RegistrarCheckin(DateTime data) => DataHoraCheckin = data;

        public void RegistrarCheckout(DateTime data) => DataHoraCheckout = data;

        public void AtualizarStatus(int statusId, string? motivoCancelamento = null)
        {
            StatusReservaId = statusId;
            MotivoCancelamento = motivoCancelamento;
        }

        public void DefinirIndiceConforto(decimal indice) => IndiceConfortoCalculado = indice;
    }
}
