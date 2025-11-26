using System.Collections.Generic;

namespace NeuroSync.Domain.Entities
{
    public class Setor : BaseEntity
    {
        public int FilialId { get; private set; }
        public string NomeSetor { get; private set; } = string.Empty;
        public string? CodigoSetor { get; private set; }
        public string? Andar { get; private set; }
        public string? Descricao { get; private set; }

        public Filial? Filial { get; private set; }
        public ICollection<Usuario> Usuarios { get; private set; } = new List<Usuario>();

        public Setor(int filialId, string nomeSetor)
        {
            FilialId = filialId;
            NomeSetor = nomeSetor;
        }

        protected Setor() { }
    }
}
