using System.Collections.Generic;

namespace NeuroSync.Domain.Entities
{
    public class TipoSensor : BaseEntity
    {
        public string NomeTipoSensor { get; private set; } = string.Empty;
        public string? UnidadeMedida { get; private set; }
        public string? Descricao { get; private set; }

        public ICollection<Sensor> Sensores { get; private set; } = new List<Sensor>();

        public TipoSensor(string nomeTipoSensor)
        {
            NomeTipoSensor = nomeTipoSensor;
        }

        protected TipoSensor() { }
    }
}
