using System.Collections.Generic;

namespace NeuroSync.Domain.Entities
{
    public class Empresa : BaseEntity
    {
        public string RazaoSocial { get; private set; } = string.Empty;
        public string? NomeFantasia { get; private set; }
        public string Cnpj { get; private set; } = string.Empty;
        public string? EmailCorporativo { get; private set; }
        public string? TelefoneCorporativo { get; private set; }

        public ICollection<Filial> Filiais { get; private set; } = new List<Filial>();

        public Empresa(string razaoSocial, string cnpj)
        {
            RazaoSocial = razaoSocial;
            Cnpj = cnpj;
        }

        protected Empresa() { }
    }
}
