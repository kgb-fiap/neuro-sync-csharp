using System;

namespace NeuroSync.Domain.Entities
{
    public class LeituraSensor : BaseEntity
    {
        public int SensorId { get; private set; }
        public DateTime DataHoraLeitura { get; private set; }
        public decimal ValorMedido { get; private set; }
        public int? QualidadeSinal { get; private set; }
        public string? OrigemRegistro { get; private set; }
        public DateTime DataProcessamento { get; private set; } = DateTime.UtcNow;

        public Sensor? Sensor { get; private set; }

        protected LeituraSensor() { }

        public LeituraSensor(int sensorId, DateTime dataHoraLeitura, decimal valorMedido)
        {
            SensorId = sensorId;
            DataHoraLeitura = dataHoraLeitura;
            ValorMedido = valorMedido;
        }
    }
}
