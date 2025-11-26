using System;
using System.Collections.Generic;

namespace NeuroSync.Domain.Entities
{
    public class Sensor : BaseEntity
    {
        public int EstacaoTrabalhoId { get; private set; }
        public int TipoSensorId { get; private set; }
        public string IdentificadorHardware { get; private set; } = string.Empty;
        public DateTime? DataInstalacao { get; private set; }
        public DateTime? DataUltimaManutencao { get; private set; }
        public string StatusSensor { get; private set; } = "ATIVO";
        public string? Observacoes { get; private set; }

        public EstacaoTrabalho? EstacaoTrabalho { get; private set; }
        public TipoSensor? TipoSensor { get; private set; }
        public ICollection<LeituraSensor> Leituras { get; private set; } = new List<LeituraSensor>();

        protected Sensor() { }

        public Sensor(int estacaoId, int tipoSensorId, string identificadorHardware)
        {
            EstacaoTrabalhoId = estacaoId;
            TipoSensorId = tipoSensorId;
            IdentificadorHardware = identificadorHardware;
        }
    }
}
