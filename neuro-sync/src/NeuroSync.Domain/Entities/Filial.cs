using System.Collections.Generic;

namespace NeuroSync.Domain.Entities
{
    public class Filial : BaseEntity
    {
        public int EmpresaId { get; private set; }
        public string NomeFilial { get; private set; } = string.Empty;
        public string? CodigoFilial { get; private set; }
        public string? Cidade { get; private set; }
        public string? Uf { get; private set; }
        public string? Pais { get; private set; }
        public string? Endereco { get; private set; }

        public Empresa? Empresa { get; private set; }
        public ICollection<Setor> Setores { get; private set; } = new List<Setor>();
        public ICollection<ZonaSensorial> Zonas { get; private set; } = new List<ZonaSensorial>();

        public Filial(int empresaId, string nomeFilial)
        {
            EmpresaId = empresaId;
            NomeFilial = nomeFilial;
        }

        protected Filial() { }
    }
}
