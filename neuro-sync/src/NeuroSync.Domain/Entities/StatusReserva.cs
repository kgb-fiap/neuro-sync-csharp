using System.Collections.Generic;

namespace NeuroSync.Domain.Entities
{
    public class StatusReserva : BaseEntity
    {
        public string CodigoStatus { get; private set; } = string.Empty;
        public string? Descricao { get; private set; }
        public bool Finalizador { get; private set; }

        public ICollection<ReservaEstacao> Reservas { get; private set; } = new List<ReservaEstacao>();

        protected StatusReserva() { }

        public StatusReserva(string codigoStatus, bool finalizador = false)
        {
            CodigoStatus = codigoStatus;
            Finalizador = finalizador;
        }
    }
}
